using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda64TextEditor
{
    public enum ROMVer
    {
        Unknown,
        Debug,
        NTSC_0_9,
        NTSC_1_0,
        NTSC_1_1,
        NTSC_1_2,
        PAL1_0,
        PAL1_1,
        PAL_MasterQuest,
        NTSC_MasterQuest,
        PAL_GameCube_Beta,
        PAL_GameCube,
        NTSC_GameCube,
        Lodgenet,
        NTSC_Majora,
        PAL_Majora1_1,
        Debug_Majora,
        GC_NTSC_Majora,
        PAL_Majora1_0,
        NTSC_KIOSK_Majora,
        GC_PAL_Majora,
    }



    public static class ROMInfo
    {
        public static Dictionary<ROMVer, List<int>> ROMTSFixer = new Dictionary<ROMVer, List<int>>()
        {
            { ROMVer.Debug, new List<int>()                 { 0xAE60B6, 0x0700, 0xAE60BA, 0, 0xAE60C6, 0x0700, 0xAE60F2, 0x48} },
            { ROMVer.PAL_GameCube_Beta, new List<int>()     { 0xAE60B6, 0x0700, 0xAE60BA, 0, 0xAE60C6, 0x0700, 0xAE60F2, 0x48} },
            { ROMVer.PAL_MasterQuest, new List<int>()       { 0xAD2A52, 0x0700, 0xAD2A56, 0, 0xAD2A86, 0x0700, 0xAD2A8E, 0x48} },
            { ROMVer.PAL_GameCube, new List<int>()          { 0xAD2A52, 0x0700, 0xAD2A56, 0, 0xAD2A86, 0x0700, 0xAD2A8E, 0x48} },
            { ROMVer.PAL1_0, new List<int>()                { 0xAD3CA2, 0x0700, 0xAD3CA6, 0, 0xAD3CD6, 0x0700, 0xAD3CDE, 0x48} },
            { ROMVer.PAL1_1, new List<int>()                { 0xAD3CA2, 0x0700, 0xAD3CA6, 0, 0xAD3CD6, 0x0700, 0xAD3CDE, 0x48} },
        };

        public static Dictionary<ROMVer, int> ROMBuildDatesOffsets = new Dictionary<ROMVer, int>()
        {
            { ROMVer.NTSC_0_9, 0x740C },
            { ROMVer.NTSC_1_0, 0x740C },
            { ROMVer.NTSC_1_1, 0x740C },
            { ROMVer.NTSC_1_2, 0x793C },
            { ROMVer.PAL1_0, 0x792C },
            { ROMVer.PAL1_1, 0x792C },
            { ROMVer.Debug, 0x12F50 },
            { ROMVer.PAL_GameCube_Beta, 0x12F50 },
            { ROMVer.PAL_GameCube, 0x7150 },
            { ROMVer.NTSC_GameCube, 0x7150 },
            { ROMVer.PAL_MasterQuest, 0x7150 },
            { ROMVer.NTSC_MasterQuest, 0x7150 },
            { ROMVer.Lodgenet, 0x7A1C },
            { ROMVer.NTSC_Majora, 0x1A4DC },
            { ROMVer.PAL_Majora1_1, 0x1A8AC },
            { ROMVer.Debug_Majora, 0x24F3C },
            { ROMVer.GC_NTSC_Majora, 0x1AE70 },
            { ROMVer.PAL_Majora1_0, 0x1A62C },
            { ROMVer.NTSC_KIOSK_Majora, 0x1AB2C },
            { ROMVer.GC_PAL_Majora, 0x1AE70 },
        };

        private static Dictionary<ROMVer, string> ROMBuildDates = new Dictionary<ROMVer, string>()
        {
            { ROMVer.NTSC_0_9, "98-10-18 23:05:00" },
            { ROMVer.NTSC_1_0, "98-10-21 04:56:31" },
            { ROMVer.NTSC_1_1, "98-10-26 10:58:45" },
            { ROMVer.NTSC_1_2, "98-11-12 18:17:03" },
            { ROMVer.PAL1_0, "98-11-10 14:34:22" },
            { ROMVer.PAL1_1, "98-11-18 17:36:49" },
            { ROMVer.Debug, "03-02-21 00:16:31" },
            { ROMVer.PAL_MasterQuest, "03-02-21 20:37:19" },
            { ROMVer.NTSC_MasterQuest, "02-12-19 14:05:42" },
            { ROMVer.Lodgenet, "99-01-19 11:37:17" },
            { ROMVer.NTSC_Majora, "00-07-31 17:04:16" },
            { ROMVer.PAL_Majora1_1, "00-09-29 09:29:41" },
            { ROMVer.Debug_Majora, "00-09-29 09:29:05" },
            { ROMVer.GC_NTSC_Majora, "03-08-26 04:20:25" },
            { ROMVer.PAL_Majora1_0, "00-09-25 11:16:53" },
            { ROMVer.NTSC_KIOSK_Majora, "00-07-12 16:14:06" },
            { ROMVer.GC_PAL_Majora, "03-10-04 00:40:20" },
            { ROMVer.PAL_GameCube_Beta, "03-02-21 00:49:18" },
            { ROMVer.PAL_GameCube, "03-02-21 20:12:23" },
            { ROMVer.NTSC_GameCube, "02-12-19 13:28:09" }

        };

        public static ROMVer GetROMVerFromDate(ROMVer ROMVerOffsetUsed, string Date)
        {
            foreach (ROMVer v in ROMBuildDates.Keys)
            {
                if (ROMBuildDates[v] == Date && v == ROMVerOffsetUsed)
                    return v;
            }

            return ROMVer.Unknown;
        }

        public static bool IsMajoraMask(ROMVer Ver)
        {
            return (int)Ver >= (int)ROMVer.NTSC_Majora;
        }

        public static bool UseSeg8(ROMVer Ver)
        {
            return Ver.ToString().Contains("Majora") && Ver.ToString().Contains("NTSC");
        }


        private static Dictionary<ROMVer, List<int>> OffsetsData = new Dictionary<ROMVer, List<int>>()
        {
            { ROMVer.NTSC_0_9,  new List<int>()             { 0x00B847DC,     0x00B889FC,       0x92D000,     0x0966000,      16928,    392,    229648,     3968,   0 } },
            { ROMVer.NTSC_1_0,  new List<int>()             { 0x00B849EC,     0x00B88C0C,       0x92D000,     0x0966000,      16928,    392,    229680,     3936,   0 } },
            { ROMVer.NTSC_1_1,  new List<int>()             { 0x00B84BAC,     0x00B88DCC,       0x92D000,     0x0966000,      16928,    392,    229632,     3936,   0 } },
            { ROMVer.NTSC_1_2,  new List<int>()             { 0x00B84A5C,     0x00B88C7C,       0x92D000,     0x0966000,      16928,    392,    229600,     3936,   0 } },
            { ROMVer.NTSC_MasterQuest,  new List<int>()     { 0x00B830CC,     0x00B872EC,       0x92C000,     0x0965000,      16928,    392,    229600,     3936,   0 } },
            { ROMVer.NTSC_GameCube,  new List<int>()        { 0x00B830EC,     0x00B8730C,       0x92C000,     0x0965000,      16928,    392,    229600,     3936,   0 } },
            { ROMVer.PAL1_0,  new List<int>()               { 0x00B801DC,     0x00B88624,       0x8BB000,     0x0968000,      16936,    392,    708608,     3920,   0 } },
            { ROMVer.PAL1_1,  new List<int>()               { 0x00B8021C,     0x00B88664,       0x8BB000,     0x0968000,      16936,    392,    708608,     3920,   0 } },
            { ROMVer.PAL_GameCube,  new List<int>()         { 0x00B7E910,     0x00B86D58,       0x8BA000,     0x0967000,      33860,    392,    708608,     3920,   0 } },
            { ROMVer.PAL_MasterQuest,  new List<int>()      { 0x00B7E8F0,     0x00B86D38,       0x8BA000,     0x0967000,      16936,    392,    708608,     3920,   0 } },
            { ROMVer.Debug, new List<int>()                 { 0x00BC24C0,     0x00BCA908,       0x8C6000,     0x0973000,      33860,    392,    704960,     3920,   0 } },
            { ROMVer.PAL_GameCube_Beta, new List<int>()     { 0x00BC24E0,     0x00BCA928,       0x8C6000,     0x0973000,      33860,    392,    704960,     3920,   0 } },
            { ROMVer.Lodgenet,  new List<int>()             { 0x00B82C0C,     0x00B86E2C,       0x92D000,     0x0966000,      16928,    392,    233472,     3920,   0 } },
            { ROMVer.NTSC_Majora,  new List<int>()          { 0x00C5D0D8,     0x00C66048,       0xAD1000,     0x0B3B000,      36720,    368,    432624,     3680,   0xC679AC } },
            { ROMVer.GC_NTSC_Majora,  new List<int>()       { 0x00C6A648,     0x00C735B8,       0xAE0000,     0x0B4A000,      36720,    368,    432864,     3680,   0xC752AC } },
            { ROMVer.NTSC_KIOSK_Majora,  new List<int>()    { 0x00C5D2D8,     0x00C66240,       0xAD2000,     0x0B3C000,      36712,    368,    432864,     3296,   0 } },
            { ROMVer.PAL_Majora1_0,  new List<int>()        { 0x00C66000,     0x00DABF58,       0xAA5000,     0x0C65000,      147456,   368,    432768,     3712,   0xDADC80 } },
            { ROMVer.PAL_Majora1_1,  new List<int>()        { 0x00C66000,     0x00DAC078,       0xAA5000,     0x0C65000,      147456,   368,    432768,     3712,   0xDADDA0 } },
            { ROMVer.Debug_Majora,  new List<int>()         { 0x00C71000,     0x00DEEE28,       0xAB0000,     0x0C70000,      147456,   368,    432768,     3712,   0xDF0EF0 } },
            { ROMVer.GC_PAL_Majora,  new List<int>()        { 0x00C75000,     0x00DB9078,       0xAB4000,     0x0C74000,      147456,   368,    432768,     3712,   0xDBADA0 } },
        };

        private enum OffsetsDataIdx
        {
            TableOffset,
            CreditsTableOffset,
            MessagesOffset,
            CreditsMessagesOffset,
            TableSizeMax,
            CreditsTableSizeMax,
            MessagesSizeMax,
            CreditsMessagesSizeMax,
            BomberNotebookMessagesList,
        }

        public static int ZZRPCodeFileTablePostion = 0x0012E4C0;

        public static int GetBomberNotebookOffset(ROMVer Version)
        {
            return OffsetsData[Version][(int)OffsetsDataIdx.BomberNotebookMessagesList];
        }

        public static int GetTableOffset(ROMVer Version, bool Credits)
        {
            return Credits ? OffsetsData[Version][(int)OffsetsDataIdx.CreditsTableOffset] : OffsetsData[Version][(int)OffsetsDataIdx.TableOffset];
        }

        public static int GetMessagesOffset(ROMVer Version, bool Credits)
        {
            return Credits ? OffsetsData[Version][(int)OffsetsDataIdx.CreditsMessagesOffset] : OffsetsData[Version][(int)OffsetsDataIdx.MessagesOffset];
        }

        public static int GetTableMaxSize(ROMVer Version, bool Credits)
        {
            return Credits ? OffsetsData[Version][(int)OffsetsDataIdx.CreditsTableSizeMax] : OffsetsData[Version][(int)OffsetsDataIdx.TableSizeMax];
        }
        public static int GetMessagesMaxSize(ROMVer Version, bool Credits)
        {
            return Credits ? OffsetsData[Version][(int)OffsetsDataIdx.CreditsMessagesSizeMax] : OffsetsData[Version][(int)OffsetsDataIdx.MessagesSizeMax];
        }
    }


}
