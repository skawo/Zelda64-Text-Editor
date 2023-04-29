using GameFormatReader.Common;
using Zelda64TextEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda64TextEditor
{
    public class TableRecord
    {
        public short MessageID { get; set; }
        public OcarinaTextboxType BoxType { get; set; }
        public TextboxPosition BoxPosition { get; set; }

        public uint Offset
        {
            get { return offset; }
            set { offset = value & 0x00FFFFFF; }
        }
        private uint offset;

        public TableRecord(EndianBinaryReader reader)
        {
            MessageID = reader.ReadInt16();

            byte typePosField = reader.ReadByte();

            BoxType = (OcarinaTextboxType)((typePosField & 0xF0) >> 4);
            BoxPosition = (TextboxPosition)(typePosField & 0x0F);

            reader.SkipByte();

            Offset = reader.ReadUInt32();
        }
    }
}
