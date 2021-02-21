using HarmonyLib;
using PhoenotopiaCheatMod.CheatMenu;
using PhoenotopiaCheatMod.Helpers;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod
{
    public class MainEntry
    {
        private static Harmony harmonyInstance;
        public static UnityModManager.ModEntry Mod { get; private set; }
        public static ModSettings.ModSettings Settings { get; private set; } = null;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            harmonyInstance = new Harmony("de.maxvollmer.phoenotopia.mousesupport");
            modEntry.OnUpdate += OnUpdate;
            modEntry.OnLateUpdate += OnLateUpdate;
            modEntry.OnFixedUpdate += OnFixedUpdate;
            modEntry.OnGUI += OnGUI;
            modEntry.OnFixedGUI += OnFixedGUI;
            modEntry.OnToggle += OnToggle;
            modEntry.OnSaveGUI += OnSave;
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                modEntry.Logger.Log("Patching all the things");
                Settings = ModSettings.ModSettings.Load<ModSettings.ModSettings>(modEntry);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                modEntry.Logger.Log("Unpatching all the things");
                harmonyInstance.UnpatchAll("de.maxvollmer.phoenotopia.mousesupport");
                Settings.Save(modEntry);
                MainModMenu.Transparent = false;
                EnemySpawner.IsPlacingEnemy = false;
                MapLocationDisplayer.DestroyTexts();
            }

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Draw(modEntry);
        }

        private static void OnSave(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        private static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            if (MainModMenu.Visible && !UnityModManager.UI.Instance.Opened)
            {
                MainModMenu.Draw();
            }
            else
            {
                MainModMenu.Transparent = false;
                EnemySpawner.IsPlacingEnemy = false;
            }
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float time)
        {
            if (UnityModManager.UI.Instance.Opened)
            {
                MainModMenu.Visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainModMenu.Visible = false;
            }

            if (Settings.OpenModMenuKeyBinding.Down())
            {
                if (MainModMenu.Visible)
                {
                    MainModMenu.Visible = false;
                }
                else
                {
                    if (UnityModManager.UI.Instance.Opened)
                    {
                        UnityModManager.UI.Instance.ToggleWindow(false);
                    }

                    MainModMenu.Visible = true;
                }
            }

            if (MainModMenu.Visible)
            {
                EnemySpawner.Update();
            }
            else
            {
                EnemySpawner.IsPlacingEnemy = false;
            }

            CheatsEnforcer.EnforceCheats();
            MapLocationDisplayer.Update();
        }

        private static void OnLateUpdate(UnityModManager.ModEntry modEntry, float time)
        {
            CheatsEnforcer.EnforceCheats();
        }

        private static void OnFixedUpdate(UnityModManager.ModEntry modEntry, float time)
        {
            CheatsEnforcer.EnforceCheats();
            CheatsEnforcer.OnFixedUpdate();
        }
    }
}
