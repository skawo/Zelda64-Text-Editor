using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcarinaTextEditor.Enums;
using GameFormatReader.Common;

namespace OcarinaTextEditor
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

        public Message()
        {
            TextData = "";
        }

        public Message(EndianBinaryReader reader, TableRecord mesgTableRecord)
        {
            MessageID = mesgTableRecord.MessageID;
            BoxType = mesgTableRecord.BoxType;
            BoxPosition = mesgTableRecord.BoxPosition;

            GetStringData(reader);
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

        private void GetStringData(EndianBinaryReader reader)
        {
            List<char> charData = new List<char>();

            byte testByte = reader.ReadByte();

            while (testByte != (byte)ControlCode.END)
            {
                bool readControlCode = false;

                if (testByte < 0x7F || testByte > 0x9E)
                {
                    if (Enum.IsDefined(typeof(ControlCode), (int)testByte))
                    {
                        charData.AddRange(GetControlCode((ControlCode)testByte, reader));
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
                       charData.Add(Enum.GetName(typeof(ControlCode), testByte).First());
                    }
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                    testByte = reader.ReadByte();
                else
                    testByte = 0x02;
            }

            TextData = new String(charData.ToArray());
        }

        private char[] GetControlCode(ControlCode code, EndianBinaryReader reader)
        {
            List<char> codeBank = new List<char>();
            string codeInsides = "";

            switch (code)
            {
                case ControlCode.COLOR:
                    Color col = (Color)reader.ReadByte();
                    codeInsides = col.ToString();
                    break;
                case ControlCode.ICON:
                    int iconID = (int)reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.ICON.ToString(), Enum.IsDefined(typeof(MsgIcon), iconID) ? ((MsgIcon)iconID).ToString() : iconID.ToString());
                    break;
                case ControlCode.LINE_BREAK:
                    return "\n".ToCharArray();
                case ControlCode.SHIFT:
                    byte numSpaces = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.SHIFT.ToString(), numSpaces);
                    break;
                case ControlCode.DELAY:
                    byte numFrames = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.DELAY.ToString(), numFrames);
                    break;
                case ControlCode.FADE:
                    byte numFramesFade = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.FADE.ToString(), numFramesFade);
                    break;
                case ControlCode.FADE2:
                    short numFramesFade2 = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", ControlCode.FADE2.ToString(), numFramesFade2);
                    break;
                case ControlCode.SOUND:
                    short soundID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1}", ControlCode.SOUND.ToString(), Dicts.SFXes.ContainsValue(soundID) ? Dicts.SFXes.FirstOrDefault(x => x.Value == soundID).Key : soundID.ToString());
                    break;
                case ControlCode.SPEED:
                    byte speed = reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.SPEED.ToString(), speed);
                    break;
                case ControlCode.HIGH_SCORE:
                    int scoreID = (int)reader.ReadByte();
                    codeInsides = string.Format("{0}:{1}", ControlCode.HIGH_SCORE.ToString().Replace("_", " "), Enum.IsDefined(typeof(HighScore), scoreID) ? ((HighScore)scoreID).ToString() : scoreID.ToString());
                    break;
                case ControlCode.JUMP:
                    short msgID = reader.ReadInt16();
                    codeInsides = string.Format("{0}:{1:X4}", ControlCode.JUMP.ToString(), msgID);
                    break;
                case ControlCode.NEW_BOX:
                    return ($"{Environment.NewLine}<{ControlCode.NEW_BOX.ToString().Replace("_", " ")}>{Environment.NewLine}").ToCharArray();
                case ControlCode.NS:
                    codeInsides = ControlCode.NS.ToString();
                    break;
                case ControlCode.DI:
                    codeInsides = ControlCode.DI.ToString();
                    break;
                case ControlCode.DC:
                    codeInsides = ControlCode.DC.ToString();
                    break;
                case ControlCode.BACKGROUND:
                    int backgroundID;
                    byte id1 = reader.ReadByte();
                    byte id2 = reader.ReadByte();
                    byte id3 = reader.ReadByte();
                    backgroundID = BitConverter.ToInt32(new byte[] { id3, id2, id1, 0 }, 0 );
                    codeInsides = string.Format("{0}:{1}", ControlCode.BACKGROUND.ToString(), backgroundID);
                    break;

                default:
                    codeInsides = code.ToString().Replace("_", " ");
                    break;
            }

            codeBank.AddRange(string.Format("<{0}>", codeInsides).ToCharArray());

            return codeBank.ToArray();
        }

        public void WriteMessage(EndianBinaryWriter writer)
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

        public List<byte> ConvertTextData(bool ShowErrors = true)
        {
            List<byte> data = new List<byte>();
            List<string> errors = new List<string>();

            for (int i = 0; i < TextData.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (TextData[i] != '<' && TextData[i] != '>')
                {
                    if (Enum.IsDefined(typeof(ControlCode), TextData[i].ToString()))
                    {
                        ControlCode Result;
                        Enum.TryParse(TextData[i].ToString(), out Result);
                        data.Add((byte)Result);
                    }
                    else if (TextData[i] == '\n')
                        data.Add((byte)ControlCode.LINE_BREAK);
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

                    if (parsedFixed == ControlCode.NEW_BOX.ToString() || parsedFixed == ControlCode.DELAY.ToString())
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

            data.Add((byte)ControlCode.END);

            if (ShowErrors && errors.Count != 0)
                System.Windows.Forms.MessageBox.Show($"Errors parsing message {MessageID}: " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

            if (errors.Count == 0)
                return data;
            else
                return new List<byte>();
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
                            output.Add((byte)ControlCode.SHIFT);
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "JUMP":
                        {
                            output.Add((byte)ControlCode.JUMP);
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
                            output.Add((byte)(int)Enum.Parse(typeof(ControlCode), code[0]));
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "FADE2":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(ControlCode), code[0]));
                            byte[] fadeAmountBytes = BitConverter.GetBytes(Convert.ToInt16(code[1]));
                            output.Add(fadeAmountBytes[1]);
                            output.Add(fadeAmountBytes[0]);
                            break;
                        }
                    case "ICON":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(ControlCode), code[0]));
                            output.Add((byte)(int)Enum.Parse(typeof(MsgIcon), code[1]));
                            break;
                        }
                    case "BACKGROUND":
                        {
                            output.Add((byte)ControlCode.BACKGROUND);
                            byte[] backgroundIDBytes = BitConverter.GetBytes(Convert.ToInt32(code[1]));
                            output.Add(backgroundIDBytes[2]);
                            output.Add(backgroundIDBytes[1]);
                            output.Add(backgroundIDBytes[0]);
                            break;
                        }
                    case "HIGH_SCORE":
                        {
                            output.Add((byte)ControlCode.HIGH_SCORE);
                            output.Add((byte)(int)Enum.Parse(typeof(MsgHighScore), code[1]));
                            break;
                        }
                    case "SOUND":
                        {
                            output.Add((byte)ControlCode.SOUND);
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
                            if (Enum.IsDefined(typeof(MsgColor), code[0]))
                            {
                                output.Add((byte)ControlCode.COLOR);
                                output.Add((byte)(int)Enum.Parse(typeof(MsgColor), code[0]));
                            }
                            else if (Enum.IsDefined(typeof(ControlCode), code[0]))
                                output.Add((byte)(int)Enum.Parse(typeof(ControlCode), code[0]));
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
