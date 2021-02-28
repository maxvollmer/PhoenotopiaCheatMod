using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.ModSettings
{
    public class ModSettings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Key Binding To Open Mod Menu (Default: Ctrl+F11)")]
        public KeyBinding OpenModMenuKeyBinding = new KeyBinding()
        {
            keyCode = KeyCode.F11,
            modifiers = 1
        };

        public bool ShowAllMaps { get; set; } = false;
        public bool ShowGameMaps { get; set; } = true;
        public bool ShowNonGameMaps { get; set; } = false;
        public bool ShowWorldMaps { get; set; } = false;
        public bool ShowSaveLocationMaps { get; set; } = false;
        public bool ShowEnemyLocationMaps { get; set; } = false;
        public bool ShowNPCLocationMaps { get; set; } = false;
        public bool ShowOnlyVisitedMaps { get; set; } = false;

        public HashSet<string> VisitedMaps { get; set; } = new HashSet<string>();

        public bool GodMode { get; set; } = false;
        public bool Fly { get; set; } = false;
        public bool Noclip { get; set; } = false;

        public bool InfiniteMoney { get; set; } = false;
        public bool InfiniteStamina { get; set; } = false;
        public bool InfiniteStack { get; set; } = false;
        public bool ImproveSprint { get; set; } = false;
        public bool AutoRollAfterFall { get; set; } = false;
        public bool InstantCook { get; set; } = false;
        public bool AutoFlute { get; set; } = false;
        public bool AutoFish { get; set; } = false;
        public bool AlwaysChargeAttack { get; set; } = false;

        public bool DisableWorldMapFoes { get; set; } = false;
        public bool AlwaysShowMapLocations { get; set; } = false;

        public void OnChange()
        {
            Save(this, MainEntry.Mod);
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
