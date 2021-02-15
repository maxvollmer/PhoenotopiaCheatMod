
using HarmonyLib;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityModManagerNet;

namespace PhoenotopiaCheatMod.CheatMenu
{
    public class AudioPlayer
    {
        private static bool isRowOpen = false;
        private static string soundNameFilter = "";
        private static int volume = 100;
        private static int pitch = 100;
        private static Vector2 scrollPosition = Vector2.zero;

        private static bool ShowCommonSounds = true;
        private static bool ShowUncommonSounds = true;
        private static bool ShowMusic = true;

        private static Regex rgx = new Regex("[^0-9]");

        public static void Draw(Rect windowRect, Vector2 windowSize)
        {
            MainModMenu.Transparent = false;

            DrawOptions();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(windowSize.x), GUILayout.ExpandHeight(false));

            if (ShowCommonSounds)
                DrawCommonSounds(windowRect);
            if (ShowUncommonSounds)
                DrawUncommonSounds(windowRect);
            if (ShowMusic)
                DrawMusic(windowRect);

            GUILayout.EndScrollView();
        }

        private static void StartRow()
        {
            CloseRowIfOpen();
            GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });
            isRowOpen = true;
        }

        private static void CloseRowIfOpen()
        {
            if (isRowOpen)
            {
                GUILayout.EndHorizontal();
            }
            isRowOpen = false;
        }

        private static void DrawOptions()
        {
            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            GUILayout.Label("Playback any sound effect of the game.", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));

            GUILayout.Space(10);

            string current_song = GetCurrentSong();
            if (string.IsNullOrEmpty(current_song))
            {
                GUILayout.Label("Current Song Playing: -", UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label("Current Song Playing: " + current_song, UnityModManager.UI.bold, GUILayout.ExpandWidth(false));
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Stop All Sounds", GUILayout.Height(35)))
            {
                StopAllSounds();
            }

            GUILayout.Space(25);

            GUILayout.BeginHorizontal();
            ShowCommonSounds = GUILayout.Toggle(ShowCommonSounds, "Common Sounds", GUILayout.ExpandWidth(false));
            GUILayout.Space(15);
            ShowUncommonSounds = GUILayout.Toggle(ShowUncommonSounds, "Uncommon Sounds", GUILayout.ExpandWidth(false));
            GUILayout.Space(15);
            ShowMusic = GUILayout.Toggle(ShowMusic, "Music", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter: ", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            soundNameFilter = GUILayout.TextField(soundNameFilter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Volume: ", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            int.TryParse(rgx.Replace(GUILayout.TextField(volume.ToString()), ""), out volume);
            GUILayout.Space(25);
            GUILayout.Label("Pitch: ", UnityModManager.UI.h1, GUILayout.ExpandWidth(false));
            GUILayout.Space(25);
            int.TryParse(rgx.Replace(GUILayout.TextField(pitch.ToString()), ""), out pitch);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private static string GetCurrentSong()
        {
            string current_song = null;
            float loudestVolume = 0f;
            MusicPlayerLogic[] activeMusicPlayers = Traverse.Create(PT2.sound_g).Field("ACTIVE_MUSIC_PLAYERS").GetValue<MusicPlayerLogic[]>();
            foreach (var activeMusicPlayer in activeMusicPlayers)
            {
                if (activeMusicPlayer != null
                    && activeMusicPlayer.audio_src != null
                    && activeMusicPlayer.audio_src.isPlaying)
                {
                    if (activeMusicPlayer.audio_src.volume > loudestVolume)
                    {
                        current_song = activeMusicPlayer.music_name;
                        loudestVolume = activeMusicPlayer.audio_src.volume;
                    }
                }
            }
            return current_song;
        }

        private static void StopAllSounds()
        {
            PT2.sound_g.StopAllLoopingAudio();
            foreach (var audioSource in Traverse.Create(PT2.sound_g).Field("_global_audio_sources").GetValue<AudioSource[]>())
            {
                audioSource.Stop();
            }
            foreach (var audioSource in Traverse.Create(PT2.sound_g).Field("_audio_sources").GetValue<AudioSource[]>())
            {
                audioSource.Stop();
            }
            foreach (var musicPlayer in Traverse.Create(PT2.sound_g).Field("ACTIVE_MUSIC_PLAYERS").GetValue<MusicPlayerLogic[]>())
            {
                if (musicPlayer != null)
                {
                    musicPlayer.Bench();
                    musicPlayer.enabled = false;
                    musicPlayer.audio_src.Stop();
                }
            }
            foreach (var musicPlayer in Traverse.Create(PT2.sound_g).Field("BENCHED_MUSIC_PLAYERS").GetValue<MusicPlayerLogic[]>())
            {
                if (musicPlayer != null)
                {
                    musicPlayer.Bench();
                    musicPlayer.enabled = false;
                    musicPlayer.audio_src.Stop();
                }
            }
        }

        private static void DrawCommonSounds(Rect windowRect)
        {
            StartRow();
            GUILayout.Label("Common Sounds", UnityModManager.UI.h1);
            CloseRowIfOpen();

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            var common_sfx = Traverse.Create(PT2.sound_g).Field("_common_sfx").GetValue<AudioClip[]>();
            for (int id = 0; id < common_sfx.Length; id++)
            {
                if (common_sfx[id] == null)
                    continue;

                if (string.IsNullOrEmpty(soundNameFilter) || common_sfx[id].name.ToLower().Contains(soundNameFilter) || id.ToString().Contains(soundNameFilter))
                {
                    DrawCommonSound(id, common_sfx[id].name);
                }
            }

            GUILayout.EndVertical();
        }

        private static void DrawUncommonSounds(Rect windowRect)
        {
            StartRow();
            GUILayout.Label("Uncommon Sounds", UnityModManager.UI.h1);
            CloseRowIfOpen();

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            foreach (var sound in Data.Data.UncommonSounds)
            {
                if (string.IsNullOrEmpty(soundNameFilter) || sound.ToLower().Contains(soundNameFilter))
                {
                    DrawUncommonSound(sound);
                }
            }

            GUILayout.EndVertical();
        }

        private static void DrawMusic(Rect windowRect)
        {
            StartRow();
            GUILayout.Label("Music", UnityModManager.UI.h1);
            CloseRowIfOpen();

            GUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(25, 25, 25, 25) });

            foreach (var music in Data.Data.Music)
            {
                if (string.IsNullOrEmpty(soundNameFilter) || music.ToLower().Contains(soundNameFilter))
                {
                    DrawMusic(music);
                }
            }

            GUILayout.EndVertical();
        }

        private static void DrawCommonSound(int id, string name)
        {
            GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(0, 0, 15, 15) }, GUILayout.ExpandWidth(false));

            GUILayout.Label("Sound ID: ", UnityModManager.UI.h2, GUILayout.Height(35), GUILayout.Width(100));
            GUILayout.TextField(id.ToString(), GUILayout.Height(35), GUILayout.Width(100));
            GUILayout.Space(25);

            GUILayout.Label("Sound Name: ", UnityModManager.UI.h2, GUILayout.Height(35), GUILayout.Width(100));
            GUILayout.TextField(name, GUILayout.Height(35), GUILayout.Width(300));
            GUILayout.Space(25);

            if (GUILayout.Button("Play", GUILayout.Height(35), GUILayout.Width(200)))
            {
                PT2.sound_g.PlayGlobalCommonSfx(id, volume * 0.01f, pitch * 0.01f);
            }

            GUILayout.EndHorizontal();
        }

        private static void DrawUncommonSound(string name)
        {
            GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(0, 0, 15, 15) }, GUILayout.ExpandWidth(false));

            GUILayout.Label("Sound Name: ", UnityModManager.UI.h2, GUILayout.Height(35), GUILayout.Width(100));
            GUILayout.TextField(name, GUILayout.Height(35), GUILayout.Width(425));
            GUILayout.Space(25);

            if (GUILayout.Button("Play", GUILayout.Height(35), GUILayout.Width(200)))
            {
                PT2.sound_g.PlayGlobalUncommonSfx(name, volume * 0.01f, pitch * 0.01f);
            }

            GUILayout.EndHorizontal();
        }

        private static void DrawMusic(string name)
        {
            GUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(0, 0, 15, 15) }, GUILayout.ExpandWidth(false));

            GUILayout.Label("Music Name: ", UnityModManager.UI.h2, GUILayout.Height(35), GUILayout.Width(100));
            GUILayout.TextField(name, GUILayout.Height(35), GUILayout.Width(425));
            GUILayout.Space(25);

            if (GUILayout.Button("Play", GUILayout.Height(35), GUILayout.Width(200)))
            {
                var music_string = name + ",vol=" + (volume * 0.01f).ToString(CultureInfo.InvariantCulture);
                PT2.sound_g.PlayMusic(music_string);
            }

            GUILayout.EndHorizontal();
        }
    }
}
