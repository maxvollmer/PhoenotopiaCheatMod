using PhoenotopiaCheatMod.Helpers;
using System;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class ItemSpawner
    {
        private static string itemFilter = "";
        private static Vector2 scrollPosition = Vector2.zero;
        private static float offset = 0f;

        private static readonly RowThingy rowThingy = new RowThingy();

        private static int LastSpawnFrameCount { get; set; } = 0;

        public static void Draw(Rect windowRect, Vector2 windowSize)
        {
            MainModMenu.Transparent = false;

            if (MenuStateDetector.IsInMainMenu())
            {
                rowThingy.StartRow();
                GUILayout.Label("You need to be in a game to use the Item Spawner.", UnityModManager.UI.h1);
                rowThingy.CloseRowIfOpen();
                return;
            }

            DrawOptions();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(windowSize.x), GUILayout.ExpandHeight(false));

            int numItemsPerRow = (((int)windowSize.x - 50) / 125) - 2;
            DrawItems(windowRect, "Tools", ItemGridLogic.ITEM_CLASS.TOOL, numItemsPerRow);
            DrawItems(windowRect, "Inventory", ItemGridLogic.ITEM_CLASS.ITEM, numItemsPerRow);
            DrawItems(windowRect, "Character", ItemGridLogic.ITEM_CLASS.STATUS, numItemsPerRow);

            GUILayout.EndScrollView();
        }

        private static void DrawOptions()
        {
            var windowTopRect = GUILayoutUtility.GetLastRect();

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            GUILayout.Label("Left click any item to spawn one. Right click to spawn stack.", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));

            GUILayout.Space(25);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter: ", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            itemFilter = GUILayout.TextField(itemFilter);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            var optionsRect = GUILayoutUtility.GetLastRect();
            offset = windowTopRect.height + optionsRect.height;
        }

        private static void DrawItems(Rect windowRect, string title, ItemGridLogic.ITEM_CLASS itemClass, int numItemsPerRow)
        {
            rowThingy.StartRow();
            GUILayout.Label(title, UnityModManager.UI.h1);
            rowThingy.CloseRowIfOpen();


            rowThingy.StartRow();

            int itemsDrawnCount = 0;
            for (int id = 0; id < DB.ITEM_DEFS.Length; id++)
            {
                if (DB.ITEM_DEFS[id].classification != itemClass)
                    continue;

                if (itemsDrawnCount != 0 && itemsDrawnCount % numItemsPerRow == 0)
                {
                    rowThingy.CloseRowIfOpen();
                    rowThingy.StartRow();
                    itemsDrawnCount = 0;
                }

                if (DrawItem(windowRect, id))
                {
                    GUILayout.Space(25);
                    itemsDrawnCount++;
                }
            }

            for (; itemsDrawnCount < numItemsPerRow; itemsDrawnCount++)
            {
                DrawEmptyItemSlot();
                GUILayout.Space(25);
            }

            rowThingy.CloseRowIfOpen();
        }

        private static bool DrawItem(Rect windowRect, int id)
        {
            var itemDef = DB.ITEM_DEFS[id];
            if (!IsValidItem(itemDef))
                return false;

            GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));

            GUILayout.Label(itemDef.item_name,
                UnityModManager.UI.h2,
                GUILayout.ExpandHeight(false),
                GUILayout.ExpandWidth(false),
                GUILayout.MaxWidth(100)
                );

            GUILayout.Space(5);

            var rect = GUILayoutUtility.GetRect(75, 75, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));

            SpriteHelper.DrawSprite(PT2.sprite_lib.all_item_sprites[itemDef.graphic_id], rect);

            if (rect.Contains(scrollPosition + new Vector2(
                Input.mousePosition.x - windowRect.x,
                Screen.height - Input.mousePosition.y - windowRect.y - offset)))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SpawnItem(id, 1);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    SpawnItem(id, Math.Max(1, CheatsEnforcer.GetItemHoldLimit(id)));
                }
            }

            GUILayout.EndVertical();

            return true;
        }

        private static void SpawnItem(int id, int amount)
        {
            if (LastSpawnFrameCount == Time.frameCount)
                return;
            LastSpawnFrameCount = Time.frameCount;

            PT2.sound_g.PlayGlobalCommonSfx(152, 1.5f);
            if (amount > 1)
            {
                PT2.sound_g.PlayGlobalCommonSfx(152, 1.5f, 1.1f);
            }

            PT2.item_gen.SpawnLoot(id, amount, PT2.gale_script.GetTransform().position, null, Vector2.up);
        }

        private static void DrawEmptyItemSlot()
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));

            GUILayout.Label("",
                UnityModManager.UI.h2,
                GUILayout.ExpandHeight(false),
                GUILayout.ExpandWidth(false),
                GUILayout.MaxWidth(100)
                );

            GUILayout.Space(5);

            GUILayout.EndVertical();
        }

        private static bool IsValidItem(ItemGridLogic.ItemOrToolDef itemDef)
        {
            return
                !string.IsNullOrEmpty(itemDef.item_name)
                && itemDef.item_name.Trim().Length > 0
                && !itemDef.item_name.ToLower().Contains("placeholder")
                && itemDef.item_name.ToLower().Contains(itemFilter);
        }
    }
}
