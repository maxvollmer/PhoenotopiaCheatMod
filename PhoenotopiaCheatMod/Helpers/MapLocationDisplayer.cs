
using HarmonyLib;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PhoenotopiaCheatMod.Helpers
{
    public class MapLocationDisplayer
    {
        private static string CurrentMapName = null;
        private static List<GameObject> LocationTexts = new List<GameObject>();

        public static void Update()
        {
            if (!MainEntry.Settings.AlwaysShowMapLocations || string.IsNullOrEmpty(LevelBuildLogic.level_name))
            {
                DestroyTexts();
                return;
            }

            if (LevelBuildLogic.level_name != CurrentMapName)
            {
                DestroyTexts();
                foreach (var funnel in Object.FindObjectsOfType<FunnelStart>())
                {
                    AddLocationText(funnel);
                }
                CurrentMapName = LevelBuildLogic.level_name;
            }
        }

        public static void DestroyTexts()
        {
            LocationTexts.ForEach(Object.Destroy);
            LocationTexts.Clear();
            CurrentMapName = null;
        }

        private static void AddLocationText(FunnelStart funnel)
        {
            var profile = Traverse.Create(funnel).Field("_profile").GetValue<FunnelStart.DOOR_PROFILE>();

            if (profile != FunnelStart.DOOR_PROFILE.MAP_NODE)
                return;

            var summon_text_ptr = Traverse.Create(funnel).Field("_summon_text_ptr").GetValue<string>();

            var textObject = Object.Instantiate(PT2.map_name.gameObject);

            foreach (var component in textObject.GetComponentsInChildren<Component>())
            {
                if (component is TextMeshPro || component is Transform || component is SpriteRenderer)
                    continue;
                Object.Destroy(component);
            }

            var map_name_text = textObject.GetComponentInChildren<TextMeshPro>();
            var rect_transform = map_name_text.GetComponent<RectTransform>();
            var bg_window = textObject.GetComponentInChildren<SpriteRenderer>();

            textObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            textObject.transform.position = funnel.transform.position;
            map_name_text.text = DB.TRANSLATE_map[summon_text_ptr];
            float x = map_name_text.preferredWidth + 1f;
            if (x > rect_transform.sizeDelta.x)
                x = rect_transform.sizeDelta.x + 1f;
            bg_window.size = new Vector2(x, map_name_text.preferredHeight + 0.4f);
            bg_window.color = new Color(0f, 0f, 0f, 1f / 1.25f);
            map_name_text.alpha = 1f;

            LocationTexts.Add(textObject);
        }
    }
}
