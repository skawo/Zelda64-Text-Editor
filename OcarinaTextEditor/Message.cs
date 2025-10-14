using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zelda64TextEditor.Enums;
using GameFormatReader.Common;

namespace Zelda64TextEditor
{
    public class Message : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged overhead
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region public short MessageID
        public short MessageID
        {
            get { return m_messageID; }
            set
            {
                if (value != m_messageID)
                {
                    m_messageID = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private short m_messageID;
        #endregion

        #region public TextboxType BoxType
        public OcarinaTextboxType BoxType
        {
            get { return m_boxType; }
            set
            {
                if (value != m_boxType)
                {
                    m_boxType = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private OcarinaTextboxType m_boxType;
        #endregion

        #region public TextboxPosition BoxPosition
        public TextboxPosition BoxPosition
        {
            get { return m_boxPosition; }
            set
            {
                if (value != m_boxPosition)
                {
                    m_boxPosition = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private TextboxPosition m_boxPosition;
        #endregion

        #region public string TextData
        public string TextData
        {
            get { return m_textData; }
            set
            {
                if (value != m_textData)
                {
                    m_textData = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string m_textData;
        #endregion

        #region public byte MajoraIcon
        public byte MajoraIcon
        {
            get { return m_MajoraIcon; }
            set
            {
                if (value != m_MajoraIcon)
                {
                    m_MajoraIcon = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private byte m_MajoraIcon;
        #endregion

        #region public short MajoraNextMessage
        public short MajoraNextMessage
        {
            get { return m_MajoraNext; }
            set
            {
                if (value != m_MajoraNext)
                {
                    m_MajoraNext = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private short m_MajoraNext;

        #endregion

        #region public short MajoraFirstItemPrice
        public short MajoraFirstItemPrice
        {
            get { return m_MajoraFirstItemPrice; }
            set
            {
                if (value != m_MajoraFirstItemPrice)
                {
                    m_MajoraFirstItemPrice = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private short m_MajoraFirstItemPrice;
        #endregion

        #region public short MajoraSecondItemPrice
        public short MajoraSecondItemPrice
        {
            get { return m_MajoraSecondItemPrice; }
            set
            {
                if (value != m_MajoraSecondItemPrice)
                {
                    m_MajoraSecondItemPrice = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private short m_MajoraSecondItemPrice;
        #endregion

        #region public MajoraTextboxType BoxType
        public MajoraTextboxType MajoraBoxType
        {
            get { return m_MajoraBoxType; }
            set
            {
                if (value != m_MajoraBoxType)
                {
                    m_MajoraBoxType = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MajoraTextboxType m_MajoraBoxType;
        #endregion

        #region unskippable
        public bool Unskippable
        {
            get { return m_UnskippableMM; }
            set
            {
                if (value != m_UnskippableMM)
                {
                    m_UnskippableMM = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool m_UnskippableMM;
        #endregion


        #region UnknownMM
        public bool MajoraTextCenter
        {
            get { return m_unkMM; }
            set
            {
                if (value != m_unkMM)
                {
                    m_unkMM = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool m_unkMM;
        #endregion

        #region Unknown2MM
        public bool MajoraDrawInstantly
        {
            get { return m_typeOutInstant; }
            set
            {
                if (value != m_typeOutInstant)
                {
                    m_typeOutInstant = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool m_typeOutInstant;
        #endregion


        public Message()
        {
            TextData = "";
        }

        public Message(byte[] Message, TableRecord mesgTableRecord, bool Credits, ROMVer Version = ROMVer.Unknown)
        {
            EndianBinaryReader reader = new EndianBinaryReader(Message, Endian.Big);

            if (ROMInfo.IsMajoraMask(Version) && !Credits)
            {
                MessageID = mesgTableRecord.MessageID;

                byte b1 = reader.ReadByte();
                byte b2 = reader.ReadByte();
                int settings = b1;
                settings <<= 8;
                settings |= b2;

                MajoraTextCenter = ((settings & 0xF000) >> 0xC) == 1;
                MajoraBoxType = (MajoraTextboxType)((settings & 0xF00) >> 0x8);
                BoxPosition = (TextboxPosition)((settings & 0xF0) >> 0x4);
                Unskippable = (settings & 0x1) == 1;
                MajoraDrawInstantly = (settings & 0x3) == 3;

                MajoraIcon = reader.ReadByte();

                MajoraNextMessage = reader.ReadInt16();
                MajoraFirstItemPrice = reader.ReadInt16();
                MajoraSecondItemPrice = reader.ReadInt16();
                reader.ReadInt16();     // Padding

                GetStringDataMajora(reader);
            }
            else
            {
                MessageID = mesgTableRecord.MessageID;
                BoxType = mesgTableRecord.BoxType;
                BoxPosition = mesgTableRecord.BoxPosition;

                GetStringData(reader);
            }
        }

        public Message(EndianBinaryReader reader, TableRecord mesgTableRecord, bool Credits, ROMVer Version = ROMVer.Unknown)
        {
            if (ROMInfo.IsMajoraMask(Version) && !Credits)
            {
                MessageID = mesgTableRecord.MessageID;

                byte b1 = reader.ReadByte();
                byte b2 = reader.ReadByte();
                int settings = b1;
                settings <<= 8;
                settings |= b2;

                MajoraTextCenter = ((settings & 0xF000) >> 0xC) == 1;
                MajoraBoxType = (MajoraTextboxType)((settings & 0xF00) >> 0x8);
                BoxPosition = (TextboxPosition)((settings & 0xF0) >> 0x4);
                Unskippable = (settings & 0x1) == 1;
                MajoraDrawInstantly = (settings & 0x3) == 3;

                MajoraIcon = reader.ReadByte();

                MajoraNextMessage = reader.ReadInt16();
                MajoraFirstItemPrice = reader.ReadInt16();
                MajoraSecondItemPrice = reader.ReadInt16();
                reader.ReadInt16();     // Padding

                GetStringDataMajora(reader);
            }
            else
            {
                MessageID = mesgTableRecord.MessageID;
                BoxType = mesgTableRecord.BoxType;
                BoxPosition = mesgTableRecord.BoxPosition;

                GetStringData(reader);
            }
        }

        public Message(string Message, OcarinaTextboxType t)
        {
            TextData = Message;
            BoxType = t;
        }

        public void Print()
        {
            string printString = string.Format("ID: {0}\nBox Type: {1}\nBox Pos: {2}\nData:\n{3}\n\n", MessageID, BoxType, BoxPosition, TextData);
            Console.Write(printString);
        }

        private void GetStringDataMajora(EndianBinaryReader reader)
        {
            List<char> charData = new List<char>();

            byte testByte = reader.ReadByte();

            while (testByte != (byte)MajoraControlCode.END)
            {
                bool readControlCode = false;

                // Control tags
                if (testByte < 0x7F || testByte > 0xAF)
                {
                    if (Enum.IsDefined(typeof(MajoraControlCode), (int)testByte))
                    {
                        charData.AddRange(GetMajoraControlCode((MajoraControlCode)testByte, reader));
                        readControlCode = true;
                    }
                }

                if (!readControlCode)
                {
                    if (testByte == 0x7F)
                    {
                        // Never actually used in-game. Appears blank.
                        charData.Add(' ');
                    }
                    // Stressed characters that don't map to ASCII directly
                    else if (testByte >= 0x80 && testByte <= 0xAF)
                    {
                        // Can't be used as indice names in the enum. Ugh.
                        if (testByte == 0xAD)
                            charData.Add('¡');
                        else if (testByte == 0xAE)
                            charData.Add('¿');
                        else
                            charData.Add(Enum.GetName(typeof(MajoraControlCode), testByte).First());
                    }
                    // The rest~
                    else if ((testByte >= 0x20 && testByte < 0x7F) || char.IsLetterOrDigit((char)testByte) || char.IsWhiteSpace((char)testByte) || char.IsPunctuation((char)testByte))
                    {
                        charData.Add((char)testByte);
                    }
                    else
                    {
                        charData.AddRange($"<UNK {testByte:X}>");
                    }
    
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                    testByte = reader.ReadByte();
                else
                    testByte = (byte)MajoraControlCode.END;
            }

            TextData = new String(charData.ToArray());
        }

        private void GetStringData(EndianBinaryReader reader)
        {
            List<char> charData = new List<char>();

            byte testByte = reader.ReadByte();

            while (testByte != (byte)OcarinaControlCode.END)
            {
                bool readControlCode = false;

                if (testByte < 0x7F || testByte > 0x9E)
                {
                    if (Enum.IsDefined(typeof(OcarinaControlCode), (int)testByte))
                    {
                        charData.AddRange(GetControlCode((OcarinaControlCode)testByte, reader));
                        readControlCode = true;
                    }
                }

                if (!readControlCode)
                {
                    if (testByte == 0x7F)
                    {
                        // Never actually used in-game. Appears blank.
                        charData.Add(' ');
                    }
                    // Stressed characters
                    else if (testByte >= 0x80 && testByte <= 0x9E)
                    {
                        charData.Add(Enum.GetName(typeof(OcarinaControlCode), testByte).First());
                    }
                    // ASCII-mapped characters
                    else if((testByte >= 0x20 && testByte < 0x7F) || (char.IsLetterOrDigit((char)testByte) || char.IsWhiteSpace((char)testByte) || char.IsPunctuation((char)testByte)))
                    {
                        charData.Add((char)testByte);
                    }
                    else
                    {
                        charData.AddRange($"<UNK {testByte:X}>");
                    }
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                    testByte = reader.ReadByte();
                else
                    testByte = 0x02;
            }

            TextData = new String(charData.ToArray());
        }

        private char[] GetControlCode(OcarinaControlCode code, EndianBinaryReader reader)
        {
            List<char> codeBank = new List<char>();
            string codeInsides = "";

            switch (code)
            {
                case OcarinaControlCode.COLOR:
                    OcarinaMsgColor col = (OcarinaMsgColor)reader.ReadByte();
                    codeInsides = col.ToString();
                    break;
                case OcarinaControlCode.ICON:
                    int iconID = (int)reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.ICON.ToString(), Enum.IsDefined(typeof(OcarinaIcon), iconID) ? ((OcarinaIcon)iconID).ToString() : iconID.ToString());
                    break;
                case OcarinaControlCode.LINE_BREAK:
                    return "\n".ToCharArray();
                case OcarinaControlCode.SHIFT:
                    byte numSpaces = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SHIFT.ToString(), numSpaces);
                    break;
                case OcarinaControlCode.DELAY:
                    byte numFrames = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.DELAY.ToString(), numFrames);
                    break;
                case OcarinaControlCode.FADE:
                    byte numFramesFade = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.FADE.ToString(), numFramesFade);
                    break;
                case OcarinaControlCode.FADE2:
                    short numFramesFade2 = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.FADE2.ToString(), numFramesFade2);
                    break;
                case OcarinaControlCode.SOUND:
                    short soundID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SOUND.ToString(), Dicts.SFXes.ContainsValue(soundID) ? Dicts.SFXes.FirstOrDefault(x => x.Value == soundID).Key : "0x" + soundID.ToString("X"));
                    break;
                case OcarinaControlCode.SPEED:
                    byte speed = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SPEED.ToString(), speed);
                    break;
                case OcarinaControlCode.HIGH_SCORE:
                    int scoreID = (int)reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.HIGH_SCORE.ToString(), Enum.IsDefined(typeof(OcarinaHighScore), scoreID) ? ((OcarinaHighScore)scoreID).ToString() : scoreID.ToString());
                    break;
                case OcarinaControlCode.JUMP:
                    short msgID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1:X4}", OcarinaControlCode.JUMP.ToString(), msgID);
                    break;
                case OcarinaControlCode.NEW_BOX:
                    return ($"{Environment.NewLine}<{OcarinaControlCode.NEW_BOX.ToString()}>{Environment.NewLine}").ToCharArray();
                case OcarinaControlCode.NS:
                    codeInsides = OcarinaControlCode.NS.ToString();
                    break;
                case OcarinaControlCode.DI:
                    codeInsides = OcarinaControlCode.DI.ToString();
                    break;
                case OcarinaControlCode.DC:
                    codeInsides = OcarinaControlCode.DC.ToString();
                    break;
                case OcarinaControlCode.BACKGROUND:
                    int backgroundID;
                    byte id1 = reader.ReadByte();
                    byte id2 = reader.ReadByte();
                    byte id3 = reader.ReadByte();
                    backgroundID = BitConverter.ToInt32(new byte[] { id3, id2, id1, 0 }, 0 );
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.BACKGROUND.ToString(), backgroundID);
                    break;

                default:
                    codeInsides = code.ToString();
                    break;
            }

            codeBank.AddRange(string.Format("<{0}>", codeInsides).ToCharArray());

            return codeBank.ToArray();
        }

        private char[] GetMajoraControlCode(MajoraControlCode code, EndianBinaryReader reader)
        {
            List<char> codeBank = new List<char>();
            string codeInsides = "";

            switch (code)
            {
                case MajoraControlCode.COLOR_DEFAULT:
                case MajoraControlCode.COLOR_RED:
                case MajoraControlCode.COLOR_GREEN:
                case MajoraControlCode.COLOR_BLUE:
                case MajoraControlCode.COLOR_YELLOW:
                case MajoraControlCode.COLOR_NAVY:
                case MajoraControlCode.COLOR_PINK:
                case MajoraControlCode.COLOR_SILVER:
                case MajoraControlCode.COLOR_ORANGE:
                    codeInsides = string.Format("{0}", ((MajoraMsgColor)code).ToString());
                    break;
                case MajoraControlCode.SHIFT:
                    byte numSpaces = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", MajoraControlCode.SHIFT.ToString(), numSpaces);
                    break;
                case MajoraControlCode.LINE_BREAK:
                    return "\n".ToCharArray();
                case MajoraControlCode.NEW_BOX:
                    return ($"{Environment.NewLine}<{MajoraControlCode.NEW_BOX.ToString()}>{Environment.NewLine}").ToCharArray();
                case MajoraControlCode.NEW_BOX_CENTER:
                    return ($"{Environment.NewLine}<{MajoraControlCode.NEW_BOX_CENTER.ToString()}>{Environment.NewLine}").ToCharArray();
                case MajoraControlCode.DELAY:
                case MajoraControlCode.DELAY_NEWBOX:
                case MajoraControlCode.DELAY_END:
                case MajoraControlCode.FADE:
                    short delay = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", code.ToString(), delay);
                    break;
                case MajoraControlCode.SOUND:
                    short soundID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SOUND.ToString(), Dicts.SFXes.ContainsValue(soundID) ? Dicts.SFXes.FirstOrDefault(x => x.Value == soundID).Key : "0x" + soundID.ToString("X"));
                    break;
                default:
                    codeInsides = code.ToString();
                    break;
            }

            codeBank.AddRange(string.Format("<{0}>", codeInsides).ToCharArray());

            return codeBank.ToArray();
        }

        public void WriteMessage(EndianBinaryWriter writer, ROMVer Version, bool Credits)
        {
            if (ROMInfo.IsMajoraMask(Version) && !Credits)
            {
                writer.Write(m_messageID);
                writer.Write((short)0);
                writer.Write((int)0);
            }
            else
            {
                writer.Write(m_messageID);

                int type = (int)BoxType;
                int pos = (int)BoxPosition;
                type <<= 4;
                type |= pos;

                writer.Write((byte)type);
                writer.Write((byte)0);
                writer.Write((int)0);
            }
        }

        public List<byte> ConvertTextData(ROMVer Version, bool Credits, bool ShowErrors = true)
        {
            if (ROMInfo.IsMajoraMask(Version) && !Credits)
                return ConvertMajoraTextData(ShowErrors);
            else
                return ConvertTextData(ShowErrors);
        }

        public string ConvertToCString(ROMVer Version, bool Credits, bool ShowErrors = true)
        {
            if (ROMInfo.IsMajoraMask(Version) && !Credits)
                return ConvertMajoraTextToCString(ShowErrors);
            else
                return ConvertTextToCString(ShowErrors);
        }

        private string ConvertMajoraTextToCString(bool ShowErrors = true)
        {
            string outS = "\"";
            List<string> errors = new List<string>();

            /*
            outS += GetXString((byte)this.MajoraBoxType);
            outS += GetXString((byte)this.BoxPosition);
            outS += GetXString((byte)this.MajoraIcon);

            byte[] nextMsgBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraNextMessage));
            outS += GetXString(nextMsgBytes[1]);
            outS += GetXString(nextMsgBytes[0]);

            byte[] firstPriceBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraFirstItemPrice));
            outS += GetXString(firstPriceBytes[1]);
            outS += GetXString(firstPriceBytes[0]);

            byte[] secondPriceBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraSecondItemPrice));
            outS += GetXString(secondPriceBytes[1]);
            outS += GetXString(secondPriceBytes[0]);

            outS += GetXString(0xFF);
            outS += GetXString(0xFF);
            */

            for (int i = 0; i < TextData.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (TextData[i] != '<' && TextData[i] != '>')
                {
                    if (TextData[i] == '¡')
                    {
                        outS += GetXString(0xAD);
                    }
                    else if (TextData[i] == '¿')
                    {
                        outS += GetXString(0xAE);
                    }
                    else if (TextData[i] == '"')
                    {
                        outS += GetXString((byte)'"');
                    }
                    else if (Enum.IsDefined(typeof(MajoraControlCode), TextData[i].ToString()))
                    {
                        _ = Enum.TryParse(TextData[i].ToString(), out MajoraControlCode Result);
                        outS += (char)Result;
                    }
                    else if (TextData[i] == '\n')
                        outS += GetXString((byte)MajoraControlCode.LINE_BREAK);
                    else if (TextData[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                        outS += (char)TextData[i];

                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (TextData[i] == '>')
                    errors.Add($"Message formatting is not valid: found stray >");
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (TextData[i] != '>' && i < TextData.Length - 1)
                    {
                        // Add code chars to the buffer
                        controlCode.Add(TextData[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    if (controlCode.Count == 0)
                        continue;

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());
                    string parsedFixed = parsedCode.Split(':')[0].Replace(" ", "_").ToUpper();

                    if (parsedFixed == MajoraControlCode.NEW_BOX.ToString() || parsedFixed == MajoraControlCode.DELAY_END.ToString() || parsedFixed == MajoraControlCode.NEW_BOX_CENTER.ToString())
                    {
                        outS = outS.Remove(outS.LastIndexOf(GetXString((byte)MajoraControlCode.LINE_BREAK)));

                        if (TextData.Length > i + Environment.NewLine.Length)
                        {
                            string s;

                            if (Environment.NewLine.Length == 2)
                                s = String.Concat(TextData[i + 1], TextData[i + 2]);
                            else
                                s = String.Concat(TextData[i + 1]);

                            if (s == Environment.NewLine)
                                i += Environment.NewLine.Length; // Skips next linebreak
                        }
                    }

                    List<byte> data = GetMajoraControlCode(parsedCode.Split(':'), ref errors);

                    foreach (byte b in data)
                    {
                        outS += GetXString(b);
                    }
                }
            }

            outS += GetXString((byte)MajoraControlCode.END);

            if (ShowErrors && errors.Count != 0)
                System.Windows.Forms.MessageBox.Show($"Errors parsing message {MessageID}: " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

            if (errors.Count == 0)
                return outS.TrimEnd('\"') + "\"";
            else
                return "";
        }

        private List<byte> ConvertMajoraTextData(bool ShowErrors = true)
        {
            List<byte> data = new List<byte>();
            List<string> errors = new List<string>();

            int settings = 0;

            // Pack the fields back into the settings integer
            settings |= (MajoraTextCenter ? 1 : 0) << 0xC;        // Upper 4 bits (0xF000)
            settings |= ((int)MajoraBoxType & 0xF) << 0x8;    // Next 4 bits (0xF00)
            settings |= ((int)BoxPosition & 0xF) << 0x4;      // Next 4 bits (0xF0)

            // Handle the lower 4 bits based on Unskippable and Unknown2MM
            int lowerN = 0;

            if (Unskippable)
                lowerN |= 1;
            if (MajoraDrawInstantly)
                lowerN |= 3;

            settings |= lowerN & 0xF;                    // Lower 4 bits (0xF)

            // Extract the two bytes
            byte b1 = (byte)((settings >> 8) & 0xFF);
            byte b2 = (byte)(settings & 0xFF);

            data.Add(b1);
            data.Add(b2);
            data.Add((byte)this.MajoraIcon);

            byte[] nextMsgBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraNextMessage));
            data.Add(nextMsgBytes[1]);
            data.Add(nextMsgBytes[0]);

            byte[] firstPriceBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraFirstItemPrice));
            data.Add(firstPriceBytes[1]);
            data.Add(firstPriceBytes[0]);

            byte[] secondPriceBytes = BitConverter.GetBytes(Convert.ToInt16(this.MajoraSecondItemPrice));
            data.Add(secondPriceBytes[1]);
            data.Add(secondPriceBytes[0]);

            data.AddRange(new byte[] { 0xFF, 0xFF });

            for (int i = 0; i < TextData.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (TextData[i] != '<' && TextData[i] != '>')
                {
                    if (TextData[i] == '¡')
                    {
                        data.Add(0xAD);
                    }
                    else if (TextData[i] == '¿')
                    {
                        data.Add(0xAE);
                    }
                    else if (Enum.IsDefined(typeof(MajoraControlCode), TextData[i].ToString()))
                    {
                        _ = Enum.TryParse(TextData[i].ToString(), out MajoraControlCode Result);
                        data.Add((byte)Result);
                    }
                    else if (TextData[i] == '\n')
                        data.Add((byte)MajoraControlCode.LINE_BREAK);
                    else if (TextData[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                        data.Add((byte)TextData[i]);

                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (TextData[i] == '>')
                    errors.Add($"Message formatting is not valid: found stray >");
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (TextData[i] != '>' && i < TextData.Length - 1)
                    {
                        // Add code chars to the buffer
                        controlCode.Add(TextData[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    if (controlCode.Count == 0)
                        continue;

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());
                    string parsedFixed = parsedCode.Split(':')[0].Replace(" ", "_").ToUpper();

                    if (parsedFixed == MajoraControlCode.NEW_BOX.ToString() || parsedFixed == MajoraControlCode.DELAY_END.ToString() || parsedFixed == MajoraControlCode.NEW_BOX_CENTER.ToString())
                    {
                        if (data.Count != 0)
                            if (data[data.Count - 1] == 0x11)
                                data.RemoveAt(data.Count - 1);

                        if (TextData.Length > i + Environment.NewLine.Length)
                        {
                            string s;

                            if (Environment.NewLine.Length == 2)
                                s = String.Concat(TextData[i + 1], TextData[i + 2]);
                            else
                                s = String.Concat(TextData[i + 1]);

                            if (s == Environment.NewLine)
                                i += Environment.NewLine.Length; // Skips next linebreak
                        }
                    }

                    data.AddRange(GetMajoraControlCode(parsedCode.Split(':'), ref errors));
                }
            }

            data.Add((byte)MajoraControlCode.END);

            if (ShowErrors && errors.Count != 0)
                System.Windows.Forms.MessageBox.Show($"Errors parsing message {MessageID}: " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

            if (errors.Count == 0)
                return data;
            else
                return new List<byte>();
        }

        private string GetXString(byte Character)
        {
            return $"\\x{Character.ToString("X2")}\"\"";
        }

        private string ConvertTextToCString(bool ShowErrors = true)
        {
            string outS = "\"";
            List<string> errors = new List<string>();

            for (int i = 0; i < TextData.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (TextData[i] != '<' && TextData[i] != '>')
                {
                    if (Enum.IsDefined(typeof(OcarinaControlCode), TextData[i].ToString()))
                    {
                        _ = Enum.TryParse(TextData[i].ToString(), out OcarinaControlCode Result);
                        outS += (char)Result;
                    }
                    else if (TextData[i] == '"')
                    {
                        outS += GetXString((byte)'"');
                    }
                    else if (TextData[i] == '\n')
                        outS += GetXString((byte)OcarinaControlCode.LINE_BREAK);
                    else if (TextData[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                        outS += (char)TextData[i];

                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (TextData[i] == '>')
                    errors.Add($"Message formatting is not valid: found stray >");
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (TextData[i] != '>' && i < TextData.Length - 1)
                    {
                        // Add code chars to the buffer
                        controlCode.Add(TextData[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    if (controlCode.Count == 0)
                        continue;

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());
                    string parsedFixed = parsedCode.Split(':')[0].Replace(" ", "_").ToUpper();

                    if (parsedFixed == OcarinaControlCode.NEW_BOX.ToString() || parsedFixed == OcarinaControlCode.DELAY.ToString())
                    {
                        outS = outS.Remove(outS.LastIndexOf(GetXString((byte)OcarinaControlCode.LINE_BREAK)));

                        if (TextData.Length > i + Environment.NewLine.Length)
                        {
                            string s;

                            if (Environment.NewLine.Length == 2)
                                s = String.Concat(TextData[i + 1], TextData[i + 2]);
                            else
                                s = String.Concat(TextData[i + 1]);

                            if (s == Environment.NewLine)
                            {
                                i += Environment.NewLine.Length; // Skips next linebreak
                            }
                        }
                    }

                    List<byte> data = GetControlCode(parsedCode.Split(':'), ref errors);
                    
                    foreach (byte b in data)
                    {
                        outS += GetXString(b);
                    }
                }
            }

            outS += GetXString((byte)OcarinaControlCode.END);

            if (ShowErrors && errors.Count != 0)
                System.Windows.Forms.MessageBox.Show($"Errors parsing message {MessageID}: " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

            if (errors.Count == 0)
                return outS.TrimEnd('\"') + "\"";
            else
                return "";
        }


        private List<byte> ConvertTextData(bool ShowErrors = true)
        {
            List<byte> data = new List<byte>();
            List<string> errors = new List<string>();

            for (int i = 0; i < TextData.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (TextData[i] != '<' && TextData[i] != '>')
                {
                    if (Enum.IsDefined(typeof(OcarinaControlCode), TextData[i].ToString()))
                    {
                        _ = Enum.TryParse(TextData[i].ToString(), out OcarinaControlCode Result);
                        data.Add((byte)Result);
                    }
                    else if (TextData[i] == '\n')
                        data.Add((byte)OcarinaControlCode.LINE_BREAK);
                    else if (TextData[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                        data.Add((byte)TextData[i]);

                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (TextData[i] == '>')
                    errors.Add($"Message formatting is not valid: found stray >");
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (TextData[i] != '>' && i < TextData.Length - 1)
                    {
                        // Add code chars to the buffer
                        controlCode.Add(TextData[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    if (controlCode.Count == 0)
                        continue;

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());
                    string parsedFixed = parsedCode.Split(':')[0].Replace(" ", "_").ToUpper();

                    if (parsedFixed == OcarinaControlCode.NEW_BOX.ToString() || parsedFixed == OcarinaControlCode.DELAY.ToString())
                    {
                        if (data.Count != 0)
                            if (data[data.Count - 1] == 0x01)
                                data.RemoveAt(data.Count - 1);

                        if (TextData.Length > i + Environment.NewLine.Length)
                        {
                            string s;

                            if (Environment.NewLine.Length == 2)
                                s = String.Concat(TextData[i + 1], TextData[i + 2]);
                            else
                                s = String.Concat(TextData[i + 1]);

                            if (s == Environment.NewLine)
                            {
                                i += Environment.NewLine.Length; // Skips next linebreak
                            }
                        }
                    }

                    data.AddRange(GetControlCode(parsedCode.Split(':'), ref errors));
                }
            }

            data.Add((byte)OcarinaControlCode.END);

            if (ShowErrors && errors.Count != 0)
                System.Windows.Forms.MessageBox.Show($"Errors parsing message {MessageID}: " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

            if (errors.Count == 0)
                return data;
            else
                return new List<byte>();
        }

        public static Message MakeCopy(Message mes)
        {
            Message mesO = new Message();

            mesO.TextData = mes.TextData;
            mesO.MajoraFirstItemPrice = mes.MajoraFirstItemPrice;
            mesO.MajoraBoxType = mes.MajoraBoxType;
            mesO.MajoraIcon = mes.MajoraIcon;
            mesO.MajoraNextMessage = mes.MajoraNextMessage;
            mesO.MajoraSecondItemPrice = mes.MajoraSecondItemPrice;
            mesO.BoxPosition = mes.BoxPosition;
            mesO.BoxType = mes.BoxType;
            mesO.MessageID = mes.MessageID;
            mesO.Unskippable = mes.Unskippable;
            mesO.MajoraTextCenter = mes.MajoraTextCenter;
            mesO.MajoraDrawInstantly = mes.MajoraDrawInstantly;

            return mesO;
        }

        private List<byte> GetMajoraControlCode(string[] code, ref List<string> errors)
        {
            List<byte> output = new List<byte>();
            try
            {
                for (int i = 0; i < code.Length; i++)
                    code[i] = code[i].Replace(" ", "_").ToUpper();

                switch (code[0])
                {
                    case "SHIFT":
                        {
                            output.Add((byte)MajoraControlCode.SHIFT);
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "DELAY":
                    case "DELAY_NEWBOX":
                    case "DELAY_END":
                    case "FADE":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(MajoraControlCode), code[0]));
                            byte[] fadeAmountBytes = BitConverter.GetBytes(Convert.ToInt16(code[1]));
                            output.Add(fadeAmountBytes[1]);
                            output.Add(fadeAmountBytes[0]);
                            break;
                        }
                    case "SOUND":
                        {
                            output.Add((byte)MajoraControlCode.SOUND);
                            short soundValue = 0;

                            if (Dicts.SFXes.ContainsKey(code[1]))
                                soundValue = (short)Dicts.SFXes[code[1]];
                            else
                            {
                                try
                                {
                                    soundValue = ExtensionMethods.IsHex(code[1]) ? Convert.ToInt16(code[1].Substring(2), 16) : Convert.ToInt16(code[1]);
                                }
                                catch (Exception)
                                {
                                    errors.Add($"{code[1]} is not a valid sound.");
                                    soundValue = 0;
                                }
                            }

                            byte[] soundIDBytes = BitConverter.GetBytes(soundValue);
                            output.Add(soundIDBytes[1]);
                            output.Add(soundIDBytes[0]);

                            break;
                        }
                    default:
                        {
                            if (Enum.IsDefined(typeof(MajoraMsgColor), code[0]))
                                output.Add((byte)(int)Enum.Parse(typeof(MajoraMsgColor), code[0]));
                            else if (Enum.IsDefined(typeof(MajoraControlCode), code[0]))
                                output.Add((byte)(int)Enum.Parse(typeof(MajoraControlCode), code[0]));
                            else
                                errors.Add($"{code[0]} is not a valid control code.");

                            break;
                        }
                }
            }
            catch (Exception)
            {
            }

            return output;
        }

    private List<byte> GetControlCode(string[] code, ref List<string> errors)
        {
            List<byte> output = new List<byte>();

            try
            {
                for (int i = 0; i < code.Length; i++)
                    code[i] = code[i].Replace(" ", "_").ToUpper();

                switch (code[0])
                {
                    case "PIXELS_RIGHT":
                        {
                            output.Add((byte)OcarinaControlCode.SHIFT);
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "JUMP":
                        {
                            output.Add((byte)OcarinaControlCode.JUMP);
                            byte[] jumpIDBytes = BitConverter.GetBytes(short.Parse(code[1], System.Globalization.NumberStyles.HexNumber));
                            output.Add(jumpIDBytes[1]);
                            output.Add(jumpIDBytes[0]);
                            break;
                        }
                    case "DELAY":
                    case "FADE":
                    case "SHIFT":
                    case "SPEED":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(OcarinaControlCode), code[0]));
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "FADE2":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(OcarinaControlCode), code[0]));
                            byte[] fadeAmountBytes = BitConverter.GetBytes(Convert.ToInt16(code[1]));
                            output.Add(fadeAmountBytes[1]);
                            output.Add(fadeAmountBytes[0]);
                            break;
                        }
                    case "ICON":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(OcarinaControlCode), code[0]));
                            output.Add((byte)(int)Enum.Parse(typeof(OcarinaIcon), code[1]));
                            break;
                        }
                    case "BACKGROUND":
                        {
                            output.Add((byte)OcarinaControlCode.BACKGROUND);
                            byte[] backgroundIDBytes = BitConverter.GetBytes(Convert.ToInt32(code[1]));
                            output.Add(backgroundIDBytes[2]);
                            output.Add(backgroundIDBytes[1]);
                            output.Add(backgroundIDBytes[0]);
                            break;
                        }
                    case "HIGH_SCORE":
                        {
                            output.Add((byte)OcarinaControlCode.HIGH_SCORE);
                            output.Add((byte)(int)Enum.Parse(typeof(OcarinaHighScore), code[1]));
                            break;
                        }
                    case "SOUND":
                        {
                            output.Add((byte)OcarinaControlCode.SOUND);
                            short soundValue = 0;

                            if (Dicts.SFXes.ContainsKey(code[1]))
                                soundValue = (short)Dicts.SFXes[code[1]];
                            else
                            {
                                try
                                {
                                    soundValue = ExtensionMethods.IsHex(code[1]) ? Convert.ToInt16(code[1].Substring(2), 16) : Convert.ToInt16(code[1]);
                                }
                                catch (Exception)
                                {
                                    errors.Add($"{code[1]} is not a valid sound.");
                                    soundValue = 0;
                                }
                            }

                            byte[] soundIDBytes = BitConverter.GetBytes(soundValue);
                            output.Add(soundIDBytes[1]);
                            output.Add(soundIDBytes[0]);

                            break;
                        }
                    default:
                        {
                            if (Enum.IsDefined(typeof(OcarinaMsgColor), code[0]))
                            {
                                output.Add((byte)OcarinaControlCode.COLOR);
                                output.Add((byte)(int)Enum.Parse(typeof(OcarinaMsgColor), code[0]));
                            }
                            else if (Enum.IsDefined(typeof(OcarinaControlCode), code[0]))
                                output.Add((byte)(int)Enum.Parse(typeof(OcarinaControlCode), code[0]));
                            else
                                errors.Add($"{code[0]} is not a valid control code.");

                            break;
                        }
                }
            }
            catch (Exception)
            {
            }

            return output;
        }
    }


}
