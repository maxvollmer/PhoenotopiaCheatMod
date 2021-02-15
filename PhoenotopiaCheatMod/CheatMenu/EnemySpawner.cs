using HarmonyLib;
using PhoenotopiaCheatMod.Helpers;
using PhoenotopiaCheatMod.Patches;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class EnemySpawner
    {
        private static Vector2 scrollPosition = Vector2.zero;

        private static Dictionary<string, GameObject> enemies = new Dictionary<string, GameObject>();

        private static string enemyFilter = "";

        private static HashSet<string> enemyTypesWithVariants = new HashSet<string>();
        private static string currentEnemyType = null;
        private static int currentEnemyCount = 0;

        private static readonly RowThingy rowThingy = new RowThingy();

        private static bool isPlacingEnemy = false;
        public static bool IsPlacingEnemy
        {
            get
            {
                return isPlacingEnemy;
            }
            set
            {
                isPlacingEnemy = value;
                if (!isPlacingEnemy)
                {
                    Object.Destroy(EnemyPlaceCursor);
                    EnemyPlaceCursor = null;
                    EnemyPlaceLocationSprites.ForEach(Object.Destroy);
                    EnemyPlaceLocationSprites.Clear();
                    EnemyPlaceLocations.Clear();
                    EnemyPlaceLocationCount = 0;
                    EnemyToPlace = null;
                    MainModMenu.Transparent = false;
                }
            }
        }

        private static string EnemyToPlace = null;
        private static GameObject EnemyPlaceCursor = null;
        private static int EnemyPlaceLocationCount = 0;
        private static List<Vector2> EnemyPlaceLocations = new List<Vector2>();
        private static List<GameObject> EnemyPlaceLocationSprites = new List<GameObject>();

        public static void Update()
        {
            if (IsPlacingEnemy)
            {
                PlaceEnemy();
                return;
            }
        }

        public static void Draw(Rect windowRect, Vector2 windowSize)
        {
            if (MenuStateDetector.IsInAnyMenu())
            {
                MainModMenu.Transparent = false;
                rowThingy.StartRow();
                GUILayout.Label("You need to be in a game with no menu open to use the Enemy Spawner.", UnityModManager.UI.h1);
                rowThingy.CloseRowIfOpen();
                return;
            }

            if (IsPlacingEnemy)
            {
                MainModMenu.Transparent = true;
                return;
            }
            MainModMenu.Transparent = false;

            DrawOptions();

            currentEnemyType = null;
            currentEnemyCount = 0;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(windowSize.x), GUILayout.ExpandHeight(false));

            var prefabs = Traverse.Create<LevelBuildLogic>().Field("PREFAB_NAME_MAP").GetValue<Dictionary<string, string>>();

            rowThingy.StartRow();

            int enemiesDrawnCount = 0;
            int numEnemiesPerRow = (((int)windowSize.x - 50) / 125) - 2;
            foreach (var enemy in Data.Data.Enemies.SelectMany(GetAllEnemySubtypes))
            {
                if (enemiesDrawnCount != 0 && enemiesDrawnCount % numEnemiesPerRow == 0)
                {
                    rowThingy.CloseRowIfOpen();
                    rowThingy.StartRow();
                    enemiesDrawnCount = 0;
                }

                if (!ShouldDrawEnemy(enemy, prefabs))
                    continue;

                GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                DrawEnemyTitle(enemy, prefabs);
                DrawEnemy(enemy);
                GUILayout.EndVertical();
                GUILayout.Space(25);

                enemiesDrawnCount++;
            }

            for (; enemiesDrawnCount < numEnemiesPerRow; enemiesDrawnCount++)
            {
                GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                DrawEmptyEnemySlot();
                GUILayout.EndVertical();
                GUILayout.Space(25);
            }

            rowThingy.CloseRowIfOpen();

            GUILayout.EndScrollView();
        }

        private static bool ShouldDrawEnemy(string enemy, Dictionary<string, string> prefabs)
        {
            if (string.IsNullOrEmpty(enemyFilter))
                return true;

            var enemyType = enemy.Split(';')[0].Split('=')[1];

            if (enemyType.ToLower().Contains(enemyFilter))
                return true;

            if (prefabs.TryGetValue(enemyType, out string prefabName))
            {
                if (prefabName.ToLower().Contains(enemyFilter))
                    return true;
            }

            return false;
        }

        private static void DrawOptions()
        {
            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            GUILayout.Label("Click on 'Spawn' to place an enemy in the current map." +
                " Enemies that follow routes (e.g. bees, slugs) need multiple clicks to set the route.",
                UnityModManager.UI.h1, GUILayout.ExpandWidth(false));

            GUILayout.Space(25);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter: ", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            enemyFilter = GUILayout.TextField(enemyFilter);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private static void PlaceEnemy()
        {
            if (Input.GetMouseButtonDown(1))
            {
                IsPlacingEnemy = false;
                return;
            }

            var position = MouseHelper.GetGameWorldCursorPos(out _);

            EnemyPlaceCursor.transform.position = new Vector3(position.x, position.y, 10);

            if (Input.GetMouseButtonDown(0))
            {
                EnemyPlaceLocations.Add(position);

                var locationGhost = CreateSpriteForEnemy(EnemyToPlace);
                locationGhost.transform.position = new Vector3(position.x, position.y, 10);

                foreach (var sprite in locationGhost.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, 0.2f);
                }

                EnemyPlaceLocationSprites.Add(locationGhost);
            }

            if (EnemyPlaceLocations.Count >= EnemyPlaceLocationCount)
            {
                FinishSpawnEnemy();
                IsPlacingEnemy = false;
            }
        }

        private static IEnumerable<string> GetAllEnemySubtypes(string enemy)
        {
            if (enemy.Contains("DIRECTION"))
            {
                yield return enemy.Replace("DIRECTION", "LEFT");
                yield return enemy.Replace("DIRECTION", "RIGHT");
                yield return enemy.Replace("DIRECTION", "UP");
                yield return enemy.Replace("DIRECTION", "DOWN");
            }
            else
            {
                yield return enemy;
            }
        }

        private static GameObject CreateSpriteForEnemy(string type)
        {
            SoundGeneratorPatch.SoundsBlocked = true;

            GameObject newparent = new GameObject();
            GameObject original = new GameObject();

            try
            {
                XmlReader reader = XmlReader.Create(new StringReader("<object type=\"" + type + ";name=KARTOFFELVURSTSALATLOL;ql=ALWAYS_TRUE\" x=\"0\" y=\"0\" width=\"16\" height=\"4080\"/>"));
                reader.Read();

                var handleEnemyMethod = typeof(LevelBuildLogic).GetMethod("_HandleEnemy", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(XmlReader) }, null);
                handleEnemyMethod.Invoke(PT2.level_builder, new object[] { reader });

                if (PT2.director.name_transform_map.TryGetValue("KARTOFFELVURSTSALATLOL", out Transform transform))
                {
                    original = transform.gameObject;

                    PT2.director.name_transform_map.Remove("KARTOFFELVURSTSALATLOL");

                    foreach (var component in transform.gameObject.GetComponentsInChildren<Component>())
                    {
                        component.GetType().GetMethod("Start", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new System.Type[0], null)?.Invoke(component, new object[0]);
                        // Simulate 10 frames or 0.5 seconds worth of frames, whichever is more,
                        // this should be enough to get all animators/sprites in the right state.
                        int framesToSimulate = (int)System.Math.Max(10, System.Math.Ceiling(0.5f / Time.deltaTime));
                        for (int i = 0; i < framesToSimulate; i++)
                        {
                            component.GetType().GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new System.Type[0], null)?.Invoke(component, new object[0]);
                            component.GetType().GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new System.Type[0], null)?.Invoke(component, new object[0]);
                        }
                    }

                    foreach (var animator in transform.gameObject.GetComponentsInChildren<Animator>())
                    {
                        animator.Update(Time.deltaTime);
                    }

                    newparent.transform.position = new Vector3(999999999, 999999999, 0);

                    foreach (var sprite in transform.gameObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.transform.SetParent(newparent.transform, false);
                    }

                    foreach (var hurtbox in newparent.GetComponentsInChildren<Hurtbox>())
                    {
                        Object.Destroy(hurtbox);
                    }

                    Object.Destroy(transform.gameObject);
                }
            }
            catch (System.Exception e)
            {
                MainEntry.Mod.Logger.LogException(e);
                Object.Destroy(newparent);
                Object.Destroy(original);
                newparent = new GameObject();
            }

            SoundGeneratorPatch.SoundsBlocked = false;

            return newparent;
        }

        private static void DrawEnemy(string type)
        {
            if (!enemies.ContainsKey(type))
            {
                enemies.Add(type, CreateSpriteForEnemy(type));
            }

            DrawEnemy(enemies[type]);

            GUILayout.Space(10);

            if (GUILayout.Button("Spawn", GUILayout.Width(100), GUILayout.ExpandWidth(false)))
            {
                BeginSpawnEnemy(type);
            }

            GUILayout.Space(25);
        }

        private static void DrawEmptyEnemySlot()
        {
            GUILayout.Label("", UnityModManager.UI.h1, GUILayout.Width(100), GUILayout.ExpandWidth(false));
        }

        private static void DrawEnemyTitle(string enemy, Dictionary<string, string> prefabs)
        {
            var enemyType = enemy.Split(';')[0].Split('=')[1];

            if (currentEnemyType != enemyType)
            {
                currentEnemyCount = 0;
            }

            if (enemyTypesWithVariants.Contains(enemyType))
            {
                currentEnemyCount++;
            }
            else
            {
                if (enemyType == currentEnemyType)
                {
                    enemyTypesWithVariants.Add(enemyType);
                }
            }

            currentEnemyType = enemyType;

            GUILayout.Label(enemyType, UnityModManager.UI.h1, GUILayout.Width(100), GUILayout.ExpandWidth(false));

            if (prefabs.TryGetValue(enemyType, out string prefabName))
            {
                GUILayout.Label("(" + prefabName + ")", UnityModManager.UI.h1, GUILayout.Width(100), GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label("", UnityModManager.UI.h1, GUILayout.Width(100), GUILayout.ExpandWidth(false));
            }

            if (currentEnemyCount > 0)
            {
                GUILayout.Label("Variant " + currentEnemyCount, GUILayout.Width(100), GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label("", GUILayout.Width(100), GUILayout.ExpandWidth(false));
            }

            GUILayout.Space(5);
        }

        private static void BeginSpawnEnemy(string type)
        {
            EnemyToPlace = type;
            IsPlacingEnemy = true;
            EnemyPlaceLocationCount = System.Math.Max(1, Regex.Matches(EnemyToPlace, Regex.Escape("tmx(0/0)")).Count);
            EnemyPlaceLocations.Clear();
            EnemyPlaceLocationSprites.Clear();
            Object.Destroy(EnemyPlaceCursor);
            EnemyPlaceCursor = CreateSpriteForEnemy(type);
        }

        private static void FinishSpawnEnemy()
        {
            var position = EnemyPlaceLocations[0];

            float x = position.x * 16;
            float y = -position.y * 16;

            var type = EnemyToPlace;//.Replace("tmx(0/0)", "tmx(" + x + "/" + y + ")");

            var regex = new Regex(Regex.Escape("tmx(0/0)"));
            foreach (var pos in EnemyPlaceLocations)
            {
                type = regex.Replace(type, "vec3(" + pos.x + "/" + pos.y + "/0)", 1);
            }

            XmlReader reader = XmlReader.Create(new StringReader("<object type=\"" + type + ";ql=ALWAYS_TRUE\" x=\"" + x + "\" y=\"" + y + "\" width=\"16\" height=\"4080\"/>"));
            reader.Read();

            var handleEnemyMethod = typeof(LevelBuildLogic).GetMethod("_HandleEnemy", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(XmlReader) }, null);
            handleEnemyMethod.Invoke(PT2.level_builder, new object[] { reader });
        }

        private static void DrawEnemy(GameObject enemy)
        {
            var rect = GUILayoutUtility.GetRect(75, 75, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
            SpriteHelper.DrawSprite(SpriteHelper.GetSprite(enemy), rect);
        }

        private static void DrawPrefab(string id, string prefabname)
        {
            GUILayout.Label(id + " / " + prefabname, UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            var rect = GUILayoutUtility.GetRect(75, 75, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
            SpriteHelper.DrawSprite(SpriteHelper.GetPrefabSprite(prefabname), rect);
            GUILayout.Space(15);
        }
    }

/*
<object id="49" name="enemy" type="type=p1_boar;instruction=baby" x="1136" y="368" width="32" height="16"/>
*/
}
