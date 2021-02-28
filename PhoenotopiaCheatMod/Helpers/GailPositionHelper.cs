using UnityEngine;

namespace PhoenotopiaCheatMod.Helpers
{
    public static class GailPositionHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by mods")]
        public static Transform GetGailTransform(this Object caller)
        {
            return PT2.gale_script.GetTransform();
        }
    }
}
