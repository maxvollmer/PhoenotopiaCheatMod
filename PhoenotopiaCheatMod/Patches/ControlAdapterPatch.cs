
using HarmonyLib;
using PhoenotopiaCheatMod.CheatMenu;

namespace PhoenotopiaCheatMod.Patches
{
    public class ControlAdapterPatch
    {
        [HarmonyPatch(typeof(ControlAdapter), "Update")]
        public class ControlAdapterUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !MainModMenu.Visible;
            }
        }
    }
}
