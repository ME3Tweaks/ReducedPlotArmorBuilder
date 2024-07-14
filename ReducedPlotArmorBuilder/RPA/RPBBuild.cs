// Uncomment this to make death values always 1 for debugging.
#define FORCEDEATH

using System.Diagnostics;
using LegendaryExplorerCore.Kismet;
using LegendaryExplorerCore.Packages;
using LegendaryExplorerCore.Packages.CloningImportingAndRelinking;
using LegendaryExplorerCore.TLK;
using LegendaryExplorerCore.TLK.ME2ME3;
using LegendaryExplorerCore.Unreal;
using LegendaryExplorerCore.Unreal.BinaryConverters;
using LegendaryExplorerCore.Unreal.ObjectInfo;

namespace ReducedPlotArmorBuilder.RPA
{
    public static class RPABuild
    {

        /// <summary>
        /// The final usable TLK ID (first starter kit string)
        /// </summary>
        private const int END_OF_TLK_FILE_ID = 3248044;
        /// <summary>
        /// Precomputed files
        /// </summary>
        private static string StaticFilesFolder;

        /// <summary>
        /// The folder that contains the mod's TLKs (vanilla)
        /// </summary>
        private static string TLKSourceDirectory;

        /// <summary>
        /// The folder that contains helper packages
        /// </summary>
        private static string HelperFilesFolder;

        /// <summary>
        /// The folder that contains files to edit
        /// </summary>
        private static string BaseFilesFolder;

        /// <summary>
        /// The folder that contains alternates to patch
        /// </summary>
        private static string PatchSourceBaseFolder;

        /// <summary>
        /// The final destination folder
        /// </summary>
        private static string OutputFolder;


        /// <summary>
        /// Sequence for porting in to do a death roll
        /// </summary>
        private static ExportEntry RPADeathRollSequence;


        /// <summary>
        /// The ID of the current TLK to assign
        /// </summary>
        private static int TLKAssignmentID;

        /// <summary>
        /// The map of loaded talk files
        /// </summary>
        private static Dictionary<MELocalization, ME2ME3TalkFile> TalkFileMap;


        public static void RunRPABuild(string sourceDirectory, string outputDirectory)
        {
            OutputFolder = outputDirectory;

            BaseFilesFolder = Path.Combine(sourceDirectory, "Basefiles");
            HelperFilesFolder = Path.Combine(sourceDirectory, "Helpers");
            StaticFilesFolder = Path.Combine(sourceDirectory, "Static");
            TLKSourceDirectory = Path.Combine(sourceDirectory, "TLK");
            PatchSourceBaseFolder = Path.Combine(sourceDirectory, "Patches");

            DeleteExistingFiles();


            // Initialize
            CopyPrecomputed();
            #region Sequence stuff
            var helperPackage = MEPackageHandler.OpenMEPackage(Path.Combine(HelperFilesFolder, "SequencePrefabs.pcc"));
            RPADeathRollSequence = helperPackage.FindExport("ReducedPlotArmorSequences.RPADeathRoll");


            var classHelper = MEPackageHandler.OpenMEPackage(Path.Combine(HelperFilesFolder, "CustomClasses.pcc"));
            foreach (var classX in classHelper.Exports.Where(x => x.IsClass))
            {
                var classInfo = GlobalUnrealObjectInfo.generateClassInfo(classX);
                var defaults = classX.FileRef.GetUExport(ObjectBinary.From<UClass>(classX).Defaults);
                Console.WriteLine($@"  Inventorying class {classX.InstancedFullPath}");
                GlobalUnrealObjectInfo.GenerateSequenceObjectInfoForClassDefaults(defaults);
                GlobalUnrealObjectInfo.InstallCustomClassInfo(classX.ObjectName, classInfo, classX.Game);
            }
            RPADeathRollSequence = helperPackage.FindExport("ReducedPlotArmorSequences.RPADeathRoll");

            #endregion
            #region TLK
            TalkFileMap = new Dictionary<MELocalization, ME2ME3TalkFile>();
            var tlkSources = Directory.GetFiles(TLKSourceDirectory);
            foreach (var tlk in tlkSources)
            {
                var loc = tlk.GetUnrealLocalization();
                TalkFileMap[loc] = new ME2ME3TalkFile(tlk);
            }
            #endregion

            // Build
            InstallFixedTLKStrings();

            var rpaedits = RPAList.GetRPAEdits();
            TLKAssignmentID = TLKAssignmentID - rpaedits.Count - 1;
            foreach (var edit in rpaedits)
            {
                ApplyRPAEdit(edit);
            }


            // Finalize
            foreach (var tlk in TalkFileMap)
            {
                var outPath = Path.Combine(OutputFolder, $"DLC_MOD_ReducedPlotArmor_{tlk.Key}.tlk");
                Console.WriteLine($"Saving TLK {Path.GetFileName(outPath)}");
                HuffmanCompression.SaveToTlkFile(outPath, tlk.Value.StringRefs);
            }
        }

        private static void InstallFixedTLKStrings()
        {
            TLKAssignmentID = END_OF_TLK_FILE_ID - 1;
            Dictionary<MELocalization, string[]> stringsToAssign = new Dictionary<MELocalization, string[]>();
            stringsToAssign[MELocalization.INT] = new[]
            {
                "Reduced Plot Armor Settings",
                "Configure options for Reduced Plot Armor",
                "Hard Mode: Enabled",
                "Hard Mode: Disabled",
                "Increases likeliness of death and adds additional plot points that may result in death.",
                "Your plot armor prevailed",
                "Suppress survival messages: Enabled",
                "Suppress survival messages: Disabled",
                "Hides messages that show up on screen when you survive a plot armor check.",
                "Your luck finally ran out"
            };

            //stringsToAssign[MELocalization.ESN] = new[]
            //{
            //    "Reduced Plot Armor Settings",
            //    "POOP",
            //    "Hard Mode: Enabled",
            //    "Hard Mode: Disabled",
            //    "Increases likeliness of death and adds additional plot points that may result in death.",
            //    "JAZZ",
            //    "Hides messages that show up on screen when you survive a plot armor check.",
            //    "Your luck finally ran out"
            //};

            int index = 0;
            var intLoc = stringsToAssign[MELocalization.INT]; //fallback value
            foreach (var str in intLoc)
            {
                foreach (var loc in TalkFileMap.Keys)
                {
                    string locStr = intLoc[index];
                    if (stringsToAssign.TryGetValue(loc, out var foundLoc) && foundLoc.Length > index)
                    {
                        // We have a localized string in this index
                        locStr = foundLoc[index];
                    }

                    TalkFileMap[loc].AddString(new TLKStringRef(TLKAssignmentID, locStr));
                }
                TLKAssignmentID--; // Next TLK ID
                index++; // Next item in RPA list of strings
            }
        }

        private static void CopyPrecomputed()
        {
            var staticFiles = Directory.GetFiles(StaticFilesFolder);
            foreach (var sf in staticFiles)
            {
#if !DEBUG
                if (Path.GetExtension(sf) == ".xml")
                {
                    Console.WriteLine($"File not for ship: {sf}, skipping");
                    continue;
                }
#endif
                var outpath = Path.Combine(OutputFolder, Path.GetFileName(sf));
                Console.WriteLine($"Copying static file {Path.GetFileName(sf)}");

                File.Copy(sf, outpath);
            }
        }

        private static void ApplyRPAEdit(RPAEdit edit)
        {
            // Add TLK string if any
            if (edit.INTReason != null)
            {
                TLKAssignmentID++; // Increment the current TLK
                TalkFileMap[MELocalization.INT].AddString(new TLKStringRef(TLKAssignmentID, edit.INTReason));
                TalkFileMap[MELocalization.ESN].AddString(new TLKStringRef(TLKAssignmentID, edit.ESNReason ?? edit.INTReason));
                TalkFileMap[MELocalization.DEU].AddString(new TLKStringRef(TLKAssignmentID, edit.DEUReason ?? edit.INTReason));
                TalkFileMap[MELocalization.RUS].AddString(new TLKStringRef(TLKAssignmentID, edit.RUSReason ?? edit.INTReason));
                TalkFileMap[MELocalization.POL].AddString(new TLKStringRef(TLKAssignmentID, edit.POLReason ?? edit.INTReason));
                TalkFileMap[MELocalization.FRA].AddString(new TLKStringRef(TLKAssignmentID, edit.FRAReason ?? edit.INTReason));
                TalkFileMap[MELocalization.ITA].AddString(new TLKStringRef(TLKAssignmentID, edit.ITAReason ?? edit.INTReason));
                TalkFileMap[MELocalization.JPN].AddString(new TLKStringRef(TLKAssignmentID, edit.JPNReason ?? edit.INTReason));
            }


            // Localized files.
            var files = new List<string>();
            if (edit.IsLocalized)
            {
                files.Add($"{edit.SourceFilename}_LOC_INT");
                files.Add($"{edit.SourceFilename}_LOC_FRA");
                files.Add($"{edit.SourceFilename}_LOC_DEU");
                files.Add($"{edit.SourceFilename}_LOC_ITA");
            }
            else
            {
                files.Add(edit.SourceFilename);
            }

            // Enumerate the files to edit.
            foreach (var f in files)
            {
                var sourcePath = Path.Combine(BaseFilesFolder, f + ".pcc");
                var outPath = Path.Combine(OutputFolder, f + ".pcc");

                ApplyEditInternal(edit, sourcePath, outPath);

                var filesToAlsoPatch = Directory.GetFiles(PatchSourceBaseFolder, "*.pcc", SearchOption.AllDirectories).Where(x => Path.GetFileNameWithoutExtension(x) == f);
                foreach (var ftap in filesToAlsoPatch)
                {
                    var modRoot = Directory.GetParent(OutputFolder)?.Parent;
                    if (modRoot != null && modRoot.Name == "Reduced Plot Armor") // Mod Library Folder
                    {
                        var alternatesRoot = Path.Combine(modRoot.FullName, "Alternates");
                        if (Directory.Exists(alternatesRoot))
                        {
                            var patchOutDir = Path.Combine(alternatesRoot, Directory.GetParent(ftap).Name, f +".pcc"); // Alternate folder
                            ApplyEditInternal(edit, ftap, patchOutDir);
                        }
                    }
                }

            }

        }

        private static void ApplyEditInternal(RPAEdit edit, string sourcePath, string outPath)
        {
            Console.WriteLine($"Processing {Path.GetFileNameWithoutExtension(sourcePath)}.pcc");

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"\tFile doesn't exist, skipping: {sourcePath}");
                return;
            }
            var package = MEPackageHandler.OpenMEPackage(sourcePath);

            var originator = package.FindExport(edit.StartKismetHookupIFP);
            var sequence = SeqTools.GetParentSequence(originator);

            List<ExportEntry> addedItems = new List<ExportEntry>();

            EntryImporter.ImportAndRelinkEntries(EntryImporter.PortingOption.CloneAllDependencies,
                RPADeathRollSequence, package, sequence, true, new RelinkerOptionsPackage(), out var portedRoll);
            var rollSeq = portedRoll as ExportEntry;
            EntryImporter.ReindexExportEntriesWithSamePath(rollSeq);
            addedItems.Add(rollSeq);

            var deathChance = SequenceObjectCreator.CreateSequenceObject(package, "SeqVar_Float");
            addedItems.Add(deathChance);
#if FORCEDEATH
            deathChance.WriteProperty(new FloatProperty(1, "FloatValue"));
#else
                deathChance.WriteProperty(new FloatProperty(edit.DeathChance, "FloatValue"));
#endif
            var tlkMsg = SequenceObjectCreator.CreateSequenceObject(package, "BioSeqVar_StrRef");
            addedItems.Add(tlkMsg);
            tlkMsg.WriteProperty(new StringRefProperty(TLKAssignmentID, "m_srValue"));

            var slomoSpeed = SequenceObjectCreator.CreateSequenceObject(package, "SeqVar_Float");
            addedItems.Add(slomoSpeed);
            slomoSpeed.WriteProperty(new FloatProperty(edit.SlomoSpeed, "FloatValue"));

            var hardModeOnly = SequenceObjectCreator.CreateSequenceObject(package, "SeqVar_Bool");
            addedItems.Add(hardModeOnly);
            hardModeOnly.WriteProperty(new IntProperty(edit.HardModeOnly ? 1 : 0, "bValue"));

            var ragdoll = SequenceObjectCreator.CreateSequenceObject(package, "SeqVar_Bool");
            addedItems.Add(ragdoll);
            ragdoll.WriteProperty(new IntProperty(edit.Ragdoll ? 1 : 0, "bValue"));

            var killPlayer = SequenceObjectCreator.CreateSequenceObject(package, "SeqVar_Bool");
            addedItems.Add(killPlayer);
            killPlayer.WriteProperty(new IntProperty(edit.KillPlayer ? 1 : 0, "bValue"));

            KismetHelper.CreateVariableLink(rollSeq, "DeathChance", deathChance);
            KismetHelper.CreateVariableLink(rollSeq, "DeathMessage", tlkMsg);
            KismetHelper.CreateVariableLink(rollSeq, "SlomoSpeed", slomoSpeed);
            KismetHelper.CreateVariableLink(rollSeq, "HardModeOnly", hardModeOnly);
            KismetHelper.CreateVariableLink(rollSeq, "Ragdoll", ragdoll);
            KismetHelper.CreateVariableLink(rollSeq, "KillPlayer", killPlayer);

            if (edit.RollDelay > 0)
            {
                // Use delay
                var delay = SequenceObjectCreator.CreateSequenceObject(package, "SeqAct_Delay");
                addedItems.Add(delay);
                delay.WriteProperty(new FloatProperty(edit.RollDelay, "Duration"));

                KismetHelper.CreateOutputLink(originator, edit.OriginatorOutlink, delay);
                KismetHelper.CreateOutputLink(delay, "Finished", rollSeq);
            }
            else
            {
                // Directly call
                KismetHelper.CreateOutputLink(originator, edit.StartKismetHookupIFP, rollSeq);
            }

            KismetHelper.AddObjectsToSequence(sequence, false, addedItems.ToArray());

            if (edit.UnskippableAnimSeqs != null)
            {
                foreach (var usas in edit.UnskippableAnimSeqs)
                {
                    package.FindExport(usas).RemoveProperty("bIsSkippable"); // Default to false.
                }
            }

            Console.WriteLine("\tSaving package");
            package.Save(outPath);
        }

        /// <summary>
        /// Cleans out the existing CookedPCConsole folder
        /// </summary>
        private static void DeleteExistingFiles()
        {
            var existingfiles = Directory.GetFiles(OutputFolder);
            foreach (var f in existingfiles)
            {
                Console.WriteLine($"Deleting existing file {Path.GetFileName(f)}");
                File.Delete(f);
            }
        }
    }
}
