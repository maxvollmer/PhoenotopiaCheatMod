
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PhoenotopiaCheatMod.Helpers
{
    public class CheatsEnforcer
    {
        private static Vector3 NoclipVelocity { get; set; } = Vector3.zero;
        private static Vector3 NoclipPosition { get; set; } = Vector3.zero;
        private static string NoclipLastLevel = null;
        private static bool IsNoclipping = false;

        private static List<ItemGridLogic.ItemOrToolDef> itemsWithNormalStackSizeBackup = new List<ItemGridLogic.ItemOrToolDef>();

        public static void EnforceCheats()
        {
            PT2.save_file.MarkBool(PT2.save_file.TranslateSaveIndex("USING_CHEAT_MODS"), true);

            if (!MenuStateDetector.IsInMainMenu() && !string.IsNullOrEmpty(LevelBuildLogic.level_name))
            {
                MainEntry.Settings.VisitedMaps.Add(LevelBuildLogic.level_name);
            }

            var gale = PT2.gale_script.GetTransform().GetComponent<GaleLogicOne>();

            Traverse.Create(gale).Field("_DEBUG_IS_INVINCIBLE").SetValue(MainEntry.Settings.GodMode);

            if (MainEntry.Settings.InfiniteMoney)
            {
                MoneyHelper.SetMoney(999, true);
            }
            if (MainEntry.Settings.InfiniteStamina)
            {
                PT2.gale_interacter.stats.stamina = PT2.gale_interacter.stats.max_stamina;
            }
            if (MainEntry.Settings.AutoRollAfterFall)
            {
                PT2.director.control.num_frames_since_last_SPRINT_PRESSED = 0;
            }
            if (MainEntry.Settings.InfiniteStack)
            {
                if (itemsWithNormalStackSizeBackup.Count == 0)
                {
                    itemsWithNormalStackSizeBackup = DB.ITEM_DEFS.ToList();
                    for (int i = 0; i < DB.ITEM_DEFS.Length; i++)
                    {
                        DB.ITEM_DEFS[i].hold_limit = int.MaxValue;
                    }
                }
            }
            else
            {
                if (itemsWithNormalStackSizeBackup.Count > 0)
                {
                    for (int i = 0; i < DB.ITEM_DEFS.Length; i++)
                    {
                        DB.ITEM_DEFS[i].hold_limit = itemsWithNormalStackSizeBackup[i].hold_limit;
                    }
                    itemsWithNormalStackSizeBackup.Clear();

                    int[] itemIDs = Traverse.Create(PT2.save_file).Field("_item_IDs").GetValue<int[]>();
                    int[] itemCounts = Traverse.Create(PT2.save_file).Field("_item_ID_count").GetValue<int[]>();

                    for (int i = 0; i < itemIDs.Length && i < itemCounts.Length; i++)
                    {
                        itemCounts[i] = Math.Min(itemCounts[i], DB.ITEM_DEFS[itemIDs[i]].hold_limit);
                    }
                }
            }

            if (MainEntry.Settings.ImproveSprint)
            {
                if (PT2.director.control.SPRINT_HELD
                    && !PT2.director.control.SPRINT_PRESSED
                    && gale.QueryStatus(GALE_QUERY_STATUS.CAN_SHOW_INTERACT_SIGNS)
                    && !gale.QueryStatus(GALE_QUERY_STATUS.IN_MAP_MODE))
                {
                    PT2.director.control.SPRINT_PRESSED = true;
                }
            }

            var rigidBody = Traverse.Create(gale).Field("_rb").GetValue<Rigidbody2D>();
            rigidBody.isKinematic = !MainEntry.Settings.Noclip;

            if (CanFly(gale))
            {
                var velocity = Traverse.Create(gale).Field("velocity").GetValue<Vector3>();
                var run_velocity = Traverse.Create(gale).Field(PT2.director.control.SPRINT_HELD ? "max_vx_sprint" : "max_vx_run").GetValue<float>();
                var jump_velocity = Traverse.Create(gale).Field("jump_velocity").GetValue<float>();

                if (PT2.director.control.JUMP_HELD && PT2.director.control.UP_DOWN_AXIS > -0.5f)
                {
                    velocity.y = jump_velocity;
                }
                else if (PT2.director.control.CROUCH_HELD)
                {
                    velocity.y = -jump_velocity;
                }
                else if (PT2.director.control.UP_DOWN_AXIS == 1)
                {
                    velocity.y = run_velocity;
                }
                else if (PT2.director.control.UP_DOWN_AXIS == -1)
                {
                    velocity.y = -run_velocity;
                }
                else
                {
                    velocity.y = 0;
                }

                if (PT2.director.control.LEFT_RIGHT_AXIS == 1)
                {
                    velocity.x = run_velocity;
                }
                else if (PT2.director.control.LEFT_RIGHT_AXIS == -1)
                {
                    velocity.x = -run_velocity;
                }
                else
                {
                    velocity.x = 0;
                }

                if (!PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.ON_GROUND))
                {
                    if (velocity.x < 0)
                    {
                        gale.transform.localScale = GL.flipped_x;
                    }
                    else if (velocity.x > 0)
                    {
                        gale.transform.localScale = Vector3.one;
                    }
                }

                if (MainEntry.Settings.Noclip)
                {
                    if (!IsNoclipping || NoclipLastLevel != LevelBuildLogic.level_name)
                    {
                        NoclipPosition = gale.transform.position;
                        NoclipLastLevel = LevelBuildLogic.level_name;
                        IsNoclipping = true;
                    }
                    NoclipVelocity = velocity;
                }
                else
                {
                    IsNoclipping = false;
                }

                Traverse.Create(gale).Field("velocity").SetValue(velocity);
                Traverse.Create(gale).Field("_override_gale_vy").SetValue(velocity.y);
            }
            else
            {
                IsNoclipping = false;
            }

            if (IsNoclipping)
            {
                gale.transform.position = NoclipPosition;
            }
        }

        public static int GetItemHoldLimit(int id)
        {
            if (itemsWithNormalStackSizeBackup.Count > 0)
            {
                return itemsWithNormalStackSizeBackup[id].hold_limit;
            }
            else
            {
                return DB.ITEM_DEFS[id].hold_limit;
            }
        }

        private static bool CanFly(GaleLogicOne gale)
        {
            if (!MainEntry.Settings.Fly && !MainEntry.Settings.Noclip)
                return false;

            if (!MainEntry.Settings.Noclip &&
                (PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.ON_GROUND)
                 || PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IN_WATER)))
                return false;

            if (PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IS_SLEEPING)
                 || PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IS_TIED_UP)
                 || PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IS_COOKING)
                 || PT2.gale_script.QueryStatus(GALE_QUERY_STATUS.IS_EATING))
                return false;

            Data.Data.GALE_STATE state = (Data.Data.GALE_STATE)(int)Traverse.Create(gale).Field("_curr_state_enum").GetValue();
            switch (state)
            {
                case Data.Data.GALE_STATE.DEFAULT:
                case Data.Data.GALE_STATE.IN_AIR:
                case Data.Data.GALE_STATE.IN_AIR_CARRY:
                case Data.Data.GALE_STATE.ON_LADDER:
                    return true;
                case Data.Data.GALE_STATE.PARALYZED:
                case Data.Data.GALE_STATE.KNOCKED_OUT_ON_FLOOR:
                case Data.Data.GALE_STATE.KNOCKED_OUT_ON_FLOOR_2:
                case Data.Data.GALE_STATE.SLEEPING:
                case Data.Data.GALE_STATE.SLEEPING_LIGHT:
                case Data.Data.GALE_STATE.SLEEPING_LIGHT_STATUE:
                case Data.Data.GALE_STATE.TIED_UP:
                case Data.Data.GALE_STATE.DYING:
                case Data.Data.GALE_STATE.MAP_MODE:
                    return false;
                default:
                    return true; // or false? not sure
            }
        }

        public static void OnFixedUpdate()
        {
            if (MainEntry.Settings.Noclip && IsNoclipping)
            {
                NoclipPosition += NoclipVelocity * Time.deltaTime;
            }
        }
    }
}
