
using UnityEngine;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class RowThingy
    {
        private bool isRowOpen = false;

        public void StartRow()
        {
            CloseRowIfOpen();
            GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });
            isRowOpen = true;
        }

        public void CloseRowIfOpen()
        {
            if (isRowOpen)
            {
                GUILayout.EndHorizontal();
            }
            isRowOpen = false;
        }
    }
}
