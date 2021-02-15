
using HarmonyLib;
using PhoenotopiaCheatMod.Helpers;
using UnityEngine;

namespace PhoenotopiaCheatMod.Patches
{
    public class CheatsPatch
    {
        [HarmonyPatch(typeof(GaleLogicOne), "_GoToState")]
        public class GaleLogicOneGoToStatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref int new_state)
            {
                if (MainEntry.Settings.AlwaysChargeAttack && new_state == (int)Data.Data.GALE_STATE.ATK_ONE)
                {
                    new_state = (int)Data.Data.GALE_STATE.ATK_CHARGED;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(GaleLogicOne), "_InNonGravityState")]
        public class GaleLogicOneInNonGravityStatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (MainEntry.Settings.Fly && !PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.ON_GROUND))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(GaleLogicOne), "FixedUpdate")]
        public class GaleLogicOneFixedUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(GaleLogicOne __instance)
            {
                var rigidBody = Traverse.Create(__instance).Field("_rb").GetValue<Rigidbody2D>();
                rigidBody.isKinematic = false;

                if (MainEntry.Settings.Noclip)
                {
                    CheatsEnforcer.EnforceCheats();

                    var wasThrown = Traverse.Create(__instance).Field("_dmg_upon_landing").GetValue<int>() != 0;
                    var velocity = Traverse.Create(__instance).Field("velocity").GetValue<Vector3>();
                    var momentum = Traverse.Create(__instance).Field("momentum").GetValue<Vector3>();

                    var moveVelocity = new Vector3(velocity.x + momentum.x, momentum.y + velocity.y);

                    if (!wasThrown)
                    {
                        var max_vx_sprint = Traverse.Create(__instance).Field("max_vx_sprint").GetValue<float>();
                        var terminal_velocity_y = Traverse.Create(__instance).Field("terminal_velocity_y").GetValue<float>();
                        moveVelocity.x = Mathf.Clamp(moveVelocity.x, -max_vx_sprint, max_vx_sprint);
                        moveVelocity.y = Mathf.Clamp(moveVelocity.y, terminal_velocity_y, -terminal_velocity_y);
                    }

                    Data.Data.GALE_STATE state = (Data.Data.GALE_STATE)(int)Traverse.Create(__instance).Field("_curr_state_enum").GetValue();

                    if (state == Data.Data.GALE_STATE.MAP_MODE || __instance.QueryStatus(GALE_QUERY_STATUS.IN_MAP_MODE))
                    {
                        rigidBody.isKinematic = true;
                        rigidBody.position += (Vector2)moveVelocity * Time.deltaTime;
                    }
                    else
                    {
                        __instance.transform.position += moveVelocity * Time.deltaTime;
                    }

                    Traverse.Create(__instance).Field("velocity").SetValue(Vector3.zero);
                    Traverse.Create(__instance).Field("momentum").SetValue(Vector3.zero);
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ItemGenerator), "SummonWorldMapFoe")]
        public class ItemGeneratorSummonWorldMapFoePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !MainEntry.Settings.DisableWorldMapFoes;
            }
        }

        [HarmonyPatch(typeof(ItemGenerator), "GIS_SummonWorldMapFoe")]
        public class ItemGeneratorGISSummonWorldMapFoePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !MainEntry.Settings.DisableWorldMapFoes;
            }
        }

        [HarmonyPatch(typeof(CookingInterfaceLogic), "Update")]
        public class CookingInterfaceLogicUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CookingInterfaceLogic __instance)
            {
                if (!MainEntry.Settings.InstantCook
                    || !PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IS_COOKING))
                    return true;

                Traverse.Create(__instance).Method("_TransformItemAndSaveData", __instance._created_item_ID_on_success, true).GetValue();
                __instance._remaining_indicator_sprite.sprite = PT2.sprite_lib.hud_sprites[38];
                Traverse.Create(__instance).Method("_ExitOutOfCooking", true).GetValue();

                return false;
            }
        }

        [HarmonyPatch(typeof(CookingInterfaceLogic), "FixedUpdate")]
        public class CookingInterfaceLogicFixedUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                return !MainEntry.Settings.InstantCook;
            }
        }
    }
}
