
using System.Collections.Generic;
using UnityEngine;

namespace PhoenotopiaCheatMod.Helpers
{
    public class SpriteHelper
    {
        private static readonly Dictionary<string, Sprite> prefabs = new Dictionary<string, Sprite>();

        public static Sprite GetPrefabSprite(string prefabname)
        {
            if (prefabs.TryGetValue(prefabname, out Sprite sprite))
                return sprite;

            if (!LevelBuildLogic.prefabs_in_memory.TryGetValue(prefabname, out GameObject prefab))
                prefab = (GameObject)Resources.Load("Prefabs/" + prefabname);

            var instance = Object.Instantiate(prefab);

            instance.GetComponentInChildren<Animator>()?.SetInteger(GL.anim_state, 10);
            instance.GetComponentInChildren<Animator>()?.Update(Time.deltaTime);

            sprite = instance.GetComponentInChildren<SpriteRenderer>().sprite;

            Object.DestroyImmediate(instance);

            prefabs.Add(prefabname, sprite);

            return sprite;
        }

        public static Sprite GetSprite(GameObject monster)
        {
            return monster?.GetComponentInChildren<SpriteRenderer>()?.sprite;
        }

        public static void DrawSprite(Sprite sprite, Rect rect)
        {
            if (sprite == null)
            {
                GUI.Box(rect, "");
                return;
            }

            var tex = sprite.texture;
            var textureRect = sprite.textureRect;

            GUI.DrawTextureWithTexCoords(
                rect,
                tex,
                new Rect(
                    textureRect.x / tex.width,
                    textureRect.y / tex.height,
                    textureRect.width / tex.width,
                    textureRect.height / tex.height));
        }
    }
}
