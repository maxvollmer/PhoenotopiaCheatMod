using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace PhoenotopiaCheatMod.Patches
{
    public class FishingGamePatch
    {
        [HarmonyPatch(typeof(FishingGameLogic), "GameIsFinished")]
        public class FishingGameLogicGameIsFinishedPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (!MainEntry.Settings.AutoFish)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(FishingGameLogic), "QueryReelingStatus")]
        public class FishingGameLogicQueryReelingStatusPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (!MainEntry.Settings.AutoFish)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(FishLogic), "_STATE_Interested")]
        public class FishLogicSTATEInterestedPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(FishLogic __instance)
            {
                if (MainEntry.Settings.AutoFish)
                {
                    Traverse.Create(__instance).Field("_misc_counter").SetValue(70);
                    Traverse.Create(__instance).Field("_wait_time").SetValue(4f);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FishLogic), "_ScanForBait")]
        public class FishLogicScanForBaitPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(FishLogic __instance)
            {
                if (!MainEntry.Settings.AutoFish)
                    return true;

                if (Traverse.Create(__instance).Field("_doesnt_bite").GetValue<bool>())
                    return true;

                Collider2D collider2D = GL.E_SightBox(__instance.transform.position, new Vector2(20f, 20f), __instance.transform.localScale.x <= 0f ? DIRECTION.LEFT : DIRECTION.RIGHT, GL.mask_FISH_LURE);
                if (!collider2D)
                {
                    Vector3 backupLocalScale = __instance.transform.localScale;
                    Vector3 reverseLocalScale = __instance.transform.localScale;
                    reverseLocalScale.x = -reverseLocalScale.x;
                    __instance.transform.localScale = reverseLocalScale;
                    collider2D = GL.E_SightBox(__instance.transform.position, new Vector2(20f, 20f), __instance.transform.localScale.x <= 0f ? DIRECTION.LEFT : DIRECTION.RIGHT, GL.mask_FISH_LURE);
                    if (!collider2D)
                    {
                        __instance.transform.localScale = backupLocalScale;
                    }
                }
                if (collider2D)
                {
                    Traverse.Create(__instance).Field("_fish_lure_transform").SetValue(collider2D.transform);
                    GoToState(__instance, 5); // FishLogic.STATE.INTERESTED = 5
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(FishLogic), "_STATE_BitingLure")]
        public class FishLogicSTATEBitingLurePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(FishLogic __instance)
            {
                if (!MainEntry.Settings.AutoFish)
                    return true;

                if (!PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.LURE_IS_FREE))
                    return true;

                var fish_lure_transform = Traverse.Create(__instance).Field("_fish_lure_transform").GetValue<Transform>();
                var wait_time = Traverse.Create(__instance).Field("_wait_time").GetValue<float>();

                __instance.GetComponent<Rigidbody2D>().MovePosition(
                    __instance.transform.position
                    + Vector3.MoveTowards(__instance._mouth_transform.position, fish_lure_transform.position, wait_time)
                    - __instance._mouth_transform.position);

                if (Vector2.Distance(__instance._mouth_transform.position, fish_lure_transform.position) < 0.01f)
                {
                    GoToState(__instance, 7); // FishLogic.STATE.FIGHTING = 7
                }
                if (wait_time > 2f)
                {
                    // fish is stuck, teleport it
                    __instance.transform.position = fish_lure_transform.position;
                    GoToState(__instance, 7); // FishLogic.STATE.FIGHTING = 7
                }

                return false;
            }
        }

        private static void GoToState(FishLogic fish, int state)
        {
            var goToStateMethod = typeof(FishLogic).GetMethod("_GoToState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (goToStateMethod != null)
            {
                goToStateMethod.Invoke(fish, new object[] { state });
            }
        }
    }
}
