﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zelda64TextEditor.Enums
{
    public enum OcarinaMsgColor
    {
        W = 0x40,
        R = 0x41,
        G = 0x42,
        B = 0x43,
        C = 0x44,
        M = 0x45,
        Y = 0x46,
        BLK = 0x47
    }

    public enum OcarinaControlCode
    {
        LINE_BREAK = 0x01,
        END = 0x02,
        NEW_BOX = 0x04,
        COLOR = 0x05,
        SHIFT = 0x06,
        JUMP = 0x07,
        DI = 0x08,
        DC = 0x09,
        PERSISTENT = 0x0A,
        EVENT = 0x0B,
        DELAY = 0x0C,
        AWAIT_BUTTON = 0x0D,
        FADE = 0x0E,
        PLAYER = 0x0F,
        OCARINA = 0x10,
        FADE2 = 0x11,
        SOUND = 0x12,
        ICON = 0x13,
        SPEED = 0x14,
        BACKGROUND = 0x15,
        MARATHON_TIME = 0x16,
        RACE_TIME = 0x17,
        POINTS = 0x18,
        GOLD_SKULLTULAS = 0x19,
        NS = 0x1A,
        TWO_CHOICES = 0x1B,
        THREE_CHOICES = 0x1C,
        FISH_WEIGHT = 0x1D,
        HIGH_SCORE = 0x1E,
        TIME = 0x1F,

        LCHEVRON = 0x3C,
        RCHEVRON = 0x3E,

        DASH = 0x7F,
        À = 0x80,
        Î = 0x81,
        Â = 0x82,
        Ä = 0x83,
        Ç = 0x84,
        È = 0x85,
        É = 0x86,
        Ê = 0x87,
        Ë = 0x88,
        Ï = 0x89,
        Ô = 0x8A,
        Ö = 0x8B,
        Ù = 0x8C,
        Û = 0x8D,
        Ü = 0x8E,
        ß = 0x8F,
        à = 0x90,
        á = 0x91,
        â = 0x92,
        ä = 0x93,
        ç = 0x94,
        è = 0x95,
        é = 0x96,
        ê = 0x97,
        ë = 0x98,
        ï = 0x99,
        ô = 0x9A,
        ö = 0x9B,
        ù = 0x9C,
        û = 0x9D,
        ü = 0x9E,

        A_BUTTON = 0x9F,
        B_BUTTON = 0xA0,
        C_BUTTON = 0xA1,
        L_BUTTON = 0xA2,
        R_BUTTON = 0xA3,
        Z_BUTTON = 0xA4,
        C_UP = 0xA5,
        C_DOWN = 0xA6,
        C_LEFT = 0xA7,
        C_RIGHT = 0xA8,
        TRIANGLE = 0xA9,
        CONTROL_STICK = 0xAA,
        D_PAD = 0xAB
    }

    public enum MajoraMsgColor
    {
        D = 0x00,
        R = 0x01,
        G = 0x02,
        B = 0x03,
        Y = 0x04,
        N = 0x05,
        P = 0x06,
        S = 0x07,
        O = 0x08,
    }

    public enum MajoraIcon
    {
        GREEN_RUPEE = 1,
        BLUE_RUPEE = 2,
        WHITE_RUPEE = 3,
        RED_RUPEE = 4,
        PURPLE_RUPEE = 5,
        ORANGE_RUPEE = 7,
        ADULT_WALLET = 8,
        GIANTS_WALLET = 9,
        RECOVERY_HEART = 10,
        PIECE_OF_HEART = 12,
        HEART_CONTAINER = 13,
        SMALL_MAGIC_JAR = 14,
        LARGE_MAGIC_JAR = 15,
        STRAY_FAIRY = 17,
        BOMB = 20,
        BOMB_2 = 21,
        BOMB_3 = 22,
        BOMB_4 = 23,
        BOMB_5 = 24,
        DEKU_STICK = 25,
        BOMBCHU = 26,
        BOMB_BAG = 27,
        BIG_BOMB_BAG = 28,
        BIGGER_BOMB_BAG = 29,
        HEROS_BOW = 30,
        HEROS_BOW_1 = 31,
        HEROS_BOW_2 = 32,
        HEROS_BOW_3 = 33,
        QUIVER = 34,
        BIG_QUIVER = 35,
        BIGGEST_QUIVER = 36,
        FIRE_ARROW = 37,
        ICE_ARROW = 38,
        LIGHT_ARROW = 39,
        DEKU_NUT = 40,
        DEKU_NUT_1 = 41,
        DEKU_NUT_2 = 42,
        HEROS_SHIELD = 50,
        MIRROR_SHIELD = 51,
        POWDER_KEG = 52,
        MAGIC_BEAN = 53,
        KOKIRI_SWORD = 55,
        RAZOR_SWORD = 56,
        GILDED_SWORD = 57,
        FIERCE_DEITYS_SWORD = 58,
        GREAT_FAIRYS_SWORD = 59,
        SMALL_KEY = 60,
        BOSS_KEY = 61,
        DUNGEON_MAP = 62,
        COMPASS = 63,
        POWDER_KEG_2 = 64,
        HOOKSHOT = 65,
        LENS_OF_TRUTH = 66,
        PICTOGRAPH_BOX = 67,
        FISHING_ROD = 68,
        OCARINA_OF_TIME = 76,
        BOMBERS_NOTEBOOK = 80,
        GOLD_SKULLTULA_TOKEN = 82,
        ODOLWAS_REMAINS = 85,
        GOHTS_REMAINS = 86,
        GYORGS_REMAINS = 87,
        TWINMOLDS_REMAINS = 88,
        RED_POTION = 89,
        EMPTY_BOTTLE = 90,
        RED_POTION_2 = 91,
        GREEN_POTION = 92,
        BLUE_POTION = 93,
        FAIRYS_SPIRIT = 94,
        DEKU_PRINCESS = 95,
        MILK = 96,
        MILK_HALF = 97,
        FISH = 98,
        BUG = 99,
        BLUE_FIRE = 100,
        POE = 101,
        BIG_POE = 102,
        SPRING_WATER = 103,
        HOT_SPRING_WATER = 104,
        ZORA_EGG = 105,
        GOLD_DUST = 106,
        MUSHROOM = 107,
        SEAHORSE = 110,
        CHATEAU_ROMANI = 111,
        HYLIAN_LOACH = 112,
        DEKU_MASK = 120,
        GORON_MASK = 121,
        ZORA_MASK = 122,
        FIERCE_DEITY_MASK = 123,
        MASK_OF_TRUTH = 124,
        KAFEIS_MASK = 125,
        ALL_NIGHT_MASK = 126,
        BUNNY_HOOD = 127,
        KEATON_MASK = 128,
        GARO_MASK = 129,
        ROMANI_MASK = 130,
        CIRCUS_LEADERS_MASK = 131,
        POSTMANS_HAT = 132,
        COUPLES_MASK = 133,
        GREAT_FAIRYS_MASK = 134,
        GIBDO_MASK = 135,
        DON_GEROS_MASK = 136,
        KAMAROS_MASK = 137,
        CAPTAINS_HAT = 138,
        STONE_MASK = 139,
        BREMEN_MASK = 140,
        BLAST_MASK = 141,
        MASK_OF_SCENTS = 142,
        GIANTS_MASK = 143,
        GOLD_DUST_2 = 147,
        HYLIAN_LOACH_2 = 148,
        SEAHORSE_2 = 149,
        MOONS_TEAR = 150,
        TOWN_TITLE_DEED = 151,
        SWAMP_TITLE_DEED = 152,
        MOUNTAIN_TITLE_DEED = 153,
        OCEAN_TITLE_DEED = 154,
        ROOM_KEY = 160,
        SPECIAL_DELIVERY_TO_MAMA = 161,
        LETTER_TO_KAFEI = 170,
        PENDANT_OF_MEMORIES = 171,
        TINGLES_MAP = 179,
        TINGLES_MAP_2 = 180,
        TINGLES_MAP_3 = 181,
        TINGLES_MAP_4 = 182,
        TINGLES_MAP_5 = 183,
        TINGLES_MAP_6 = 184,
        TINGLES_MAP_7 = 185,
        ANJU = 220,
        KAFEI = 221,
        CURIOSITY_SHOP_OWNER = 222,
        BOMB_SHOP_OWNERS_MOTHER = 223,
        ROMANI = 224,
        CREMIA = 225,
        MAYOR_DOTOUR = 226,
        MADAME_AROMA = 227,
        TOTO = 228,
        GORMAN = 229,
        POSTMAN = 230,
        ROSA_SISTERS = 231,
        TOILET_HAND = 232,
        GRANNY = 233,
        KAMARO = 234,
        GROG = 235,
        GORMAN_BROTHERS = 236,
        SHIRO = 237,
        GURUGURU = 238,
        BOMBERS = 239,
        EXCLAMATION_MARK = 240,
        NO_ICON = 254,

    }

    public enum MajoraControlCode
    {
        COLOR_DEFAULT = 0x00,
        COLOR_RED = 0x01,
        COLOR_GREEN = 0x02,
        COLOR_BLUE = 0x03,
        COLOR_YELLOW = 0x04,
        COLOR_NAVY = 0x05,
        COLOR_PINK = 0x06,
        COLOR_SILVER = 0x07,
        COLOR_ORANGE = 0x08,
        UNK_09 = 0x09,
        NULL_CHAR = 0x0A,
        SWAMP_CRUISE_HITS = 0x0B,
        STRAY_FAIRY_SCORE = 0x0C,
        GOLD_SKULLTULAS = 0x0D,
        FISH_WEIGHT = 0x0E,
        UNK_0F = 0xF,
        NEW_BOX = 0x10,
        LINE_BREAK = 0x11,
        NEW_BOX_CENTER = 0x12,
        RESET_CURSOR = 0x13,
        SHIFT = 0x14,
        NOSKIP = 0x15,
        PLAYER = 0x16,
        DI = 0x17,
        DC = 0x18,
        NOSKIP_SOUND = 0x19,
        PERSISTENT = 0x1A,
        DELAY_NEWBOX = 0x1B,
        FADE = 0x1C,
        DELAY_END = 0x1D,
        SOUND = 0x1E,
        DELAY = 0x1F,
        END = 0xBF,
        BACKGROUND = 0xC1,
        TWO_CHOICES = 0xC2,
        THREE_CHOICES = 0xC3,
        POSTMAN_RESULTS = 0xC4,
        TIMER = 0xC5,
        TIMER2 = 0xC6,
        MOON_CRASH_TIME = 0xC7,
        DEKU_RESULTS = 0xC8,
        TIMER3 = 0xC9,
        TIMER4 = 0xCA,
        SHOOTING_GALLERY_RESULT = 0xCB,
        BANK_PROMPT = 0xCC,
        RUPEES_ENTERED = 0xCD,
        RUPEES_IN_BANK = 0xCE,
        MOON_CRASH_TIME_REMAINS = 0xCF,
        BET_RUPEES_PROMPT = 0xD0,
        BOMBER_CODE_PROMPT = 0xD1,
        ITEM_PROMPT = 0xD2,
        UNK_D3 = 0xD3,
        SOARING_DESTINATION = 0xD4,
        LOTTERY_NUMBER_PROMPT = 0xD5,
        OCEANSIDE_HOUSE_ORDER = 0xD6,
        WOODFALL_FAIRIES_REMAIN = 0xD7,
        SNOWHEAD_FAIRIES_REMAIN = 0xD8,
        BAY_FAIRIES_REMAIN = 0xD9,
        IKANA_FAIRIES_REMAIN = 0xDA,
        SWAMP_CRUISE_RESULT = 0xDB,
        WINNING_LOTTERY_NUM = 0xDC,
        PLAYER_LOTTERY_NUM = 0xDD,
        ITEM_VALUE = 0xDE,
        BOMBER_CODE = 0xDF,
        END_CONVERSATION = 0xE0,
        OCEANSIDE_HOUSE_ORDER_1 = 0xE1,
        OCEANSIDE_HOUSE_ORDER_2 = 0xE2,
        OCEANSIDE_HOUSE_ORDER_3 = 0xE3,
        OCEANSIDE_HOUSE_ORDER_4 = 0xE4,
        OCEANSIDE_HOUSE_ORDER_5 = 0xE5,
        OCEANSIDE_HOUSE_ORDER_6 = 0xE6,
        MOON_CRASH_HOURS_REMAIN = 0xE7,
        UNTIL_MORNING = 0xE8,
        UNK_E9 = 0xE9,
        UNK_EA = 0xEA,
        UNK_EB = 0xEB,
        UNK_EC = 0xEC,
        UNK_ED = 0xED,
        UNK_EE = 0xEE,
        UNK_EF = 0xEF,
        TOTAL_IN_BANK = 0xF0,
        UNK_F1 = 0xF1,
        UNK_F2 = 0xF2,
        UNK_F3 = 0xF3,
        UNK_F4 = 0xF4,
        UNK_F5 = 0xF5,
        TOWN_SHOOTING_HIGHSCORE = 0xF6,
        UNK_F7 = 0xF7,
        UNK_F8 = 0xF8,
        EPONA_ARCHERY_HIGHSCORE = 0xF9,
        DEKU_HIGHSCORE_DAY1 = 0xFA,
        DEKU_HIGHSCORE_DAY2 = 0xFB,
        DEKU_HIGHSCORE_DAY3 = 0xFC,
        UNK_FD = 0xFD,
        UNK_FE = 0xFE,
        UNK_FF = 0xFF,

        LCHEVRON = 0x3C,
        RCHEVRON = 0x3E,

        DASH = 0x7F,
        À = 0x80,
        Á = 0x81,
        Â = 0x82,
        Ä = 0x83,
        Ç = 0x84,
        È = 0x85,
        É = 0x86,
        Ê = 0x87,
        Ë = 0x88,
        Ì = 0x89,
        Í = 0x8A,
        Î = 0x8B,
        Ï = 0x8C,
        Ñ = 0x8D,
        Ò = 0x8E,
        Ó = 0x8F,
        Ô = 0x90,
        Ö = 0x91,
        Ù = 0x92,
        Ú = 0x93,
        Û = 0x94,
        Ü = 0x95,
        ß = 0x96,
        à = 0x97,
        á = 0x98,
        â = 0x99,
        ä = 0x9A,
        ç = 0x9B,
        è = 0x9C,
        é = 0x9D,
        ê = 0x9E,
        ë = 0x9F,
        ì = 0xA0,
        í = 0xA1,
        î = 0xA2,
        ï = 0xA3,
        ñ = 0xA4,
        ò = 0xA5,
        ó = 0xA6,
        ô = 0xA7,
        ö = 0xA8,
        ù = 0xA9,
        ú = 0xAA,
        û = 0xAB,
        ü = 0xAC,
        ª = 0xAF,

        A_BUTTON = 0xB0,
        B_BUTTON = 0xB1,
        C_BUTTON = 0xB2,
        L_BUTTON = 0xB3,
        R_BUTTON = 0xB4,
        Z_BUTTON = 0xB5,
        C_UP = 0xB6,
        C_DOWN = 0xB7,
        C_LEFT = 0xB8,
        C_RIGHT = 0xB9,
        TRIANGLE = 0xBA,
        CONTROL_STICK = 0xBB,
        D_PAD = 0xBC
    }

    public enum OcarinaHighScore
    {
        ARCHERY = 0x00,
        POE_POINTS = 0x01,
        FISH_WEIGHT = 0x02,
        HORSE_RACE = 0x03,
        MARATHON = 0x04,
        HS_UNK = 0x05,
        DAMPE_RACE = 0x06
    }

    public enum OcarinaIcon
    {
        DEKU_STICK,
        DEKU_NUT,
        BOMBS,
        BOW,
        FIRE_ARROWS,
        DINS_FIRE,
        SLINGSHOT,
        FAIRY_OCARINA,
        OCARINA_OF_TIME,
        BOMBCHUS,
        HOOKSHOT,
        LONGSHOT,
        ICE_ARROWS,
        FARORES_WIND,
        BOOMERANG,
        LENS_OF_TRUTH,
        BEANS,
        MEGATON_HAMMER,
        LIGHT_ARROWS,
        NAYRUS_LOVE,
        EMPTY_BOTTLE,
        RED_POTION,
        GREEN_POTION,
        BLUE_POTION,
        FAIRY,
        FISH,
        MILK,
        RUTOS_LETTER,
        BLUE_FIRE,
        BOTTLE_BUG,
        BOTTLE_POE,
        HALF_MILK,
        BOTTLE_BIGPOE,
        WEIRD_EGG,
        CHICKEN,
        ZELDAS_LETTER,
        KEATON_MASK,
        SKULL_MASK,
        SPOOKY_MASK,
        BUNNY_HOOD,
        GORON_MASK,
        ZORA_MASK,
        GERUDO_MASK,
        MASK_OF_TRUTH,
        SOLD_OUT,
        POCKET_EGG,
        POCKET_CUCCO,
        COJIRO,
        ODD_MUSHROOM,
        ODD_POTION,
        POACHERS_SAW,
        BROKEN_SWORD,
        PRESCRIPTION,
        EYEBALL_FROG,
        EYEDROPS,
        CLAIM_CHECK,
        BOW_FIRE,
        BOW_ICE,
        BOW_LIGHT,
        KOKIRI_SWORD,
        MASTER_SWORD,
        BIGGORON_SWORD,
        DEKU_SHIELD,
        HYLIAN_SHIELD,
        MIRROR_SHIELD,
        KOKIRI_TUNIC,
        GORON_TUNIC,
        ZORA_TUNIC,
        BOOTS,
        IRON_BOOTS,
        HOVER_BOOTS,
        BULLET_BAG,
        BIGGER_BULLET_BAG,
        BIGGEST_BULLET_BAG,
        QUIVER,
        BIG_QUIVER,
        BIGGEST_QUIVER,
        BOMB_BAG,
        BIGGER_BOMB_BAG,
        BIGGEST_BOMB_BAG,
        GORON_BRACELET,
        SILVER_GAUNTLETS,
        GOLDEN_GAUNTLETS,
        ZORA_SCALE,
        GOLDEN_SCALE,
        BROKEN_KNIFE,
        ADULTS_WALLET,
        GIANTS_WALLET,
        DEKU_SEEDS,
        FISHING_ROD,
        NOTHING_1,
        NOTHING_2,
        NOTHING_3,
        NOTHING_4,
        NOTHING_5,
        NOTHING_6,
        NOTHING_7,
        NOTHING_9,
        NOTHING_10,
        NOTHING_11,
        NOTHING_12,
        NOTHING_13,
        FOREST_MEDALLION,
        FIRE_MEDALLION,
        WATER_MEDALLION,
        SPIRIT_MEDALLION,
        SHADOW_MEDALLION,
        LIGHT_MEDALLION,
        KOKIRI_EMERALD,
        GORON_RUBY,
        ZORA_SAPPHIRE,
        STONE_OF_AGONY,
        GERUDO_PASS,
        GOLDEN_SKULLTULA,
        HEART_CONTAINER,
        HEART_PIECE,
        BOSS_KEY,
        COMPASS,
        DUNGEON_MAP,
        SMALL_KEY,
        MAGIC_JAR,
        BIG_MAGIC_JAR,
    }
}
