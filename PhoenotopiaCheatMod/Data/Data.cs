﻿
namespace PhoenotopiaCheatMod.Data
{
    public class Data
    {
        public enum GALE_STATE
        {
            BOUNCE_FREE_LIMITED,
            BOUNCE_REPELLED,
            LANDING_LAG,
            GRAB_WHIFF,
            HURT,
            DEFAULT,
            IN_AIR,
            HOVERING,
            HOVERING_LOOP,
            HOVERING_CARRY,
            HOVERING_CARRY_LOOP,
            FREEZE_B4_BOUNCE_FREE,
            GO_TO_DUCK,
            ROLLING,
            ON_LADDER,
            IN_AIR_CARRY,
            DUCK_STATE,
            THROWING,
            GRAB,
            LIFTING,
            TRANSITION_GRAB_TO_CARRY,
            CARRY_STATE_1,
            CARRY_STATE_2,
            IN_AIR_LEAPING_STATE,
            SET_LIFTABLE_DOWN_QUICK,
            SET_LIFTABLE_DOWN_pt1,
            SET_LIFTABLE_DOWN_pt2,
            SPRINT_STATE,
            SKID_STATE,
            SLINGSHOT_AIMING,
            SLINGSHOT_JUST_FIRED,
            CROSSBOW_AIMING,
            CROSSBOW_JUST_FIRED,
            OCARINA_GET,
            OCARINA_PLAYING,
            OCARINA_PUTAWAY,
            OCARINA_FINISHED,
            JAVELIN_PUTAWAY,
            JAVELIN_GET,
            JAVELIN_THROW,
            JAVELIN_GET_LEAP,
            FISH_ROD_CHARGE,
            FISH_ROD_CAST,
            FISH_ROD_REELING,
            CAUGHT_FISH,
            LAMP_CRANK,
            LAMP_CRANK_LEAP,
            LAMP_PUTAWAY,
            COOKING,
            COOKING_SUCCESS,
            COOKING_FAIL,
            SHOOT_KOBOLD_GUN,
            SHOOT_KOBOLD_GUN_LEAP,
            KOBOLD_GUN_PUTAWAY,
            EATING_STATE,
            DRINKING_STATE,
            EATING_FROM_BOX_STATE,
            ATK_HOLD_CHARGE,
            ATK_ONE,
            ATK_TWO,
            ATK_CHARGED,
            ATK_FAIR,
            ATK_LEAP_FAIR,
            ATK_HARPY_WHIRL,
            ATK_HEADBUTT,
            SLIP_ON_SLOPE,
            WATER_JUST_DIVED,
            WATER_FLOATING,
            WATER_DASH,
            REMOTE_CONTROLLED_NO_GRAV,
            EXIT_REMOTE_CONTROLS,
            MOVE_TO_SPEAK_WITH_NPC,
            REMOTE_CONTROLLED_FACING,
            KAIN_BACKUP,
            PARALYZED,
            TIED_UP,
            HEADBUTT_STUN,
            COMBOED_EATENED,
            UNCOMBOED_SPATOUT,
            KNOCKED_OUT_ON_FLOOR,
            KNOCKED_OUT_ON_FLOOR_2,
            SLEEPING,
            SLEEPING_LIGHT,
            SLEEPING_LIGHT_STATUE,
            DYING,
            MAP_MODE,
        }

        public static readonly string[] Enemies =
        {
            "type=fish;instruction=CLOWN;alerted_GIS=decorative",
            "type=fish;instruction=FISH_L",
            "type=fish;instruction=FISH_M",
            "type=fish;instruction=FISH_M;initial_behavior=STATIC",
            "type=fish;instruction=FISH_M;use_all_bright",
            "type=fish;instruction=FISH_S",
            "type=fish;instruction=FISH_S;alerted_GIS=decorative",
            "type=fish;instruction=FISH_S;initial_behavior=STATIC",
            "type=fish;instruction=TADPOLE;alerted_GIS=no_bites",
            "type=p1_anuri_slime;instruction=sleep",
            "type=p1_aviad",
            "type=p1_beam_drone",
            "type=p1_bee;instruction=tmx(0/0),tmx(0/0)",
            "type=p1_bee;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_bee;instruction=tmx(0/0);initial_behavior=angry",
            "type=p1_bit_bot;instruction=tmx(0/0)!tmx(0/0)!tmx(0/0)!tmx(0/0)",
            "type=p1_boar",
            "type=p1_boar;instruction=baby",
            "type=p1_boar;instruction=plant",
            "type=p1_boar;instruction=plant;initial_behavior=sniper",
            "type=p1_boar;instruction=plant;initial_behavior=special",
            "type=p1_borg",
            "type=p1_borg;instruction=sentry_alt",
            "type=p1_boss_kobold",
            //"type=p1_boss_toad;instruction=tmx(0/0),tmx(0/0);scale=1.5",
            "type=p1_bowknight;instruction=sniper",
            "type=p1_cave_bat",
            "type=p1_crawlie;instruction=BOMB;dir=DIRECTION",
            "type=p1_crawlie;instruction=BOMB;dir=DIRECTION;initial_behavior=narrow_vision",
            "type=p1_crawlie;instruction=FOOD;dir=DIRECTION",
            "type=p1_crawlie;instruction=SHOOT_DRONE;dir=DIRECTION",
            "type=p1_crawlie;instruction=SPIKE;dir=DIRECTION",
            "type=p1_drake;instruction=tmx(0/0),tmx(0/0)",
            "type=p1_drake;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_easy_turret;dir=DIRECTION",
            "type=p1_easy_turret;dir=DIRECTION;instruction=start_armed",
            "type=p1_enemy_cohort;name=baine",
            "type=p1_firefish",
            "type=p1_firefish;instruction=100",
            "type=p1_flying_mine",
            "type=p1_flying_mine;instruction=narrow_sight",
            "type=p1_flying_scrapper;instruction=puzzler",
            "type=p1_gale_shadow",
            "type=p1_great_mother",
            "type=p1_harpy",
            "type=p1_harpy;instruction=await_cmd",
            "type=p1_invisi_toad",
            "type=p1_jelly",
            "type=p1_jelly;instruction=135",
            "type=p1_kobold;instruction=tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_kobold;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0);initial_behavior=dungeon_ambush_logic",
            "type=p1_maneater",
            "type=p1_maneater;instruction=no_vines",
            "type=p1_maneater;instruction=no_vines;initial_behavior=exposed",
            "type=p1_maneater;instruction=tmx(0/0),tmx(0/0)",
            "type=p1_maneater;instruction=tmx(0/0),tmx(0/0);initial_behavior=exposed",
            "type=p1_matriarch",
            "type=p1_mech_crawler;instruction=SHOOT_DRONE;dir=DIRECTION",
            "type=p1_mech_crawler;instruction=SPIKE_DRONE;dir=DIRECTION",
            "type=p1_megalith",
            "type=p1_megalith;instruction=invincible,sight,2.5",
            "type=p1_megalith;instruction=invincible,sight,5",
            "type=p1_megalith;instruction=invincible,sight,5.1",
            "type=p1_megalith;instruction=sight,4.5",
            "type=p1_megalith;instruction=sight,5",
            "type=p1_megalith;instruction=sight,5,hurt_range,25",
            "type=p1_megalith;instruction=sight,5,invincible",
            "type=p1_megalith;instruction=sight,5.75",
            "type=p1_megalith;instruction=sight,6",
            "type=p1_nest;instruction=angle,0,number,0",
            "type=p1_nest;instruction=angle,225,pattern,SA,number,4,reserve,FAS,timer,10",
            "type=p1_nest;instruction=angle,270,pattern,FAS,number,3",
            "type=p1_nest;instruction=angle,270,pattern,FFF,number,1000,reserve,SSS,timer,6,boundary,14",
            "type=p1_nest;instruction=angle,270,pattern,SSS,number,1000,reserve,SSS,timer,10,boundary,14",
            "type=p1_nest;instruction=angle,45,pattern,SF,number,2,reserve,SA,timer,10",
            "type=p1_nest;instruction=angle,90,number,0",
            "type=p1_nest;instruction=angle,90,number,1,pattern,S",
            "type=p1_nest;instruction=number,0,angle,0",
            "type=p1_nest;instruction=number,0,angle,270;initial_behavior=invincible",
            "type=p1_ninja;instruction=sentry",
            "type=p1_paddle_bot;instruction=tmx(0/0)!tmx(0/0)!tmx(0/0)!tmx(0/0);all_bright",
            "type=p1_poison_slime;instruction=hang",
            "type=p1_poison_slimeboss",
            "type=p1_rock_toad",
            "type=p1_rollo_mine",
            "type=p1_saw_bot;instruction=tmx(0/0)!tmx(0/0)",
            "type=p1_saw_bot;instruction=tmx(0/0)!tmx(0/0)!tmx(0/0)!tmx(0/0)!tmx(0/0)!tmx(0/0)",
            "type=p1_slime",
            "type=p1_slime;instruction=ending_backup",
            "type=p1_slime;instruction=ending_backup2",
            "type=p1_slime;instruction=hang",
            "type=p1_slimeboss",
            "type=p1_slimeboss;instruction=sleep",
            "type=p1_slug;dir=DIRECTION;instruction=tmx(0/0),tmx(0/0)",
            "type=p1_slug;dir=DIRECTION;instruction=tmx(0/0),tmx(0/0);initial_behavior=2",
            "type=p1_spearknight",
            "type=p1_spearknight;instruction=sentry",
            "type=p1_spider;instruction=1;initial_behavior=punching_bag",
            "type=p1_spider;instruction=4.5",
            "type=p1_spider;instruction=7;initial_behavior=speed_bag",
            "type=p1_stealerbird",
            "type=p1_super_comp",
            "type=p1_toad",
            "type=p1_troll;instruction=can_lose_aggro",
            "type=p1_troll;instruction=can_lose_aggro,sentry",
            "type=p1_troll;instruction=can_lose_aggro;instruction=sentry_alt",
            "type=p1_troll;instruction=sentry",
            "type=p1_troll;instruction=sentry_alt",
            "type=p1_turret;dir=DIRECTION",
            "type=p1_turret;dir=DIRECTION;instruction=271,89",
            "type=p1_turret;dir=DIRECTION;instruction=155,205",
            "type=p1_turret;dir=DIRECTION;instruction=220,320",
            "type=p1_turret;dir=DIRECTION;instruction=270,360",
            "type=p1_turret;dir=DIRECTION;instruction=-30,45",
            "type=p1_turret;dir=DIRECTION;instruction=340,50",
            "type=p1_turret;dir=DIRECTION;instruction=90,180",
            "type=p1_turret;dir=DIRECTION;instruction=90,-20",
            "type=p1_viking",
            "type=p1_viking;instruction=sentry",
            "type=p1_wendigo;initial_behavior=slower;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_wendigo;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_wendigo;instruction=tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0),tmx(0/0)",
            "type=p1_wrecker;instruction=relentless,peaceful",
            "type=p1_wrecker;instruction=standby,lifebar1,relentless",
            "type=p1_wrecker;instruction=standby,lifebar1,relentless,dont_explode",
            "type=p1_zombot_broken",
            "type=p1_zombot_broken;instruction=sleep",
            "type=p1_zombot_white",
            "type=p1_zombot_white;instruction=ambush",
            "type=p1_zombot_white;instruction=sleep"
        };

        public static readonly string[] UncommonSounds =
        {
            "arboar_launch",
            "arboar_shoot_prep",
            "art_unlocked",
            "atmos1",
            "aviad_loop",
            "baby_boar_death",
            "baby_cry",
            "bad_ending_jingle",
            "bat_death",
            "bat_wake",
            "battery_lock",
            "battery_powered_up",
            "beak_cannon_charge",
            "beam_drone_charge",
            "beam_drone_hurt1",
            "beam_drone_hurt2",
            "beam_drone_loop",
            "beam_drone_move",
            "beam_drone_shot",
            "bee_charge",
            "bee_die",
            "bell_alarm",
            "big_explosion",
            "bird_chirp1",
            "bird_chirp2",
            "bird_chirp3",
            "bird_chirp4",
            "bird_die",
            "bird_fly",
            "bit_bot_bullet",
            "bit_bot_death",
            "bit_bot_hit",
            "bit_bot_rail",
            "bit_bot_reload",
            "bit_bot_shot",
            "boar_charge_hoof",
            "boar_death",
            "boar_hurt",
            "boar_prepare_charge",
            "boar_rebound",
            "boar_slide",
            "boar_snort",
            "bomb_bug_explode",
            "bomb_ipeas_rxn",
            "bone_break",
            "bone_crack",
            "borg_fall",
            "borg_hurt",
            "borg_idle1",
            "borg_idle2",
            "borg_idle3",
            "borg_idle4",
            "borg_jump",
            "borg_shot",
            "borg_step",
            "borg_sword1",
            "borg_sword2",
            "borg_to_fly",
            "borg_to_shoot",
            "borg_to_swing",
            "bowknight_aim",
            "bowknight_defeat",
            "bowknight_hurt",
            "bowknight_shot",
            "bowknight_shove",
            "bowknight_step",
            "bubble_switch",
            "bug_b4_leap",
            "bug_death",
            "bug_leap_atk",
            "bug_pain",
            "bug_pain2",
            "bug_queen_dizzy",
            "bug_queen_roar",
            "bug_queen_squeal1",
            "bug_queen_squeal2",
            "bug_queen_transform",
            "cartoon_run",
            "comp_destruct_sequence",
            "comp_drive",
            "comp_failure",
            "comp_force_field",
            "comp_gate_open",
            "comp_hyper_program",
            "comp_intro_speech1",
            "comp_life_detected",
            "comp_malfunction",
            "comp_shield_hit",
            "comp_weapons_launch",
            "company_sfx_logo",
            "computer_access",
            "computer_denied",
            "construction",
            "cow_moo",
            "crawler_snap",
            "crazy_spin",
            "crow_death",
            "crow_flap",
            "crow_hurt",
            "crow_idle1",
            "crow_swoop",
            "crystal_on",
            "crystal_owp",
            "cutscene_ship_abduct",
            "cutscene_ship_grunt",
            "cutscene_ship_warp_in",
            "cymbal",
            "dog_woof",
            "error_buzzer",
            "fire_fish_alert",
            "fire_fish_burst",
            "fire_fish_charge",
            "fire_fish_death",
            "fire_fish_huff",
            "fire_fish_hurt",
            "fire_fish_spit",
            "fire_fish_splash",
            "flag_touch",
            "flush",
            "flying_scrapper_open",
            "flying_scrapper_spin",
            "flysentry_fly",
            "frog_croak",
            "frog_die",
            "frontloader_printer_spawn",
            "gale_hmm",
            "gale_martial_learning",
            "gale_yawn",
            "ghoul_scream",
            "goofy_boss_intro",
            "great_mother_hurt",
            "great_mother_roar",
            "great_mother_roar2",
            "great_mother_servo",
            "hair_dyeing",
            "harpy_defeat",
            "harpy_feather",
            "harpy_feather2",
            "harpy_flap",
            "harpy_hurt",
            "harpy_hurt2",
            "harpy_laugh",
            "harpy_slash1",
            "harpy_slash2",
            "harpy_to_battle",
            "health_bar_entrance",
            "health_bar_refill",
            "heavy_jump",
            "heavy_stomp",
            "hint_bell",
            "hologram_static",
            "horror_reveal",
            "inn_sleep",
            "intro_menu_advance",
            "intro_menu_back",
            "intro_menu_start",
            "jail_door_open",
            "jail_door_relock",
            "jail_door_unlock",
            "jelly_death",
            "jelly_move",
            "kain_punch",
            "knife_krill",
            "kobold_grenade_armed",
            "lightning",
            "lightning2",
            "matri_defeat",
            "matri_hurt",
            "matri_kick",
            "matri_laugh",
            "matri_pogo",
            "matri_slide",
            "matri_throw",
            "mech_crawler_open",
            "megalith_beam",
            "megalith_death",
            "megalith_dmg",
            "megalith_hum",
            "megalith_prefire",
            "meow",
            "metal_break",
            "metal_break_s",
            "mother_missile",
            "mother_summon_borg",
            "mouse_click",
            "mouse_death",
            "mouse_squeak1",
            "mouse_squeak2",
            "music_box_a",
            "music_box_bb",
            "music_box_c",
            "music_box_e",
            "music_box_f",
            "ninja_defeat",
            "ninja_hurt",
            "ninja_slash",
            "ninja_step",
            "ninja_throw",
            "ninja_unsheathe",
            "p1_anuri_gate_opening",
            "p1_beep",
            "p1_blow_dart",
            "p1_boss_breath",
            "p1_boss_defeat",
            "p1_boss_drake_burrow",
            "p1_boss_drake_chomp",
            "p1_boss_drake_crash",
            "p1_boss_drake_growl",
            "p1_boss_drake_lava_breath",
            "p1_boss_drake_lava_launch",
            "p1_boss_drake_roll",
            "p1_boss_drake_tail_prepare",
            "p1_boss_drake_tail_whip",
            "p1_boss_drake_wakeup_roar",
            "p1_boss_gargle",
            "p1_boss_inhale",
            "p1_boss_jump",
            "p1_boss_pain",
            "p1_boss_petrify",
            "p1_boss_roar",
            "p1_boss_roof_break",
            "p1_boss_sniff",
            "p1_boss_snore",
            "p1_boss_spit",
            "p1_boss_stomp",
            "p1_boss_swallow",
            "p1_branch_crack",
            "p1_bridge_break",
            "p1_burrowing",
            "p1_cacti_bomb_inflate",
            "p1_cartoon_drop",
            "p1_chomp",
            "p1_crash_with_cat",
            "p1_crash_without_cat",
            "p1_deploy_metal",
            "p1_door_cloth_open",
            "p1_door_locked",
            "p1_door_locked_scifi",
            "p1_door_open",
            "p1_door_unlock",
            "p1_drake_death",
            "p1_drake_pain",
            "p1_drake_squeak",
            "p1_duri_crack",
            "p1_duri_impact",
            "p1_duri_smash",
            "p1_fall_rock",
            "p1_fat_swing",
            "p1_flying_mine_fail",
            "p1_flying_mine_open",
            "p1_flying_mine_spin",
            "p1_gut_explosion",
            "p1_katash_programming",
            "p1_kobold_boss_bad_food",
            "p1_kobold_boss_good_food",
            "p1_kobold_boss_hurt1",
            "p1_kobold_boss_hurt2",
            "p1_kobold_boss_laugh",
            "p1_kobold_boss_slash",
            "p1_kobold_boss_slash_prep",
            "p1_kobold_boss_sword_hit",
            "p1_kobold_boss_unsheathe",
            "p1_kobold_defeat",
            "p1_kobold_draw",
            "p1_kobold_howl",
            "p1_kobold_pain",
            "p1_kobold_retreat",
            "p1_kobold_ship_beep",
            "p1_kobold_ship_charge_laser",
            "p1_kobold_ship_deploy",
            "p1_kobold_ship_enter",
            "p1_kobold_ship_idle",
            "p1_kobold_ship_laser",
            "p1_kobold_shoot",
            "p1_kobold_warp1",
            "p1_kobold_warp2",
            "p1_laser_alarm_fireonce",
            "p1_laser_alarm_loop",
            "p1_laser_alarm_touched",
            "p1_maneater_ambush",
            "p1_maneater_chew",
            "p1_maneater_death",
            "p1_maneater_pain",
            "p1_metal_sweetener",
            "p1_nest_emerge",
            "p1_nest_inflate",
            "p1_perro_chatter1",
            "p1_perro_chatter2",
            "p1_perro_fly",
            "p1_perro_hurt",
            "p1_poison_creep",
            "p1_puki_angry",
            "p1_puki_bleat1",
            "p1_puki_bleat2",
            "p1_puki_bleat3",
            "p1_puki_bounce",
            "p1_puki_mad",
            "p1_rock_break",
            "p1_slime_attack",
            "p1_slime_drop_prepare",
            "p1_slime_pop",
            "p1_slime_prepare_atk",
            "p1_slime_rise",
            "p1_slotting_gem",
            "p1_slug",
            "p1_spider_death",
            "p1_spider_drop",
            "p1_spider_rewind",
            "p1_spit",
            "p1_star_hit",
            "p1_trap_arrow_reload",
            "P1_trap_arrow_reload",
            "p1_unburrow",
            "p1_warning_beep",
            "p1_zombot_gargle",
            "p1_zombot_getup",
            "p1_zombot_headturn",
            "p1_zombot_scream",
            "p1_zombot_shutoff",
            "p1_zombot_spit",
            "p1_zombot_walk",
            "page_flip",
            "parrot",
            "phalanx_beam_mill_charge",
            "phalanx_beam_mill_loop",
            "phalanx_beam_mill_shot",
            "phalanx_bullets_charge",
            "phalanx_dive",
            "phalanx_feather",
            "phalanx_grav",
            "phalanx_grav_consolidate",
            "phalanx_grav_generate",
            "phalanx_homing_shot",
            "phalanx_homing2_shot",
            "phalanx_jewel",
            "phalanx_light_flap",
            "phalanx_roar",
            "phalanx_roar4",
            "phalanx_screech",
            "phalanx_shield",
            "phalanx_shield_charge",
            "phalanx_shot_flap",
            "phalanx_threat_grip",
            "phalanx_v_shot",
            "phalanx_wing_flap",
            "phoenix_charge",
            "phoenix_emit",
            "phoenix_flight",
            "pipe_keeper_elevator_close",
            "pipe_keeper_elevator_open",
            "pipe_pneumo_gate_open",
            "pipe_pneumo_tube_launch",
            "pop_01",
            "queen_foot_stab",
            "question_mark",
            "raise_weapon",
            "recycler_display",
            "recycler_processing",
            "robo_dog",
            "robo_reconnect",
            "robo_rocket_fist_launch",
            "rollo_beep",
            "rollo_bounce",
            "rollo_last_alarm",
            "rope_constrict",
            "royal_alarm",
            "saw_blade",
            "science_door_open",
            "scorp_death",
            "scrapper_charge",
            "scrapper_shot",
            "security_cam_alert",
            "sewing",
            "sfx_logo_duration",
            "shadow_pop",
            "shock_panel_charge",
            "shock_panel_jolt",
            "shrapnel_bomb",
            "slide",
            "slime_boss_death",
            "slime_boss_grunt1",
            "slime_boss_grunt2",
            "slime_boss_grunt3",
            "slime_boss_move",
            "slime_boss_prepare_atk",
            "slime_boss_rise",
            "slime_boss_shoot",
            "slime_boss_shrink",
            "slime_boss_spike",
            "slime_boss_whip",
            "sonar_ping",
            "spawner_back_open",
            "spearknight_charge",
            "spearknight_charge2",
            "spearknight_defeat",
            "spearknight_grunt",
            "spearknight_hurt",
            "spearknight_slice",
            "spearknight_step",
            "spearknight_throw",
            "sphere_door",
            "spider_crawling",
            "spider_death",
            "stell_beam",
            "sub_dive",
            "sub_whistle",
            "target_hit",
            "target_hit_bells",
            "target_jingle",
            "teleport",
            "time_switch_crank",
            "train_slow",
            "train_whistle",
            "troll_alert",
            "troll_defeat",
            "troll_growl1",
            "troll_growl2",
            "troll_growl3",
            "troll_hurt",
            "troll_smash",
            "troll_step",
            "troll_throw",
            "tumbleweed_hit",
            "turret_off",
            "turret_on",
            "turret_shot",
            "turret_windup",
            "victory_fanfare",
            "viking_charge_step",
            "viking_defeat",
            "viking_hurt",
            "viking_run_slash",
            "viking_slash",
            "viking_step",
            "waker",
            "wave",
            "weapon_clang",
            "weight_switch_pressed",
            "weight_switch_pressed_forever",
            "weight_switch_unpressed",
            "wendigo_breath1",
            "wendigo_breath2",
            "wendigo_breath3",
            "wendigo_camping",
            "wendigo_defeat",
            "wendigo_growl1",
            "wendigo_growl2",
            "wendigo_growl3",
            "wendigo_growl4",
            "wendigo_slash_wall",
            "wendigo_step",
            "whistle",
            "whistling_a",
            "whistling_bb",
            "whistling_c",
            "whistling_e",
            "whistling_f",
            "whoosh_air",
            "wrecker_bomb_alarm",
            "wrecker_dash",
            "wrecker_emp_burst",
            "wrecker_fall",
            "wrecker_grab_throw",
            "wrecker_grenade_bounce",
            "wrecker_grenade_launch",
            "wrecker_grenade_off",
            "wrecker_grenade_ping",
            "wrecker_jump",
            "wrecker_laser_charge",
            "wrecker_laser_shot",
            "wrecker_loadshot",
            "wrecker_noise_alert",
            "wrecker_power_down",
            "wrecker_power_up",
            "wrecker_servo",
            "wrecker_walk"
        };

        public static readonly string[] Music =
        {
            "ambience_computers",
            "ambience_underwater",
            "astral_garden",
            "bad_ending",
            "bandits_lair",
            "barn_town",
            "boss_battle_classic",
            "comp_idle",
            "crowd_chatter",
            "cutscene_short_01",
            "daea",
            "day_dreams",
            "dojo",
            "dungeon",
            "dungeon_ambience",
            "eldwin",
            "evil_plan",
            "fellowship_relaxed",
            "fellowship_upbeat",
            "final_battle",
            "force_field",
            "forest_ambience",
            "fortuner",
            "frans_theme",
            "geo_club",
            "golem_forest",
            "great_tree",
            "humble_hamlet",
            "katash_theme",
            "lone_house",
            "lullaby",
            "map_scorch",
            "map_scorch_battle",
            "megalith_field",
            "mini_boss",
            "mini_boss2",
            "mini_game",
            "misty_gorge_nature",
            "mood_candles_burning",
            "mood_creepy",
            "mood_ocean",
            "mood_sewer",
            "mood_soldiers_battle",
            "mood_water_flow1",
            "mood_water_flow2",
            "mood_waterfall",
            "mood_waterfall_low",
            "mood_wind_desert",
            "mood_wind2",
            "moonlight_ravine",
            "mul_caves_main",
            "music_troupe",
            "mystic_cave",
            "nature_ambience",
            "night_ambience",
            "old_castle",
            "old_wood",
            "overworld_aether",
            "p1_anuri_temple_full",
            "p1_anuri_temple_start",
            "p1_atai_city",
            "p1_boss_battle",
            "p1_cave_ambience",
            "p1_duri_forest",
            "p1_intro_sequence",
            "p1_panselo",
            "p1_panselo_sad",
            "p1_scorch_lands",
            "P1_sunflower_road",
            "p1_world_map",
            "phalanx_battle",
            "phoenix_aura",
            "phoenix_lab",
            "piano_stone_angel",
            "pillars",
            "prelude",
            "pristine_city",
            "resolute",
            "reversed_golem",
            "royal_archive_battle",
            "royal_archive_slow",
            "sanctuary",
            "sanctuary_lab",
            "scientists",
            "scorch_lands",
            "secret_area",
            "shadow_battle",
            "shadow_fight",
            "shamshir_ruins",
            "silence",
            "song_for_the_dead",
            "spring_horror",
            "subterra",
            "temp_among_friends",
            "temp_aruni_village",
            "temp_terranigma_overworld",
            "temp_wild_arms_eru",
            "temp_wild_arms_town",
            "temp_wild_arms_young_lady",
            "the_borders",
            "the_new_fellowship",
            "towers",
            "train_tracks",
            "twin_towers",
            "twin_towers_intro",
            "wheat_path",
            "world_map_battle",
            "world_map_battle2"
        };
    }
}
