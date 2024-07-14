using LegendaryExplorerCore;
using ReducedPlotArmorBuilder.RPA;

namespace ReducedPlotArmorBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reduced Plot Armor by ME3Tweaks");

            // Initialize Legendary Explorer Core
            LegendaryExplorerCoreLib.InitLib(TaskScheduler.Current, x => Console.WriteLine($"ERROR: {x}"));

            var inputDirectory = "Y:\\ModLibrary\\LE3\\Reduced Plot Armor\\Source";
            var outputDirectory = "Y:\\ModLibrary\\LE3\\Reduced Plot Armor\\DLC_MOD_ReducedPlotArmor\\CookedPCConsole";
            // var outputDirectory = "E:\\SteamLibrary\\steamapps\\common\\Mass Effect Legendary Edition\\Game\\ME3\\BioGame\\DLC\\DLC_MOD_ReducedPlotArmor\\CookedPCConsole";
            RPABuild.RunRPABuild(inputDirectory, outputDirectory);
        }
    }
}