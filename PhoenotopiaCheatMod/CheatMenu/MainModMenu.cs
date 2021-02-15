using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class MainModMenu
    {
        public static bool Visible { get; set; } = false;
        public static bool Transparent { get; set; } = false;

        private static Vector2 windowSize = Vector2.zero;
        private static Rect windowRect = Rect.zero;

        private static int tabId = 0;
        private static string[] tabs = { "Cheats", "Map Portal", "Item Spawner", "Enemy Spawner", "Audio Player" };

        public static void Draw()
        {
            windowSize = ClampWindowSize(new Vector2(1280f, 960f));
            windowRect = new Rect((Screen.width - windowSize.x) / 2f, (Screen.height - windowSize.y) / 2f, 0, 0);

            var backgroundColor = GUI.backgroundColor;
            var color = GUI.color;
            GUI.backgroundColor = Transparent ? new Color(1f, 1f, 1f, 0.2f) : Color.white;
            GUI.color = Color.white;
            windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "", UnityModManager.UI.window, GUILayout.Height(windowSize.y));
            GUI.backgroundColor = backgroundColor;
            GUI.color = color;
        }

        private static Vector2 ClampWindowSize(Vector2 orig)
        {
            return new Vector2(Mathf.Clamp(orig.x, Mathf.Min(1280, Screen.width), Screen.width), Mathf.Clamp(orig.y, Mathf.Min(960, Screen.height), Screen.height));
        }

        private static void DoMyWindow(int id)
        {
            GUILayout.BeginVertical(GUILayout.MinWidth(windowSize.x));

            GUILayout.Label("Phoenotopia Cheat Mod", UnityModManager.UI.h1);
            GUILayout.Label("WARNING: Using this mod will permanently disable achievements in your savegames!", UnityModManager.UI.bold);

            GUILayout.Space(5);
            tabId = GUILayout.Toolbar(tabId, tabs, UnityModManager.UI.button, GUILayout.ExpandWidth(false));
            GUILayout.Space(7);

            GUILayout.EndVertical();

            DrawTab(tabId);

            GUILayout.FlexibleSpace();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Close", UnityModManager.UI.button, GUILayout.ExpandWidth(false)))
            {
                Visible = false;
            }

            /*
            if (GUILayout.Button("Save", button, GUILayout.ExpandWidth(false)))
            {
                SaveSettingsAndParams();
            }
            */

            GUILayout.EndHorizontal();
        }

        private static void DrawTab(int tabId)
        {
            if (tabId != 3)
            {
                EnemySpawner.IsPlacingEnemy = false;
            }

            switch (tabId)
            {
                case 0: // Cheats
                    CheatsMenu.Draw(windowRect, windowSize);
                    break;
                case 1: // Map Portal
                    MapPortal.Draw(windowRect, windowSize);
                    break;
                case 2: // Item Spawner
                    ItemSpawner.Draw(windowRect, windowSize);
                    break;
                case 3: // Enemy Spawner
                    EnemySpawner.Draw(windowRect, windowSize);
                    break;
                case 4: // Audio Player
                    AudioPlayer.Draw(windowRect, windowSize);
                    break;
            }
        }
    }
}
