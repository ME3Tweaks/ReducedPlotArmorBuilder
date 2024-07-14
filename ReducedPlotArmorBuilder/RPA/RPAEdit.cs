using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReducedPlotArmorBuilder.RPA
{
    /// <summary>
    /// Describes an edit to game files
    /// </summary>
    public class RPAEdit
    {
        /// <summary>
        /// The source file to edit
        /// </summary>
        public string SourceFilename { get; set; }

        /// <summary>
        /// The entry to hook off
        /// </summary>
        public string StartKismetHookupIFP { get; set; }

        /// <summary>
        /// The chance of death when action is fired
        /// </summary>
        public float DeathChance { get; set; }

        /// <summary>
        /// The speed to set when the player is killed, used to make it fit a bit better in cutscenes
        /// </summary>
        public float SlomoSpeed { get; set; } = 1.0f;

        /// <summary>
        /// If the player should be ragdolled on kill trigger
        /// </summary>
        public bool Ragdoll { get; set; } = false;

        /// <summary>
        /// If the player should be killed. If this is false, when the player would have died, the remote event 'RPAPrimePlayerDied' is fired.
        /// </summary>
        public bool KillPlayer { get; set; } = true;

        /// <summary>
        /// Reason (INT) for failure
        /// </summary>
        public string INTReason { get; set; }

        /// <summary>
        /// If this package file is the LOC version
        /// </summary>
        public bool IsLocalized { get; set; }

        /// <summary>
        /// The outlink to use on the originator
        /// </summary>
        public string OriginatorOutlink { get; set; }

        /// <summary>
        /// Delay to use for the roll, don't set for zero
        /// </summary>
        public float RollDelay { get; set; }
        /// <summary>
        /// If this can only occur in hard mode
        /// </summary>
        public bool HardModeOnly { get; set; }
        public string ESNReason { get; set; }
        public string DEUReason { get; set; }
        public string RUSReason { get; set; }
        public string POLReason { get; set; }
        public string FRAReason { get; set; }
        public string ITAReason { get; set; }
        public string JPNReason { get; set; }
        public string[] UnskippableAnimSeqs { get; set; }
    }
}
