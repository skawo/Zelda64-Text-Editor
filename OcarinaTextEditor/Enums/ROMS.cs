using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcarinaTextEditor
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
        NTSC_Majora,
    }



    public static class ROMInfo
    {
        public static Dictionary<ROMVer, List<int>> ROMTSFixer = new Dictionary<ROMVer, List<int>>()
        {
            { ROMVer.Debug, new List<int>()                 { 0xAE60B6, 0x0700, 0xAE60BA, 0, 0xAE60C6, 0x0700, 0xAE60F2, 0x48} },
            { ROMVer.PAL_MasterQuest, new List<int>()       { 0xAD2A52, 0x0700, 0xAD2A56, 0, 0xAD2A86, 0x0700, 0xAD2A8E, 0x48} },
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
            { ROMVer.PAL_MasterQuest, 0x7150 },
            { ROMVer.NTSC_MasterQuest, 0x7150 },
            { ROMVer.NTSC_Majora, 0x1A4DC },
        };

        private static Dictionary<ROMVer, string> ROMBuildDates = new Dictionary<ROMVer, string>()
        {
            { ROMVer.NTSC_0_9, "98-10-18" },
            { ROMVer.NTSC_1_0, "98-10-21" },
            { ROMVer.NTSC_1_1, "98-10-26" },
            { ROMVer.NTSC_1_2, "98-11-12" },
            { ROMVer.PAL1_0, "98-11-10" },
            { ROMVer.PAL1_1, "98-11-18" },
            { ROMVer.Debug, "03-02-21" },
            { ROMVer.PAL_MasterQuest, "03-02-21" },
            { ROMVer.NTSC_MasterQuest, "02-12-19" },
            { ROMVer.NTSC_Majora, "00-07-31" },
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

        private static Dictionary<ROMVer, List<int>> OffsetsData = new Dictionary<ROMVer, List<int>>()
        {
                                     
            { ROMVer.Debug, new List<int>()                 { 0x00BC24C0,     0x00BCA908,       0x8C6000,     0x0973000,      33860,    392,    704960,     3920 } },
            { ROMVer.NTSC_0_9,  new List<int>()             { 0x00B847DC,     0x00B889FC,       0x92D000,     0x0966000,      16928,    392,    229648,     3968 } },
            { ROMVer.NTSC_1_0,  new List<int>()             { 0x00B849EC,     0x00B88C0C,       0x92D000,     0x0966000,      16928,    392,    231568,     3920 } },
            { ROMVer.NTSC_1_1,  new List<int>()             { 0x00B84BAC,     0x00B88DCC,       0x92D000,     0x0966000,      16928,    392,    229632,     3936 } },
            { ROMVer.NTSC_1_2,  new List<int>()             { 0x00B84A5C,     0x00B88C7C,       0x92D000,     0x0966000,      16928,    392,    229600,     3936 } },
            { ROMVer.NTSC_MasterQuest,  new List<int>()     { 0x00B830CC,     0x00B872EC,       0x92C000,     0x0965000,      16928,    392,    229440,     3920 } },
            { ROMVer.PAL1_0,  new List<int>()               { 0x00B801DC,     0x00B88624,       0x8BB000,     0x0968000,      16936,    392,    705281,     3920 } },
            { ROMVer.PAL1_1,  new List<int>()               { 0x00B8021C,     0x00B88664,       0x8BB000,     0x0968000,      16936,    392,    705281,     3920 } },
            { ROMVer.PAL_MasterQuest,  new List<int>()      { 0x00B7E8F0,     0x00B86D38,       0x8BA000,     0x0967000,      16936,    392,    704945,     3920 } },
            { ROMVer.NTSC_Majora,  new List<int>()          { 0x00C5D0D8,     0x00B86D38,       0xAD1000,     0x0967000,      36720,    392,    432624,     3920 } },
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
        }

        public static int ZZRPCodeFileTablePostion = 0x0012E4C0;

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
