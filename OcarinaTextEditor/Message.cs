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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
        public TextboxType BoxType
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
        private TextboxType m_boxType;
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


        public Message()
        {
            TextData = "";
        }

        public Message(EndianBinaryReader reader, TableRecord mesgTableRecord, bool Credits, ROMVer Version = ROMVer.Unknown)
        {
            if (ROMInfo.IsMajoraMask(Version) && !Credits)
            {
                MessageID = mesgTableRecord.MessageID;

                MajoraBoxType = (MajoraTextboxType)reader.ReadByte();
                BoxPosition = (TextboxPosition)reader.ReadByte();
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

        public Message(string Message, TextboxType t)
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

                if (testByte < 0x7F || testByte > 0x9E)
                {
                    if (Enum.IsDefined(typeof(MajoraControlCode), (int)testByte))
                    {
                        charData.AddRange(GetMajoraControlCode((MajoraControlCode)testByte, reader));
                        readControlCode = true;
                    }
                }

                if (!readControlCode)
                {
                    if ((testByte >= 0x20 && testByte < 0x7F) || (char.IsLetterOrDigit((char)testByte) || char.IsWhiteSpace((char)testByte) || char.IsPunctuation((char)testByte)))
                    {
                        charData.Add((char)testByte);
                    }
                    else if (testByte == 0x7F)
                    {
                        // Never actually used in-game. Appears blank.
                        charData.Add(' ');
                    }
                    else if (testByte >= 0x80 && testByte <= 0x9E)
                    {
                        charData.Add(Enum.GetName(typeof(MajoraControlCode), testByte).First());
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
                    if ((testByte >= 0x20 && testByte < 0x7F) || (char.IsLetterOrDigit((char)testByte) || char.IsWhiteSpace((char)testByte) || char.IsPunctuation((char)testByte)))
                    {
                        charData.Add((char)testByte);
                    }
                    else if (testByte == 0x7F)
                    {
                        // Never actually used in-game. Appears blank.
                        charData.Add(' ');
                    }    
                    else if (testByte >= 0x80 && testByte <= 0x9E)
                    { 
                       charData.Add(Enum.GetName(typeof(OcarinaControlCode), testByte).First());
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
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SOUND.ToString(), Dicts.SFXes.ContainsValue(soundID) ? Dicts.SFXes.FirstOrDefault(x => x.Value == soundID).Key : soundID.ToString());
                    break;
                case OcarinaControlCode.SPEED:
                    byte speed = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SPEED.ToString(), speed);
                    break;
                case OcarinaControlCode.HIGH_SCORE:
                    int scoreID = (int)reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.HIGH_SCORE.ToString().Replace("_", " "), Enum.IsDefined(typeof(HighScore), scoreID) ? ((HighScore)scoreID).ToString() : scoreID.ToString());
                    break;
                case OcarinaControlCode.JUMP:
                    short msgID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1:X4}", OcarinaControlCode.JUMP.ToString(), msgID);
                    break;
                case OcarinaControlCode.NEW_BOX:
                    return ($"{Environment.NewLine}<{OcarinaControlCode.NEW_BOX.ToString().Replace("_", " ")}>{Environment.NewLine}").ToCharArray();
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
                    codeInsides = code.ToString().Replace("_", " ");
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
                    return ($"{Environment.NewLine}<{MajoraControlCode.NEW_BOX.ToString().Replace("_", " ")}>{Environment.NewLine}").ToCharArray();
                case MajoraControlCode.NEW_BOX_INCOMPL:
                    return ($"{Environment.NewLine}<{MajoraControlCode.NEW_BOX_INCOMPL.ToString().Replace("_", " ")}>{Environment.NewLine}").ToCharArray();
                case MajoraControlCode.DELAY_DC:
                case MajoraControlCode.DELAY_DI:
                case MajoraControlCode.DELAY_END:
                case MajoraControlCode.FADE:
                    short delay = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", code.ToString(), delay);
                    break;
                case MajoraControlCode.SOUND:
                    short soundID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", OcarinaControlCode.SOUND.ToString(), soundID.ToString());
                    break;
                default:
                    codeInsides = code.ToString().Replace("_", " ");
                    break;
            }

            codeBank.AddRange(string.Format("<{0}>", codeInsides).ToCharArray());

            return codeBank.ToArray();
        }

        public void WriteMessage(EndianBinaryWriter writer, ROMVer Version)
        {
            if (ROMInfo.IsMajoraMask(Version))
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
                type = type << 4;
                type = type | pos;

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

        private List<byte> ConvertMajoraTextData(bool ShowErrors = true)
        {
            List<byte> data = new List<byte>();
            List<string> errors = new List<string>();

            data.Add((byte)this.MajoraBoxType);
            data.Add((byte)this.BoxPosition);
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
                    if (Enum.IsDefined(typeof(MajoraControlCode), TextData[i].ToString()))
                    {
                        MajoraControlCode Result;
                        Enum.TryParse(TextData[i].ToString(), out Result);
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

                    if (parsedFixed == MajoraControlCode.NEW_BOX.ToString() || parsedFixed == MajoraControlCode.DELAY_END.ToString() || parsedFixed == MajoraControlCode.NEW_BOX_INCOMPL.ToString())
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
                        OcarinaControlCode Result;
                        Enum.TryParse(TextData[i].ToString(), out Result);
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
                    case "DELAY_DC":
                    case "DELAY_DI":
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
                            short soundValue = Convert.ToInt16(code[1]);
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
                                    soundValue = Convert.ToInt16(code[1]);
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
