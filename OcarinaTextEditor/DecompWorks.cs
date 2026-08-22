using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda64TextEditor
{
    public class ControlCodeInfo
    {
        public string Name;
        public string ArgFormat; // 'b' = 1 byte, 'h' = 2 bytes (big-endian)
        public Func<int, string>[] Formatters;

        public ControlCodeInfo(string name, string argFormat, params Func<int, string>[] formatters)
        {
            Name = name;
            ArgFormat = argFormat;
            Formatters = formatters;
        }
    };

    public class DecompWorks
    {
        public static Dictionary<byte, ControlCodeInfo> _controlCodes = new Dictionary<byte, ControlCodeInfo>
        {
            [0x01] = new ControlCodeInfo("NEWLINE", "", null),
            [0x02] = new ControlCodeInfo("END", "", null),
            [0x04] = new ControlCodeInfo("BOX_BREAK", "", null),
            [0x05] = new ControlCodeInfo("COLOR", "b", FormatColor),
            [0x06] = new ControlCodeInfo("SHIFT", "b", null),
            [0x07] = new ControlCodeInfo("TEXTID", "h", null),
            [0x08] = new ControlCodeInfo("QUICKTEXT_ENABLE", "", null),
            [0x09] = new ControlCodeInfo("QUICKTEXT_DISABLE", "", null),
            [0x0A] = new ControlCodeInfo("PERSISTENT", "", null),
            [0x0B] = new ControlCodeInfo("EVENT", "", null),
            [0x0C] = new ControlCodeInfo("BOX_BREAK_DELAYED", "b", null),
            [0x0D] = new ControlCodeInfo("AWAIT_BUTTON_PRESS", "", null),
            [0x0E] = new ControlCodeInfo("FADE", "b", null),
            [0x0F] = new ControlCodeInfo("NAME", "", null),
            [0x10] = new ControlCodeInfo("OCARINA", "", null),
            [0x11] = new ControlCodeInfo("FADE2", "h", null),
            [0x12] = new ControlCodeInfo("SFX", "h", FormatSfxId),
            [0x13] = new ControlCodeInfo("ITEM_ICON", "b", FormatItemId),
            [0x14] = new ControlCodeInfo("TEXT_SPEED", "b", null),
            [0x15] = new ControlCodeInfo("BACKGROUND", "bbb", FormatBgArg, FormatBgBits1, FormatBgBits2),
            [0x16] = new ControlCodeInfo("MARATHON_TIME", "", null),
            [0x17] = new ControlCodeInfo("RACE_TIME", "", null),
            [0x18] = new ControlCodeInfo("POINTS", "", null),
            [0x19] = new ControlCodeInfo("TOKENS", "", null),
            [0x1A] = new ControlCodeInfo("UNSKIPPABLE", "", null),
            [0x1B] = new ControlCodeInfo("TWO_CHOICE", "", null),
            [0x1C] = new ControlCodeInfo("THREE_CHOICE", "", null),
            [0x1D] = new ControlCodeInfo("FISH_INFO", "", null),
            [0x1E] = new ControlCodeInfo("HIGHSCORE", "b", FormatHighscore),
            [0x1F] = new ControlCodeInfo("TIME", "", null),
           
            [0x9F] = new ControlCodeInfo("[A]", "", null),
            [0xA0] = new ControlCodeInfo("[B]", "", null),
            [0xA1] = new ControlCodeInfo("[C]", "", null),
            [0xA2] = new ControlCodeInfo("[L]", "", null),
            [0xA3] = new ControlCodeInfo("[R]", "", null),
            [0xA4] = new ControlCodeInfo("[Z]", "", null),
            [0xA5] = new ControlCodeInfo("[C-Up]", "", null),
            [0xA6] = new ControlCodeInfo("[C-Down]", "", null),
            [0xA7] = new ControlCodeInfo("[C-Left]", "", null),
            [0xA8] = new ControlCodeInfo("[C-Right]", "", null),
            [0xA9] = new ControlCodeInfo("▼", "", null),
            [0xAA] = new ControlCodeInfo("[Control-Pad]", "", null),
            [0xAB] = new ControlCodeInfo("[D-Pad]", "", null),
        };

        private static string FormatHighscore(int c)
        {
            switch (c)
            {
                case 0: return "HS_HBA";
                case 1: return "HS_POE_POINTS";
                case 2: return "HS_FISHING";
                case 3: return "HS_HORSE_RACE";
                case 4: return "HS_MARATHON";
                case 5: return "HS_UNK_05";
                case 6: return "HS_DAMPE_RACE";
                default: return c.ToString();
            }
        }

        private static string FormatColor(int c)
        {
            switch (c)
            {
                case 0x40: return "DEFAULT";
                case 0x41: return "RED";
                case 0x42: return "ADJUSTABLE";
                case 0x43: return "BLUE";
                case 0x44: return "LIGHTBLUE";
                case 0x45: return "PURPLE";
                case 0x46: return "YELLOW";
                case 0x47: return "BLACK";
                default: return c.ToString();
            }
        }

        private static string FormatBgArg(int c)
        {
            switch (c)
            {
                case 0: return "X_LEFT";
                case 1: return "X_RIGHT";
                default: return c.ToString();
            }
        }

        private static string FormatBgBits1(int c)
        {
            int c1 = (c >> 4) & 0xF;
            int c2 = c & 0xF;

            string fgcol;
            switch (c1)
            {
                case 0: fgcol = "WHITE"; break;
                case 1: fgcol = "DARK_RED"; break;
                case 2: fgcol = "ORANGE"; break;
                case 3: fgcol = "WHITE_3"; break;
                case 4: fgcol = "WHITE_4"; break;
                case 5: fgcol = "WHITE_5"; break;
                case 6: fgcol = "WHITE_6"; break;
                case 7: fgcol = "WHITE_7"; break;
                default: return c1.ToString();
            }

            string bgcol;
            switch (c2)
            {
                case 0: bgcol = "BLACK"; break;
                case 1: bgcol = "GOLD"; break;
                case 2: bgcol = "BLACK_2"; break;
                case 3: bgcol = "BLACK_3"; break;
                default: return c2.ToString();
            }

            return $"{fgcol}, {bgcol}";
        }

        private static string FormatBgBits2(int c)
        {
            int c1 = (c >> 4) & 0xF;
            int c2 = c & 0xF;

            string yOffset;
            switch (c1)
            {
                case 0: yOffset = "1"; break;
                case 1: yOffset = "2"; break;
                default: return c1.ToString();
            }

            return $"{yOffset}, {c2}";
        }

        private static readonly Dictionary<int, string> ItemIds = new Dictionary<int, string>
        {
            [0x00] = "ITEM_DEKU_STICK",
            [0x01] = "ITEM_DEKU_NUT",
            [0x02] = "ITEM_BOMB",
            [0x03] = "ITEM_BOW",
            [0x04] = "ITEM_ARROW_FIRE",
            [0x05] = "ITEM_DINS_FIRE",
            [0x06] = "ITEM_SLINGSHOT",
            [0x07] = "ITEM_OCARINA_FAIRY",
            [0x08] = "ITEM_OCARINA_OF_TIME",
            [0x09] = "ITEM_BOMBCHU",
            [0x0A] = "ITEM_HOOKSHOT",
            [0x0B] = "ITEM_LONGSHOT",
            [0x0C] = "ITEM_ARROW_ICE",
            [0x0D] = "ITEM_FARORES_WIND",
            [0x0E] = "ITEM_BOOMERANG",
            [0x0F] = "ITEM_LENS_OF_TRUTH",
            [0x10] = "ITEM_MAGIC_BEAN",
            [0x11] = "ITEM_HAMMER",
            [0x12] = "ITEM_ARROW_LIGHT",
            [0x13] = "ITEM_NAYRUS_LOVE",
            [0x14] = "ITEM_BOTTLE_EMPTY",
            [0x15] = "ITEM_BOTTLE_POTION_RED",
            [0x16] = "ITEM_BOTTLE_POTION_GREEN",
            [0x17] = "ITEM_BOTTLE_POTION_BLUE",
            [0x18] = "ITEM_BOTTLE_FAIRY",
            [0x19] = "ITEM_BOTTLE_FISH",
            [0x1A] = "ITEM_BOTTLE_MILK_FULL",
            [0x1B] = "ITEM_BOTTLE_RUTOS_LETTER",
            [0x1C] = "ITEM_BOTTLE_BLUE_FIRE",
            [0x1D] = "ITEM_BOTTLE_BUG",
            [0x1E] = "ITEM_BOTTLE_BIG_POE",
            [0x1F] = "ITEM_BOTTLE_MILK_HALF",
            [0x20] = "ITEM_BOTTLE_POE",
            [0x21] = "ITEM_WEIRD_EGG",
            [0x22] = "ITEM_CHICKEN",
            [0x23] = "ITEM_ZELDAS_LETTER",
            [0x24] = "ITEM_MASK_KEATON",
            [0x25] = "ITEM_MASK_SKULL",
            [0x26] = "ITEM_MASK_SPOOKY",
            [0x27] = "ITEM_MASK_BUNNY_HOOD",
            [0x28] = "ITEM_MASK_GORON",
            [0x29] = "ITEM_MASK_ZORA",
            [0x2A] = "ITEM_MASK_GERUDO",
            [0x2B] = "ITEM_MASK_TRUTH",
            [0x2C] = "ITEM_SOLD_OUT",
            [0x2D] = "ITEM_POCKET_EGG",
            [0x2E] = "ITEM_POCKET_CUCCO",
            [0x2F] = "ITEM_COJIRO",
            [0x30] = "ITEM_ODD_MUSHROOM",
            [0x31] = "ITEM_ODD_POTION",
            [0x32] = "ITEM_POACHERS_SAW",
            [0x33] = "ITEM_BROKEN_GORONS_SWORD",
            [0x34] = "ITEM_PRESCRIPTION",
            [0x35] = "ITEM_EYEBALL_FROG",
            [0x36] = "ITEM_EYE_DROPS",
            [0x37] = "ITEM_CLAIM_CHECK",
            [0x38] = "ITEM_BOW_FIRE",
            [0x39] = "ITEM_BOW_ICE",
            [0x3A] = "ITEM_BOW_LIGHT",
            [0x3B] = "ITEM_SWORD_KOKIRI",
            [0x3C] = "ITEM_SWORD_MASTER",
            [0x3D] = "ITEM_SWORD_BIGGORON",
            [0x3E] = "ITEM_SHIELD_DEKU",
            [0x3F] = "ITEM_SHIELD_HYLIAN",
            [0x40] = "ITEM_SHIELD_MIRROR",
            [0x41] = "ITEM_TUNIC_KOKIRI",
            [0x42] = "ITEM_TUNIC_GORON",
            [0x43] = "ITEM_TUNIC_ZORA",
            [0x44] = "ITEM_BOOTS_KOKIRI",
            [0x45] = "ITEM_BOOTS_IRON",
            [0x46] = "ITEM_BOOTS_HOVER",
            [0x47] = "ITEM_BULLET_BAG_30",
            [0x48] = "ITEM_BULLET_BAG_40",
            [0x49] = "ITEM_BULLET_BAG_50",
            [0x4A] = "ITEM_QUIVER_30",
            [0x4B] = "ITEM_QUIVER_40",
            [0x4C] = "ITEM_QUIVER_50",
            [0x4D] = "ITEM_BOMB_BAG_20",
            [0x4E] = "ITEM_BOMB_BAG_30",
            [0x4F] = "ITEM_BOMB_BAG_40",
            [0x50] = "ITEM_STRENGTH_GORONS_BRACELET",
            [0x51] = "ITEM_STRENGTH_SILVER_GAUNTLETS",
            [0x52] = "ITEM_STRENGTH_GOLD_GAUNTLETS",
            [0x53] = "ITEM_SCALE_SILVER",
            [0x54] = "ITEM_SCALE_GOLDEN",
            [0x55] = "ITEM_GIANTS_KNIFE",
            [0x56] = "ITEM_ADULTS_WALLET",
            [0x57] = "ITEM_GIANTS_WALLET",
            [0x58] = "ITEM_DEKU_SEEDS",
            [0x59] = "ITEM_FISHING_POLE",
            [0x5A] = "ITEM_SONG_MINUET",
            [0x5B] = "ITEM_SONG_BOLERO",
            [0x5C] = "ITEM_SONG_SERENADE",
            [0x5D] = "ITEM_SONG_REQUIEM",
            [0x5E] = "ITEM_SONG_NOCTURNE",
            [0x5F] = "ITEM_SONG_PRELUDE",
            [0x60] = "ITEM_SONG_LULLABY",
            [0x61] = "ITEM_SONG_EPONA",
            [0x62] = "ITEM_SONG_SARIA",
            [0x63] = "ITEM_SONG_SUN",
            [0x64] = "ITEM_SONG_TIME",
            [0x65] = "ITEM_SONG_STORMS",
            [0x66] = "ITEM_MEDALLION_FOREST",
            [0x67] = "ITEM_MEDALLION_FIRE",
            [0x68] = "ITEM_MEDALLION_WATER",
            [0x69] = "ITEM_MEDALLION_SPIRIT",
            [0x6A] = "ITEM_MEDALLION_SHADOW",
            [0x6B] = "ITEM_MEDALLION_LIGHT",
            [0x6C] = "ITEM_KOKIRI_EMERALD",
            [0x6D] = "ITEM_GORON_RUBY",
            [0x6E] = "ITEM_ZORA_SAPPHIRE",
            [0x6F] = "ITEM_STONE_OF_AGONY",
            [0x70] = "ITEM_GERUDOS_CARD",
            [0x71] = "ITEM_SKULL_TOKEN",
            [0x72] = "ITEM_HEART_CONTAINER",
            [0x73] = "ITEM_HEART_PIECE",
            [0x74] = "ITEM_DUNGEON_BOSS_KEY",
            [0x75] = "ITEM_DUNGEON_COMPASS",
            [0x76] = "ITEM_DUNGEON_MAP",
            [0x77] = "ITEM_SMALL_KEY",
            [0x78] = "ITEM_MAGIC_JAR_SMALL",
            [0x79] = "ITEM_MAGIC_JAR_BIG",
            [0x7A] = "ITEM_HEART_PIECE_2",
            [0x7B] = "ITEM_INVALID_1",
            [0x7C] = "ITEM_INVALID_2",
            [0x7D] = "ITEM_INVALID_3",
            [0x7E] = "ITEM_INVALID_4",
            [0x7F] = "ITEM_INVALID_5",
            [0x80] = "ITEM_INVALID_6",
            [0x81] = "ITEM_INVALID_7",
            [0x82] = "ITEM_MILK",
            [0x83] = "ITEM_RECOVERY_HEART",
            [0x84] = "ITEM_RUPEE_GREEN",
            [0x85] = "ITEM_RUPEE_BLUE",
            [0x86] = "ITEM_RUPEE_RED",
            [0x87] = "ITEM_RUPEE_PURPLE",
            [0x88] = "ITEM_RUPEE_GOLD",
            [0x89] = "ITEM_INVALID_8",
            [0x8A] = "ITEM_DEKU_STICKS_5",
            [0x8B] = "ITEM_DEKU_STICKS_10",
            [0x8C] = "ITEM_DEKU_NUTS_5",
            [0x8D] = "ITEM_DEKU_NUTS_10",
            [0x8E] = "ITEM_BOMBS_5",
            [0x8F] = "ITEM_BOMBS_10",
            [0x90] = "ITEM_BOMBS_20",
            [0x91] = "ITEM_BOMBS_30",
            [0x92] = "ITEM_ARROWS_5",
            [0x93] = "ITEM_ARROWS_10",
            [0x94] = "ITEM_ARROWS_30",
            [0x95] = "ITEM_DEKU_SEEDS_30",
            [0x96] = "ITEM_BOMBCHUS_5",
            [0x97] = "ITEM_BOMBCHUS_20",
            [0x98] = "ITEM_DEKU_STICK_UPGRADE_20",
            [0x99] = "ITEM_DEKU_STICK_UPGRADE_30",
            [0x9A] = "ITEM_DEKU_NUT_UPGRADE_30",
            [0x9B] = "ITEM_DEKU_NUT_UPGRADE_40",
            [0xFC] = "ITEM_SWORD_CS",
            [0xFE] = "ITEM_NONE_FE",
            [0xFF] = "ITEM_NONE",
        };

        private static string FormatItemId(int id)
        {
            return ItemIds.TryGetValue(id, out string name) ? name : $"{id}";
        }
        private static string FormatSfxId(int id)
        {
            var entry = Dicts.SFXes.FirstOrDefault(x => x.Value == id);
            return entry.Key != null ? entry.Key : $"{id}";
        }

        public static string GetDecompControlCode(byte[] b)
        {
            byte code = b[0];

            if (!DecompWorks._controlCodes.TryGetValue(code, out ControlCodeInfo info))
                return $"UNKNOWN({code})";

            if (string.IsNullOrEmpty(info.ArgFormat))
                return $"{info.Name}";

            var args = new List<string>();
            int offset = 1;

            for (int i = 0; i < info.ArgFormat.Length; i++)
            {
                int value;

                if (info.ArgFormat[i] == 'b')
                {
                    value = b[offset];
                    offset += 1;
                }
                else if (info.ArgFormat[i] == 'h')
                {
                    value = (b[offset] << 8) | b[offset + 1];
                    offset += 2;
                }
                else
                {
                    return "UNKNOWN_TAG";
                }

                var formatter = info.Formatters?[i];
                args.Add(formatter != null ? formatter(value) : value.ToString());
            }

            return $"{info.Name}({string.Join(",", args)})";
        }

    }
}