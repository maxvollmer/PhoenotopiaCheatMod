
using UnityEngine;

namespace PhoenotopiaCheatMod.Helpers
{
    public class MouseHelper
    {
        private static Vector3 cursorPos = default;
        private static bool isNewCursorPos = false;
        private static int lastCursorUpdateFrame = 0;

        private static Vector3 worldCursorPos = default;
        private static bool isNewWorldCursorPos = false;
        private static int lastWorldCursorUpdateFrame = 0;

        public static Vector2 GetCursorPos(out bool isNewCursorPos)
        {
            if (lastCursorUpdateFrame != Time.frameCount)
            {
                var screenCursorPos = Input.mousePosition;
                screenCursorPos.z = 10;
                var cursorPos = Camera.current.ScreenToWorldPoint(screenCursorPos);

                MouseHelper.isNewCursorPos = cursorPos != MouseHelper.cursorPos;
                MouseHelper.cursorPos = cursorPos;
                MouseHelper.lastCursorUpdateFrame = Time.frameCount;
            }

            isNewCursorPos = MouseHelper.isNewCursorPos;
            return MouseHelper.cursorPos;
        }

        public static Vector2 GetGameWorldCursorPos(out bool isNewCursorPos)
        {
            if (lastWorldCursorUpdateFrame != Time.frameCount)
            {
                var screenCursorPos = Input.mousePosition;
                screenCursorPos.z = 10;
                var worldCursorPos = Camera.main.ScreenToWorldPoint(screenCursorPos);

                MouseHelper.isNewWorldCursorPos = worldCursorPos != MouseHelper.worldCursorPos;
                MouseHelper.worldCursorPos = worldCursorPos;
                MouseHelper.lastWorldCursorUpdateFrame = Time.frameCount;
            }

            isNewCursorPos = MouseHelper.isNewWorldCursorPos;
            return MouseHelper.worldCursorPos;
        }
    }
}
