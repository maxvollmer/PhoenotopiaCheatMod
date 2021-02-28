
using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PhoenotopiaCheatMod.PhoenotopiaCheatMod.Patches
{
    public class FlutePlayPatch
    {
        [HarmonyPatch(typeof(GaleLogicOne), "_STATE_OcarinaPlaying")]
        public class GaleLogicOneOcarinaPlayingPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(GaleLogicOne __instance)
            {
                if (!MainEntry.Settings.AutoFlute)
                    return true;

                return PlayFlute(__instance, Traverse.Create(__instance).Field("_anim").GetValue<Animator>());
            }
        }

        public static bool PlayFlute(GaleLogicOne gale, Animator anim)
        {
            if (PT2.save_file.gales_using_item_ID != PT2.save_file.tool_hud_ID)
                return true;

            if (!PT2.director.control.TOOL_HELD)
                return true;

            if (Traverse.Create(gale).Field("_is_sprinting").GetValue<bool>())
                return true;

            string desired_song = GetDesiredSong(gale);
            if (desired_song == null)
                return true;

            var javelinOrGunMovementMethod = typeof(GaleLogicOne).GetMethod("_JavelinOrGunMovement", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[0], null);
            javelinOrGunMovementMethod.Invoke(gale, new object[0]);

            bool isDebugPlayingFlute = Traverse.Create(gale).Field("DEBUG_PLAYING_FLUTE").GetValue<bool>();
            int anim_state = Animator.StringToHash("anim");

            PT2.juicer.J_ScaleSineWobble(PT2.gale_interacter.gale_sprite.transform, 0.3f, 0.05f);

            float pitch = 1f;
            if (PT2.director.control.SPRINT_HELD) pitch -= 0.1f;
            if (PT2.director.control.CROUCH_HELD) pitch += 0.1f;

            Traverse.Create(gale).Field("_is_sprinting").SetValue(true);

            char note = GetNextNote(gale, desired_song);
            int animindex = GetAnimIndexForNote(gale, note);

            if (isDebugPlayingFlute)
            {
                anim.SetInteger(anim_state, animindex + 10);
            }
            else
            {
                anim.SetInteger(anim_state, animindex);
            }
            PT2.item_gen.EmitMusicalNote(gale.transform.position, note, pitch: pitch, is_spheralis: !isDebugPlayingFlute);

            var playedOcarinaNoteMethod = typeof(GaleLogicOne).GetMethod("_PlayedOcarinaNote", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(char) }, null);
            playedOcarinaNoteMethod.Invoke(gale, new object[] { note });

            if (!isDebugPlayingFlute)
            {
                PT2.p_g.NEWTYPE_GraphicBurst(false, 42, gale.transform.position, Color.white, 1f, 0.0f).SetMotion_CustomTweens(Vector3.zero, 0.85f, 3f, 0.0f, 0.5f * Vector3.one, 1.5f * Vector3.one, 0.0f, Vector3.zero, 1);
                PT2.sound_g.PlayGlobalUncommonSfx("waker", 1f, src_index: 2);
            }

            return false;
        }

        private static int GetAnimIndexForNote(GaleLogicOne gale, char note)
        {
            switch (note)
            {
                case 'L':
                    return gale.transform.localScale.x > 0f ? 100 : 101;
                case 'R':
                    return gale.transform.localScale.x > 0f ? 101 : 100;
                case 'U':
                    return 102;
                case 'D':
                    return 103;
                case 'N':
                default:
                    return 104;
            }
        }

        private static char GetNextNote(GaleLogicOne gale, string song)
        {
            var curr_song = Traverse.Create(gale).Field("_curr_song").GetValue<string>();

            for (int i = 1; i < song.Length; i++)
            {
                if (curr_song.EndsWith(song.Substring(0, song.Length - i)))
                {
                    return song[song.Length - i];
                }
            }

            return song[0];
        }

        private static string GetDesiredSong(GaleLogicOne gale)
        {
            var colliders = Physics2D.OverlapPointAll(gale.transform.position, GL.mask_SongField);

            if (colliders != null && colliders.Length > 0)
            {
                var songs_library = Traverse.Create(gale).Field("_songs_library").GetValue<string[]>();

                foreach (var aiZone in colliders.Select(c => c.GetComponent<AIZone>()))
                {
                    var desired_song = Traverse.Create(aiZone).Field("_desired_song").GetValue<int>();
                    if (desired_song >= 0 && desired_song < songs_library.Length)
                    {
                        return songs_library[desired_song];
                    }

                    if (aiZone.free_form_song != null)
                    {
                        return aiZone.free_form_song;
                    }
                }
            }

            return null;
        }
    }
}
