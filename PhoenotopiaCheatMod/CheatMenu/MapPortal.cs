using HarmonyLib;
using PhoenotopiaCheatMod.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class MapPortal
    {
        private class Map
        {
            public string name;
            public string file;
            public bool isGameMap;
            public bool isWorldMap;
            public bool hasSavePoint;
            public bool hasEnemies;
            public bool hasNPCs;
        }

        private static Vector2 scrollPosition = Vector2.zero;
        private static bool isLoadingMap = false;
        private static int numMaps = 0;

        private static List<Map> maps = new List<Map>();
        private static string mapNameFilter = "";

        public static void Draw(Rect windowRect, Vector2 windowSize)
        {
            MainModMenu.Transparent = false;

            DrawOptions();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(windowSize.x), GUILayout.ExpandHeight(false));

            DrawMaps();

            GUILayout.EndScrollView();
        }

        private static void DrawOptions()
        {
            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            GUILayout.Space(5);

            GUILayout.Label("Find and load any map from the game. Load custom maps located in the Levels folder.", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Label("Loading a map from the main menu will first load a game from slot 1 or create a new game in slot 1.", UnityModManager.UI.bold, GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            if (MenuStateDetector.IsInMainMenu() || string.IsNullOrEmpty(LevelBuildLogic.level_name))
            {
                GUILayout.Label("Current Map: -", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label("Current Map: " + LevelBuildLogic.level_name, UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            }

            GUILayout.Space(5);
            GUILayout.Label("Savegame 1 Map: " + ExtractMapNameFromSave(SaveFile.file_0_string_data), UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            GUILayout.Label("Savegame 2 Map: " + ExtractMapNameFromSave(SaveFile.file_1_string_data), UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            GUILayout.Label("Savegame 3 Map: " + ExtractMapNameFromSave(SaveFile.file_2_string_data), UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            GUILayout.Label("Savegame 4 Map: " + ExtractMapNameFromSave(SaveFile.file_3_string_data), UnityModManager.UI.bold, GUILayout.ExpandWidth(false));


            GUILayout.Space(15);

            GUILayout.Label("Filter:", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));

            GUILayout.Space(5);

            MainEntry.Settings.ShowAllMaps = ShowMapOption(MainEntry.Settings.ShowAllMaps, "Show all maps");

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            MainEntry.Settings.ShowGameMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowGameMaps, "Game maps");
            MainEntry.Settings.ShowNonGameMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowNonGameMaps, "Non-game maps");
            MainEntry.Settings.ShowWorldMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowWorldMaps, "World maps");
            MainEntry.Settings.ShowSaveLocationMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowSaveLocationMaps, "Save locations");
            MainEntry.Settings.ShowNPCLocationMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowNPCLocationMaps, "NPC locations");
            MainEntry.Settings.ShowEnemyLocationMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowEnemyLocationMaps, "Enemy locations");
            MainEntry.Settings.ShowOnlyVisitedMaps = ShowMapOption(!MainEntry.Settings.ShowAllMaps && MainEntry.Settings.ShowOnlyVisitedMaps, "Only maps you have visited");

            int count = new bool[] {
                MainEntry.Settings.ShowGameMaps,
                MainEntry.Settings.ShowNonGameMaps,
                MainEntry.Settings.ShowWorldMaps,
                MainEntry.Settings.ShowSaveLocationMaps,
                MainEntry.Settings.ShowNPCLocationMaps,
                MainEntry.Settings.ShowEnemyLocationMaps,
                MainEntry.Settings.ShowOnlyVisitedMaps
            }.Count(x => x);

            MainEntry.Settings.ShowAllMaps = (count == 0);

            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter Name: ", UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            mapNameFilter = GUILayout.TextField(mapNameFilter);
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.Label("Map Count: " + numMaps + " / " + maps.Count, UnityModManager.UI.bold);

            GUILayout.Space(5);

            GUILayout.EndVertical();
        }

        private static string ExtractMapNameFromSave(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "-";

            int index = data.IndexOf(',');
            if (index <= 0)
                return "-";

            return data.Substring(0, data.IndexOf(','));
        }

        private static bool ShowMapOption(bool value, string label)
        {
            value = GUILayout.Toggle(value, label, GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            return value;
        }

        private static void DrawMaps()
        {
            if (maps.Count == 0)
            {
                InitMaps();
            }

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            numMaps = 0;
            foreach (var map in maps)
            {
                if (ShouldShowMap(map))
                {
                    DrawMap(map);
                    numMaps++;
                }
            }

            GUILayout.EndVertical();
        }

        private static void InitMaps()
        {
            foreach (var file in Directory.GetFiles(Application.dataPath + "/StreamingAssets/Levels/", "*.xml", SearchOption.AllDirectories))
            {
                var mapname = Path.GetFileNameWithoutExtension(file);
                Map map = new Map
                {
                    name = mapname,
                    file = file,
                    isGameMap = mapname.StartsWith("p1_"),
                    isWorldMap = false,
                    hasSavePoint = false,
                    hasEnemies = false,
                    hasNPCs = false
                };

                using (var reader = XmlReader.Create(File.OpenRead(file)))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "objectgroup")
                        {
                            var name = reader.GetAttribute("name");
                            if (name == "map_objects")
                            {
                                map.isWorldMap = true;
                                break;
                            }
                            else if (name.StartsWith("objects"))
                            {
                                ParseLevelObjects(reader, ref map.hasSavePoint, ref map.hasEnemies, ref map.hasNPCs);
                            }
                        }
                    }
                }

                maps.Add(map);
            }
        }

        private static void ParseLevelObjects(XmlReader reader, ref bool hasSavePoint, ref bool hasEnemies, ref bool hasNPCs)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "object")
                {
                    string name = reader.GetAttribute("name");
                    if (name == "enemy")
                    {
                        hasEnemies = true;
                    }
                    if (name == "save_point")
                    {
                        hasSavePoint = true;
                    }
                    if (name == "npc")
                    {
                        hasNPCs = true;
                    }
                }
            }
        }

        private static void DrawMap(Map map)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            if (GUILayout.Button("Load", UnityModManager.UI.button, GUILayout.Width(100), GUILayout.ExpandWidth(false)))
            {
                LoadMap(map.name);
            }

            GUILayout.Space(10);

            GUILayout.Label(map.name, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();

            GUILayout.Space(25);
        }

        private static bool ShouldShowMap(Map map)
        {
            if (!string.IsNullOrEmpty(mapNameFilter) && !map.name.Contains(mapNameFilter))
                return false;

            if (MainEntry.Settings.ShowAllMaps)
                return true;

            if (MainEntry.Settings.ShowOnlyVisitedMaps && !MainEntry.Settings.VisitedMaps.Contains(map.name))
                return false;

            if (MainEntry.Settings.ShowGameMaps && !map.isGameMap)
                return false;

            if (MainEntry.Settings.ShowNonGameMaps && map.isGameMap)
                return false;

            if (MainEntry.Settings.ShowWorldMaps && !map.isWorldMap)
                return false;

            if (MainEntry.Settings.ShowSaveLocationMaps && !map.hasSavePoint)
                return false;

            if (MainEntry.Settings.ShowEnemyLocationMaps && !map.hasEnemies)
                return false;

            if (MainEntry.Settings.ShowNPCLocationMaps && !map.hasNPCs)
                return false;

            return true;
        }

        private static void LoadMap(string mapname)
        {
            if (isLoadingMap)
                return;

            isLoadingMap = true;
            MainModMenu.Visible = false;

            if (PT2.director.current_opening_menu != null)
            {
                var opening_menu = PT2.director.current_opening_menu;
                LoadMapFromMenu(opening_menu, mapname);
                PT2.director.current_opening_menu = null;
            }
            else
            {
                PT2.LoadLevel(mapname, 0, Vector3.zero);
                isLoadingMap = false;
            }
        }

        private static void LoadMapFromMenu(OpeningMenuLogic opening_menu, string mapname)
        {
            Traverse.Create(opening_menu).Field("StateFn").SetValue(new Action(() => { }));
            PT2.sound_g.AdjustMusicVolume(null, 0.0f, 0.5f, adjust_all_songs: true);
            PT2.tv_hud.FadeToBlack(() => { GameStartWithMap(opening_menu, mapname); }, 2f, false);
        }

        private static void GameStartWithMap(OpeningMenuLogic opening_menu, string mapname)
        {
            if (string.IsNullOrEmpty(SaveFile.file_0_string_data))
            {
                SaveFile.file_0_string_data = PT2.save_file._NS_CreateEmptySaveString(false);
            }

            var backupdata = SaveFile.file_0_string_data;
            string[] strArray = SaveFile.file_0_string_data.Split(',');
            strArray[0] = mapname;
            SaveFile.file_0_string_data = string.Join(",", strArray);

            SaveFile.save_file_index = 0;
            PT2.coming_from_opening_menu = true;
            opening_menu.GameStart();
            PT2.LoadLevel(mapname, 999999999, Vector3.zero);

            SaveFile.file_0_string_data = backupdata;

            isLoadingMap = false;
        }
    }
}
