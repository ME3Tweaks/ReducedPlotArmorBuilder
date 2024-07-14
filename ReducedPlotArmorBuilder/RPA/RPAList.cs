using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReducedPlotArmorBuilder.RPA
{
    /// <summary>
    /// Class for list of RPA definitions
    /// </summary>
    internal class RPAList
    {
        public static List<RPAEdit> GetRPAEdits()
        {
            List<RPAEdit> edits = new List<RPAEdit>();

            // Normal Mode

            // James crashes the shuttle into shepard on Mars
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_ProMar_740BossFight",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Shuttle_Crash_Cutscene.SeqEvent_SequenceActivated_0",
                OriginatorOutlink = "Out",
                RollDelay = 3.0f,
                SlomoSpeed = 0.7f,
                DeathChance = 0.25f,
                INTReason = "James killed you",
            });

            // Turian fighter crashes into shepard on Tuchanka
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Kro002_400highway",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.CineCrash.ANIMCUTSCENE_Kro002_Convoy_pt1.SFXSeqAct_SetWeaponVisibility_1",
                OriginatorOutlink = "Out",
                RollDelay = 49.3f,
                DeathChance = 0.3f,
                SlomoSpeed = 0.4f,
                INTReason = "You died in the explosion",
            });

            // Shepard uploads into the matrix
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_GthLeg_200Server_In",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.ANIMCUTSCENE_GthLeg_Jackin.SFXSeqAct_SetWeaponVisibility_0",
                OriginatorOutlink = "Out",
                RollDelay = 36.3f,
                DeathChance = 0.1f,
                INTReason = "Error 500: Internal Server Error",
                SlomoSpeed = 0.25f,
                Ragdoll = true
            });

            // Rannoch fall after laser shot
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Gth002_220ReaperAwakes",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Level_Events.CinDes_Shepard_Falls.SeqAct_ToggleHidden_2",
                OriginatorOutlink = "Out",
                RollDelay = 9.6f,
                DeathChance = 0.2f,
                Ragdoll = true,
                SlomoSpeed = 0.25f,
                INTReason = "You broke your spine",
            });

            // Gunship shoots at shepard (Thessia)
            // Disabled: doesn't look that good and it gives him two chances to fight against you
            //edits.Add(new RPAEdit()
            //{
            //    SourceFilename = "BioD_Cat002_700PostKLFight",
            //    StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.SEQ_Ruined_Catherdral.Shepard_Falls_Cutscene.ANIMCUTSCENE_Cat002_ShepFalls_part1.SFXSeqAct_ToggleKaiLengSword_0",
            //    OriginatorOutlink = "Out",
            //    RollDelay = 23.4f,
            //    SlomoSpeed = 0.5f,
            //    DeathChance = 0.27f,
            //    INTReason = "You were killed",
            //});

            // Shepard falls into hole (Thessia)
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Cat002_700PostKLFight",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.SEQ_Ruined_Catherdral.Shepard_Falls_Cutscene.ANIMCUTSCENE_Cat002_ShepFalls_part1.SFXSeqAct_ToggleKaiLengSword_0",
                OriginatorOutlink = "Out",
                RollDelay = 44.5f,
                DeathChance = 0.13f,
                INTReason = "You fell to your death",
            });

            // Cronos intro ship crash
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Cat004_050Landing",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.SEQ_Cat004_050Landing.SEQ_Landing.Landing_Cutscene.ANIMCUTSCENE_Cat004_Arrival_CrashPT2.SFXSeqAct_SetWeaponVisibility_1",
                OriginatorOutlink = "Out",
                RollDelay = 29.5f,
                DeathChance = 0.1f,
                SlomoSpeed = 0.37f,
                INTReason = "You died on impact",
                UnskippableAnimSeqs = new[]
                {
                    "TheWorld.PersistentLevel.Main_Sequence.SEQ_Cat004_050Landing.SEQ_Landing.Landing_Cutscene.ANIMCUTSCENE_Cat004_Arrival_CrashPT2.SeqAct_Interp_0"
                }
            });

            // Geth Dreadnaught bridge collapse at end
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Gth001_700Hangar",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.LevelEvents.BioSeqAct_SetGestureMode_0",
                OriginatorOutlink = "Done",
                RollDelay = 2.7f,
                DeathChance = 0.19f,
                SlomoSpeed = 0.3f,
                Ragdoll = true,
                INTReason = "You broke your spine",
            });

            // London: Shot by harby
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_End001_910RaceToConduit",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Hit_by_Beam.SeqAct_Gate_0",
                OriginatorOutlink = "Out",
                RollDelay = 0.01f,
                DeathChance = 0.33f,
                SlomoSpeed = 0.5f,
                Ragdoll = true,
                INTReason = "You were killed",
            });

            // Shepard flies into the citadel at the end after warp
            // This file is static as it contains a ton of custom edits
            //edits.Add(new RPAEdit()
            //{
            //    SourceFilename = "BioD_End002_100Opening",
            //    StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.SEQ_Level_Opening.ANIMCUTSCENE_End02_ConduitExit.SFXSeqAct_SetWeaponVisibility_0",
            //    OriginatorOutlink = "Out",
            //    RollDelay = 86f,
            //    DeathChance = 1.0f,
            //    SlomoSpeed = 0.5f,
            //    Ragdoll = false,
            //    KillPlayer = false, // We have special logic
            //    INTReason = "You died on impact",
            //});

            // Shepard falls through Citadel fishtank
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Cit001_125Fall",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Fall.ANIMCUTSCENE_Cit001_Fall.SFXSeqAct_SetWeaponVisibility_1",
                OriginatorOutlink = "Out",
                RollDelay = 38f,
                DeathChance = 0.3f,
                SlomoSpeed = 0.5f,
                Ragdoll = false,
                INTReason = "You died on impact",
                UnskippableAnimSeqs = new[]
                {
                    "TheWorld.PersistentLevel.Main_Sequence.Fall.ANIMCUTSCENE_Cit001_Fall.SeqAct_Interp_0"
                }
            });

            // Leviathan Despoina: Crash onto ship
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Lev004_000ShuttleLoad",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Teleport_to_Surface.BioSeqAct_BioToggleCinematicMode_1",
                OriginatorOutlink = "Out",
                RollDelay = 5.0f,
                DeathChance = 0.15f,
                SlomoSpeed = 1.0f,
                Ragdoll = false,
                INTReason = "You died on impact",
            });

            // Citadel Coup: Crash shuttle after Kai Lame
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Cat003_480CarCutscene",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Patrol_Car.ANIMCUTSCENE_Cat003_Phantom.SFXSeqAct_SetWeaponVisibility_1",
                OriginatorOutlink = "Out",
                RollDelay = 54.3f,
                DeathChance = 0.25f,
                SlomoSpeed = 0.2f,
                Ragdoll = true,
                INTReason = "You died on impact",
                UnskippableAnimSeqs = new[]
                {
                    "TheWorld.PersistentLevel.Main_Sequence.Patrol_Car.ANIMCUTSCENE_Cat003_Phantom.SeqAct_Interp_0" // Sadly, this is hard mode only, but we can't know that...
                }
            });

            // Tuchanka: Bridge explodes from reaper shooting it
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Kro002_850BossRoad",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.3_Sec_Cutscene.BioSeqAct_ToggleSave_0",
                OriginatorOutlink = "Out",
                RollDelay = 4.1f,
                SlomoSpeed = 0.25f,
                DeathChance = 0.17f,
                Ragdoll = true,
                INTReason = "You broke your spine",
            });

            // Utukku Grunt: Fall from container
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_KroGru_150Camp",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.LevelEvents.SFXSeqAct_SetMusicID_3",
                OriginatorOutlink = "Out",
                RollDelay = 7.5f,
                SlomoSpeed = 0.3f,
                DeathChance = 0.17f,
                Ragdoll = true,
                INTReason = "You broke your neck",
            });

            // Hard Mode
            // HARD MODE - PROEAR TRIAL
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_ProEar_140Invasion",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.Earth_Invasion_Cutscene.ANIMCUTSCENE_proear_trial_reapers.SeqEvent_SequenceActivated_0",
                OriginatorOutlink = "Out",
                RollDelay = 23.5f,
                DeathChance = 0.19f,
                INTReason = "You died in the explosion",
                HardModeOnly = true,
                UnskippableAnimSeqs = new[]
                {
                    "TheWorld.PersistentLevel.Main_Sequence.Earth_Invasion_Cutscene.ANIMCUTSCENE_proear_trial_reapers.SeqAct_Interp_0" // Sadly, this is hard mode only, but we can't know that...
                }
            });

            // Fight with clone, hanging on the ledge
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_Cit004_290FightScene",
                StartKismetHookupIFP = "TheWorld.PersistentLevel.Main_Sequence.ANIMCUTSCENE_Cit004_CloneEnd.SeqAct_ToggleHidden_0",
                OriginatorOutlink = "Out",
                RollDelay = 27.3f,
                DeathChance = 1.0f,
                SlomoSpeed = 0.35f,
                INTReason = "You fell to your death",
                Ragdoll = true,
                HardModeOnly = true,
                UnskippableAnimSeqs = new[]
                {
                    "TheWorld.PersistentLevel.Main_Sequence.ANIMCUTSCENE_Cit004_CloneEnd.SeqAct_Interp_0"
                }
            });

            // London: Shot down on landing - LOCALIZED!
            // LOC files are premodified to prevent skipping.
            edits.Add(new RPAEdit()
            {
                SourceFilename = "BioD_End001_110LoadInterior",
                IsLocalized = true,
                StartKismetHookupIFP = "end001_shuttle_down_v_D.Node_Data_Sequence.BioSeqEvt_ConvNode_11",
                OriginatorOutlink = "Started",
                RollDelay = 1.3f,
                DeathChance = 0.17f,
                HardModeOnly = true,
                INTReason = "You were shot down",
            });

            return edits;
        }
    }
}
