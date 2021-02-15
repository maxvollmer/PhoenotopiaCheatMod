
using HarmonyLib;

namespace PhoenotopiaCheatMod.Patches
{
    public class SoundGeneratorPatch
    {
        public static bool SoundsBlocked { get; set; } = false;

        [HarmonyPatch(typeof(SoundGenerator), "PlayCommonSfx")]
        public class SoundGeneratorPlayCommonSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayGaleLoopingSfx")]
        public class SoundGeneratorPlayGaleLoopingSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayGlobalCommonSfx")]
        public class SoundGeneratorPlayGlobalCommonSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayGlobalUncommonSfx")]
        public class SoundGeneratorPlayGlobalUncommonSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayGlobalVocalSfx")]
        public class SoundGeneratorPlayGlobalVocalSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayMusic")]
        public class SoundGeneratorPlayMusicPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayTransitionMusic")]
        public class SoundGeneratorPlayTransitionMusicPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }

        [HarmonyPatch(typeof(SoundGenerator), "PlayUncommonSfx")]
        public class SoundGeneratorPlayUncommonSfxPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !SoundsBlocked;
            }
        }
    }
}
