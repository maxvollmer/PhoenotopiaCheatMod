using HarmonyLib;
using PhoenotopiaCheatMod.Helpers;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class CheatsMenu
    {
        private static Vector2 scrollPosition = Vector2.zero;

        public static void Draw(Rect windowRect, Vector2 windowSize)
        {
            if (MenuStateDetector.IsInMainMenu())
            {
                MainModMenu.Transparent = false;
                GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });
                GUILayout.Label("You need to be in a game to use cheats.", UnityModManager.UI.h1);
                GUILayout.EndVertical();
                //return;
            }

            MainModMenu.Transparent = false;

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });
            GUILayout.Label("Cheats for experiments, debugging, casual gaming, or skipping a puzzle.", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.EndVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(windowSize.x), GUILayout.ExpandHeight(false));

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            GUILayout.Label("Modify Game Values", UnityModManager.UI.h2, GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            int moneyvalue = PT2.save_file.GetInt((int)SaveFile.SAVE_ENUMS.MONEY);
            if (DrawInputSetting("Money", ref moneyvalue, 0))
            {
                MoneyHelper.SetMoney(moneyvalue);
            }

            GUILayout.Space(20);

            DrawStatsSettings();

            GUILayout.Space(20);

            DrawInputSetting("Inventory Size", ref PT2.save_file.SLOTS_MAX, 8, 64);

            GUILayout.Space(20);

            GUILayout.Label("Proper Cheats", UnityModManager.UI.h2, GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            MainEntry.Settings.GodMode = DrawToggleSetting("God Mode", MainEntry.Settings.GodMode);
            MainEntry.Settings.Fly = DrawToggleSetting("Fly", MainEntry.Settings.Fly);
            MainEntry.Settings.Noclip = DrawToggleSetting("Noclip*", MainEntry.Settings.Noclip);
            GUILayout.Label("*Warning: Noclipping into the ground or other inaccessible areas can crash the game.", GUILayout.ExpandWidth(false));

            GUILayout.Space(20);

            MainEntry.Settings.InfiniteMoney = DrawToggleSetting("Infinite Money", MainEntry.Settings.InfiniteMoney);
            MainEntry.Settings.InfiniteStamina = DrawToggleSetting("Infinite Stamina", MainEntry.Settings.InfiniteStamina);
            MainEntry.Settings.InfiniteStack = DrawToggleSetting("Infinite Inventory Stack Size", MainEntry.Settings.InfiniteStack);

            GUILayout.Space(20);

            MainEntry.Settings.AlwaysChargeAttack = DrawToggleSetting("Always Charge Attack", MainEntry.Settings.AlwaysChargeAttack);
            MainEntry.Settings.InstantCook = DrawToggleSetting("Instant Cook", MainEntry.Settings.InstantCook);
            MainEntry.Settings.AutoFish = DrawToggleSetting("Auto Fish", MainEntry.Settings.AutoFish);
            MainEntry.Settings.AutoFlute = DrawToggleSetting("Auto Play Correct Songs With Flute", MainEntry.Settings.AutoFlute);
            MainEntry.Settings.AutoRollAfterFall = DrawToggleSetting("Auto Roll After Fall", MainEntry.Settings.AutoRollAfterFall);
            MainEntry.Settings.ImproveSprint = DrawToggleSetting("Improve Sprint Behavior", MainEntry.Settings.ImproveSprint);

            GUILayout.Space(20);

            MainEntry.Settings.DisableWorldMapFoes = DrawToggleSetting("Disable World Map Foes", MainEntry.Settings.DisableWorldMapFoes);
            MainEntry.Settings.AlwaysShowMapLocations = DrawToggleSetting("Always Show Map Locations", MainEntry.Settings.AlwaysShowMapLocations);
            // MainEntry.Settings.DisableEnemySpawns = DrawToggleSetting("Disable enemy spawns*", MainEntry.Settings.DisableEnemySpawns);
            // GUILayout.Label("*Enemies that are necessary to progress in the story will still spawn. This includes most bosses.", GUILayout.ExpandWidth(false));

            GUILayout.Space(20);

            GUILayout.Label("Hidden Debug Settings", UnityModManager.UI.h2, GUILayout.ExpandWidth(false));
            GUILayout.Label("(Some of these are actually used by the game, e.g. through special inventory items.)", GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            var gale = PT2.gale_script.GetTransform().GetComponent<GaleLogicOne>();
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_ATTACK");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_HOVER");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_SWIM");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_COOK_BETTER");
            DrawToggleSettingForPrivateField(gale, "DEBUG_HAS_BLOOD_RING");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_HEADBUTT");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_SPEED_CHARGE");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_ENERGY_SPEAR");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_ULT_DEFEND");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_STORE_CHARGE");
            DrawToggleSettingForPrivateField(gale, "DEBUG_CAN_HARPY_WHIRL");
            DrawToggleSettingForPrivateField(gale, "DEBUG_PLAYING_FLUTE");
            DrawToggleSettingForPrivateField(gale, "DEBUG_IS_INVINCIBLE", "_DEBUG_IS_INVINCIBLE");

            GUILayout.Space(20);

            GUILayout.Label("＿＿＿＿＿＿＿＿＿＿＿＿＿", GUILayout.ExpandWidth(true));

            GUILayout.Space(10);

            GUILayout.Label("OPAF Buttons", UnityModManager.UI.h2, GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            if (GUILayout.Button("Kill All", GUILayout.ExpandWidth(false)))
            {
                KillAll(WhatToKill.ENEMIES);
            }
            GUILayout.Space(5);
            GUILayout.Label("Kills all enemies currently in this map.", GUILayout.ExpandWidth(false));

            GUILayout.Space(25);

            if (GUILayout.Button("Destroy All", GUILayout.ExpandWidth(false)))
            {
                KillAll(WhatToKill.THINGS);
            }
            GUILayout.Space(5);
            GUILayout.Label("Destroys all destructible objects currently in this map.", GUILayout.ExpandWidth(false));

            GUILayout.Space(25);

            if (GUILayout.Button("Give All", GUILayout.ExpandWidth(false)))
            {
                GiveAll();
            }
            GUILayout.Space(5);
            GUILayout.Label("Unlocks all tools and quest items, gives all abilities, gives maximum health and money, unlocks maximum inventory slots.", GUILayout.ExpandWidth(false));

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private static void DrawToggleSettingForPrivateField(GaleLogicOne gale, string fieldName, string actualFieldName = null)
        {
            var field = Traverse.Create(gale).Field(actualFieldName != null ? actualFieldName : fieldName);
            field.SetValue(DrawToggleSetting(fieldName, field.GetValue<bool>()));
        }

        private static void GiveAll()
        {
            for (int id = 0; id < DB.ITEM_DEFS.Length; id++)
            {
                if (DB.ITEM_DEFS[id].classification == ItemGridLogic.ITEM_CLASS.TOOL
                    || DB.ITEM_DEFS[id].classification == ItemGridLogic.ITEM_CLASS.STATUS)
                {
                    PT2.save_file.AddItemToolOrStatusIdToInventory(id, 1, true);
                }
            }
            PT2.thing_wheel.UpdateToolHudGraphics(false, false, true);
            PT2.thing_wheel.UpdateWheelGraphics();
            PT2.gale_script.EnableAbilitiesBasedOnInv(PT2.save_file.FetchData(MenuLogic.MENU_TYPE.P1_STATUS, true, null));

            PT2.gale_interacter.stats.hp = System.Math.Max(999, PT2.gale_interacter.stats.hp);
            PT2.gale_interacter.stats.max_hp = System.Math.Max(999, PT2.gale_interacter.stats.max_hp);
            PT2.hud_heart.J_UpdateHealth(PT2.gale_interacter.stats.hp, PT2.gale_interacter.stats.max_hp);

            MoneyHelper.SetMoney(999, true);

            PT2.save_file.SLOTS_MAX = 64;
        }

        private static Hurtbox.AttackResult CreateOverpoweredAttackResult(Hurtbox hurtbox)
        {
            return new Hurtbox.AttackResult
            {
                atk_status = Hitbox.ATK_STATUS.ATK_SUCCESS,
                atk_class = Hitbox.ATK_CLASS.PROJECTILE,
                atk_effect1 = Hitbox.ATK_EFFECT.IGNORE_DEF,
                atk_effect2 = Hitbox.ATK_EFFECT.SUPER_EFFECTIVE,
                side_effects = null,
                collided_layer_mask = -1,  // all bits
                final_damage_transferred = int.MaxValue,
                dmg_to_limb = int.MaxValue,
                knock_back = Vector2.zero,
                atk_immune_tag = null,
                damaged_party = hurtbox
            };
        }

        private static Hitbox.AttackStat CreateBoxDestroyerAttackStats()
        {
            return new Hitbox.AttackStat
            {
                atk_status = Hitbox.ATK_STATUS.ATK_SUCCESS,
                atk_class = Hitbox.ATK_CLASS.PROJECTILE,
                atk_effect1 = Hitbox.ATK_EFFECT.IGNORE_DEF,
                atk_effect2 = Hitbox.ATK_EFFECT.BOX_DESTROYER
            };
        }

        private enum WhatToKill
        {
            ENEMIES,
            THINGS
        }

        private static void KillAll(WhatToKill whatToKill)
        {
            if (whatToKill == WhatToKill.THINGS)
            {
                foreach (var box in Object.FindObjectsOfType(typeof(BoxLogic)).Select(b => b as BoxLogic).Where(b => b != null))
                {
                    if (box.is_indestructible)
                        continue;

                    box.hp = 0;
                    Traverse.Create(box).Field("_immunities").SetValue(null);
                    box.GetAttackResultAgainstLiftable(CreateBoxDestroyerAttackStats(), box.transform.position, null, false);
                }
            }

            foreach (var hurtbox in Object.FindObjectsOfType(typeof(Hurtbox)).Select(hb => hb as Hurtbox).Where(hb => hb != null))
            {
                MainEntry.Mod.Logger.Log("hurtbox: " + hurtbox);

                if (!hurtbox.gameObject.activeSelf || !hurtbox.gameObject.activeInHierarchy)
                    continue;

                MainEntry.Mod.Logger.Log("hurtbox.team_orientation: " + hurtbox.team_orientation);

                if (hurtbox.team_orientation == Hitbox.TEAM_ORIENTATION.FRIENDLY)
                    continue;

                if (whatToKill == WhatToKill.ENEMIES && hurtbox.team_orientation != Hitbox.TEAM_ORIENTATION.ENEMY)
                    continue;

                if (whatToKill == WhatToKill.THINGS && hurtbox.team_orientation != Hitbox.TEAM_ORIENTATION.NEUTRAL)
                    continue;

                var hurtable = Traverse.Create(hurtbox).Field("_hurtable").GetValue<Object>();
                MainEntry.Mod.Logger.Log("hurtable: " + hurtable);

                if (hurtable == null)
                    continue;

                MainEntry.Mod.Logger.Log("hurtable.name: " + hurtable.name);

                if (whatToKill == WhatToKill.ENEMIES && !hurtable.name.StartsWith("Enemy"))
                    continue;

                if (whatToKill == WhatToKill.THINGS && hurtable.name.StartsWith("Enemy"))
                    continue;

                var receiveAttackResultMethod = hurtable.GetType().GetMethod("ReceiveAttackResult");
                if (receiveAttackResultMethod != null)
                {
                    receiveAttackResultMethod.Invoke(hurtable, new object[] { CreateOverpoweredAttackResult(hurtbox) });
                }
            }
        }

        private static void DrawStatsSettings()
        {
            var hpChanged = DrawInputSetting("Health", ref PT2.gale_interacter.stats.hp, 1);
            var maxHPChanged = DrawInputSetting("Max Health", ref PT2.gale_interacter.stats.max_hp, 1);
            var staminaChanged = DrawInputSetting("Stamina", ref PT2.gale_interacter.stats.stamina, 0);
            var maxStaminaChanged = DrawInputSetting("Max Stamina", ref PT2.gale_interacter.stats.max_stamina, 4);
            var lLiftPowerChanged = DrawInputSetting("Lift Power", ref PT2.gale_interacter.stats.lift_power, 0);
            var staminaBuffChanged = DrawInputSetting("Stamina Buff", ref PT2.gale_interacter.stats.stamina_buff, 0);
            var attackBuffChanged = DrawInputSetting("Attack Buff", ref PT2.gale_interacter.stats.attack_buff, 0);

            if (hpChanged || maxHPChanged)
            {
                PT2.hud_heart.J_UpdateHealth(PT2.gale_interacter.stats.hp, PT2.gale_interacter.stats.max_hp);
            }
        }

        private static bool DrawInputSetting(string label, ref int value, int minValue, int maxValue = -1)
        {
            int oldvalue = value;
            if (oldvalue < minValue)
                minValue = oldvalue;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(150), GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            int.TryParse(GUILayout.TextField(value.ToString(NumberFormatInfo.InvariantInfo), GUILayout.Width(150), GUILayout.ExpandWidth(false)), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value);
            if (value < minValue)
                value = minValue;
            if (maxValue != -1 && value > maxValue)
                value = maxValue;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            return oldvalue != value;
        }

        private static bool DrawInputSetting(string label, ref float value, float minValue)
        {
            float oldvalue = value;
            if (oldvalue < minValue)
                minValue = oldvalue;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(150), GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            float.TryParse(GUILayout.TextField(value.ToString(NumberFormatInfo.InvariantInfo), GUILayout.Width(150), GUILayout.ExpandWidth(false)), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value);
            if (value < minValue)
                value = minValue;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            return oldvalue != value;
        }

        private static bool DrawToggleSetting(string label, bool value)
        {
            GUILayout.BeginHorizontal();
            value = GUILayout.Toggle(value, label, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            return value;
        }
    }
}
