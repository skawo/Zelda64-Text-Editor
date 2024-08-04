using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zelda64TextEditor;
using Zelda64TextEditor.Enums;
using GameFormatReader.Common;
using System.Windows;

namespace Zelda64TextEditor
{
    internal class Exporter
    {
        private readonly string m_fileName;

        public Exporter()
        {

        }

        public Exporter(ObservableCollection<Message> messageList, string path, ExportType exportType, 
                                            MemoryStream inputFile, ROMVer Version, bool Credits = false,
                                            string tablePath = null, string messagePath = null)
        {
            m_fileName = path;

            // We need the char table, with an index of -4, at the start of all the entries. So we'll find it and put it at the top.
            for (int i = 0; i < messageList.Count; i++)
            {
                if (ROMInfo.ROMTSFixer.ContainsKey(Version) && messageList[i].MessageID == -4)
                {
                    // Message is already at the start, we do nothing here
                    if (i == 0)
                        break;

                    Message charTable = messageList[i]; // Copy char table to buffer
                    messageList.Insert(0, charTable); // Insert at the top of the list
                    messageList.RemoveAt(i + 1); // Delete the original message
                }
            }

            List<byte> stringBank = new List<byte>();

            using (MemoryStream messageTableStream = new MemoryStream())
            {
                EndianBinaryWriter messageTableWriter = new EndianBinaryWriter(messageTableStream, Endian.Big);

                foreach (Message mes in messageList)
                {
                    Message mesN = Message.MakeCopy(mes);

                    if (App.charMap != null)
                        mesN.TextData = Converters.CharMapTextConverter.RemapTextFrom(mesN.TextData);

                    mesN.WriteMessage(messageTableWriter, Version, Credits);

                    messageTableWriter.BaseStream.Seek(-4, SeekOrigin.Current);

                    int stringOffset = stringBank.Count();

                    byte[] decompOffset = BitConverter.GetBytes(stringOffset);
                    decompOffset[3] = (byte)(ROMInfo.UseSeg8(Version) ? 0x08 : 0x07);

                    for (int i = 3; i > -1; i--)
                        messageTableWriter.Write(decompOffset[i]);


                    List<byte> msgB = mesN.ConvertTextData(Version, Credits, true);

                    if (msgB.Count > Properties.Settings.Default.MsgMaxSize)
                    {
                        MessageBox.Show($"Message ID 0x{mesN.MessageID.ToString("X")} exceeds maximum message size and will not be saved.");
                        Message msgBlank = new Message("", OcarinaTextboxType.Black);
                        msgB = msgBlank.ConvertTextData(Version, Credits, true);
                    }

                    stringBank.AddRange(msgB);
                    ExtensionMethods.PadByteList4(stringBank);
                }


                // Write end-of-list message
                messageTableWriter.Write((short)-1);
                messageTableWriter.Write((short)0);
                messageTableWriter.Write((int)0);

                messageTableStream.Position = 0;

                using (MemoryStream stringData = new MemoryStream())
                {
                    EndianBinaryWriter stringWriter = new EndianBinaryWriter(stringData, Endian.Big);

                    ExtensionMethods.PadByteList16(stringBank);
                    stringWriter.Write(stringBank.ToArray());

                    stringData.Position = 0;

                    switch (exportType)
                    {
                        case ExportType.File:
                            ExportToFile((MemoryStream)messageTableWriter.BaseStream, (MemoryStream)stringWriter.BaseStream, tablePath, messagePath);
                            break;
                        case ExportType.OriginalROM:
                            ExportToOriginalROM(messageTableStream, stringData, Version, Credits);
                            break;
                        case ExportType.NewROM:
                            ExportToNewRom(messageTableStream, stringData, inputFile, Version, Credits);
                            break;
                        case ExportType.ZZRP:
                            ExportToZZRP(messageTableStream, stringData);
                            break;
                        case ExportType.ZZRPL:
                            ExportToZZRPL(messageTableStream, stringData);
                            break;
                        case ExportType.Z64ROM:
                            ExportToZ64ROM(messageTableStream, stringData, Credits);
                            break;
                    }
                }
            }
        }

        private bool CheckIfPossibleToInsertToROM(MemoryStream table, MemoryStream stringBank, ROMVer Version, bool Credits = false)
        {
            int StringBankLimit = ROMInfo.GetMessagesMaxSize(Version, Credits);
            int TableLimit = ROMInfo.GetTableMaxSize(Version, Credits);

            if (table.Length > TableLimit || stringBank.Length > StringBankLimit)
            {
                MessageBox.Show($"Message data exceeds size possible to insert into ROM.\nMessage table: {table.Length}/{TableLimit}\nMessage data:{stringBank.Length}/{StringBankLimit}");
                return false;
            }

            return true;
        }

        private void ExportToOriginalROM(MemoryStream table, MemoryStream stringBank, ROMVer Version, bool Credits = false)
        {
            try
            {
                if (!CheckIfPossibleToInsertToROM(table, stringBank, Version, Credits))
                    return;

                using (FileStream romFile = new FileStream(m_fileName, FileMode.Open))
                {
                    ExportToROMInternal(romFile, table, stringBank, Version, Credits);
                }
            }

            catch (IOException)
            {
                MessageBox.Show("The ROM you are trying to save to is open in another program. Please close that program and try to save it again.", "ROM is In Use");
                return;
            }
        }

        private void ExportToNewRom(MemoryStream table, MemoryStream stringBank, MemoryStream inputFile, ROMVer Version, bool Credits = false)
        {
            try
            {
                if (!CheckIfPossibleToInsertToROM(table, stringBank, Version, Credits))
                    return;

                using (FileStream romFile = new FileStream(m_fileName, FileMode.Create, FileAccess.Write))
                {
                    inputFile.Position = 0;
                    inputFile.CopyTo(romFile);
                    romFile.Position = 0;
                    ExportToROMInternal(romFile, table, stringBank, Version, Credits);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("The ROM you are trying to save to is open in another program. Please close that program and try to save it again.", "ROM is In Use");
                return;
            }
        }

        private void ExportToROMInternal(FileStream romFile, MemoryStream table, MemoryStream stringBank, ROMVer Version, bool Credits = false)
        {
            EndianBinaryWriter writer = new EndianBinaryWriter(romFile, Endian.Big);

            romFile.Position = ROMInfo.GetTableOffset(Version, Credits);

            table.CopyTo(romFile);

            romFile.Position = ROMInfo.GetMessagesOffset(Version, Credits);

            stringBank.CopyTo(romFile);


            // Since OoT uses a character table for the title screen, the file select, and Link's name,
            // And we might move this table's offset, we're going to hack the game a bit.
            // What we'll do is make the char table the first message (like we did in the constructor above)
            // And change the code so that it gets a start address of 0x07000000
            // And an end address of 0x07000048.
            /*
                romFile.Position = 0xAE60B6; // Set position to start address LUI lower half
                writer.Write((short)0x0700); // Overwrite 0x0704 with 0x0700
                romFile.Position = 0xAE60BA; // Set position to start address ADDIU lower half
                writer.Write((short)0); // Overwite 0x80D4 with 0

                romFile.Position = 0xAE60C6; // Set position to LUI lower half
                writer.Write((short)0x0700); // Overwite 0x0704 with 0x0700
                romFile.Position = 0xAE60F2; // Set position to end address ADDIU lower half
                writer.Write((short)0x48); // Overwrite 0x811C with 0x0048
            */

            if (ROMInfo.ROMTSFixer.ContainsKey(Version) && !Credits)
            {
                List<int> Data = ROMInfo.ROMTSFixer[Version];

                for (int i = 0; i < Data.Count; i+= 2)
                {
                    romFile.Position = Data[i];
                    writer.Write(Convert.ToInt16(Data[i + 1]));
                }
            }
        }

        private void ExportToFile(MemoryStream table, MemoryStream stringBank, string TablePath = null, string MsgDataPath = null)
        {
            if (TablePath == null)
                TablePath = string.Format(@"{0}\{1}", m_fileName, Properties.Settings.Default.TableFileName);

            if (MsgDataPath == null)
                MsgDataPath = string.Format(@"{0}\{1}", m_fileName, Properties.Settings.Default.MessagesFileName);

            try
            {
                File.Delete(TablePath);

                using (FileStream tableFile = new FileStream(TablePath, FileMode.Create, FileAccess.Write))
                {
                    tableFile.Position = 0;
                    table.WriteTo(tableFile);
                }

                File.Delete(MsgDataPath);

                using (FileStream msgFile = new FileStream(MsgDataPath, FileMode.Create, FileAccess.Write))
                {
                    msgFile.Position = 0;
                    stringBank.WriteTo(msgFile);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ExportToZ64ROM(MemoryStream table, MemoryStream stringBank, bool CreditsMode)
        {
            string cfgFolder = Path.GetDirectoryName(m_fileName);
            string staticFolder = Path.Combine(cfgFolder, "rom", "system", "static");

            string msgDataFn = CreditsMode ? "message_data_static_staff.bin" : "message_data_static_NES.bin";
            string tableFn = CreditsMode ? "message_data_static_staff.tbl" : "message_data_static_NES.tbl";

            string msgDataPath = Path.Combine(staticFolder, msgDataFn);
            string tablePath = Path.Combine(staticFolder, tableFn);

            ExportToFile(table, stringBank, tablePath, msgDataPath);
        }

        private void ExportToZZRPL(MemoryStream table, MemoryStream stringBank)
        {
            string zzrplFolder = Path.GetDirectoryName(m_fileName);
            string tablePath = Path.Combine(zzrplFolder, "messages", "MessageTable.tbl");
            string msgDataPath = Path.Combine(zzrplFolder, "messages", "StringData.bin");

            ExportToFile(table, stringBank, tablePath, msgDataPath);
        }

        private void ExportToZZRP(MemoryStream table, MemoryStream stringBank)
        {
            string zzrpFolder = Path.GetDirectoryName(m_fileName);
            string codeFilePath = Path.Combine(zzrpFolder, "system", "code");
            string msgDataPath = Path.Combine(zzrpFolder, "misc", "nes_message_data_static");

            try
            {
                using (FileStream codeFile = new FileStream(codeFilePath, FileMode.Open))
                {
                    codeFile.Position = 0;
                    EndianBinaryWriter writer = new EndianBinaryWriter(codeFile, Endian.Big);

                    codeFile.Position = ROMInfo.ZZRPCodeFileTablePostion;
                    table.CopyTo(codeFile);
                    // Since OoT uses a character table for the title screen, the file select, and Link's name,
                    // And we might move this table's offset, we're going to hack the game a bit.
                    // What we'll do is make the char table the first message (like we did in the constructor above)
                    // And change the code so that it gets a start address of 0x07000000
                    // And an end address of 0x07000048.

                    codeFile.Position = 0x520B6; // Set position to start address LUI lower half
                    writer.Write((short)0x0700); // Overwrite 0x0704 with 0x0700
                    codeFile.Position = 0x520BA; // Set position to start address ADDIU lower half
                    writer.Write((short)0); // Overwite 0x80D4 with 0

                    codeFile.Position = 0x520C6; // Set position to LUI lower half
                    writer.Write((short)0x0700); // Overwite 0x0704 with 0x0700
                    codeFile.Position = 0x520F2; // Set position to end address ADDIU lower half
                    writer.Write((short)0x48); // Overwrite 0x811C with 0x0048 */
        }

        File.Delete(msgDataPath);

                using (FileStream msgFile = new FileStream(msgDataPath, FileMode.Create, FileAccess.Write))
                {
                    msgFile.Position = 0;
                    stringBank.WriteTo(msgFile);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
