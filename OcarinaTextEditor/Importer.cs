using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using System.IO;
using Zelda64TextEditor.Enums;
using System.Windows;

namespace Zelda64TextEditor
{
    class Importer
    {
        private List<short> lBomberMsgs;
        private ObservableCollection<Message> m_messageList;
        private readonly MemoryStream m_inputFile;

        public Importer()
        {
            m_messageList = new ObservableCollection<Message>();
        }

        public Importer(string fileName, Enums.EditorMode Mode, ROMVer ROMVersion, bool Credits = false)
        {
            List<TableRecord> tableRecordList = new List<TableRecord>();

            long offset = ROMInfo.ZZRPCodeFileTablePostion;
            long msgOffset = 0;
            long mmBombersOffset = 0;

            if (Mode != EditorMode.ZZRPMode)
            {
                offset = ROMInfo.GetTableOffset(ROMVersion, Credits);
                msgOffset = ROMInfo.GetMessagesOffset(ROMVersion, Credits);
                mmBombersOffset = ROMInfo.GetBomberNotebookOffset(ROMVersion);
            }

            string zzrpFolder = Path.GetDirectoryName(fileName);
            string codeFilePath = Path.Combine(zzrpFolder, "system", "code");
            string msgDataPath = Path.Combine(zzrpFolder, "misc", "nes_message_data_static");

            try
            {
                using (FileStream stream = new FileStream(Mode == EditorMode.ZZRPMode ? codeFilePath : fileName, FileMode.Open, FileAccess.Read))
                {
                    m_inputFile = new MemoryStream();
                    stream.CopyTo(m_inputFile);

                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);
                    reader.BaseStream.Seek(offset, 0);
              
                    //Read in message table records
                    while (reader.PeekReadInt16() != -1)
                    {
                        TableRecord mesRecord = new TableRecord(reader);

                        if (!Properties.Settings.Default.IgnoreDuplicatedMsg && tableRecordList.Find(x => x.MessageID == mesRecord.MessageID) != null)
                            throw new Exception("Duplicate message entry.");

                        tableRecordList.Add(mesRecord);

                        
                    }
                }

                using (FileStream stream = new FileStream(Mode == EditorMode.ZZRPMode ? msgDataPath : fileName, FileMode.Open, FileAccess.Read))
                {
                    m_messageList = new ObservableCollection<Message>();
                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                    foreach (var mesgRecord in tableRecordList)
                    {
                        reader.BaseStream.Position = msgOffset + mesgRecord.Offset;

                        long savedPos = reader.BaseStream.Position;

                        Message mes = new Message(reader, mesgRecord, Credits, ROMVersion);

                        long byteSz = reader.BaseStream.Position - savedPos;

                        if (byteSz > Properties.Settings.Default.MsgMaxSize)
                            throw new Exception("Entry exceeded maximum message size.");

                        if (App.charMap != null)
                            mes.TextData = Converters.CharMapTextConverter.RemapTextTo(mes.TextData);
    
                        m_messageList.Add(mes);
                    }
                }

                if (ROMInfo.IsMajoraMask(ROMVersion) && mmBombersOffset != 0)
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        lBomberMsgs = new List<short>();
                        EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                        reader.BaseStream.Position = mmBombersOffset;

                        while (true)
                        {
                            short MsgID = reader.ReadInt16();

                            if (MsgID == 0)
                                break;
                            else
                                lBomberMsgs.Add(MsgID);
                        }
                    }
                }
                else
                    lBomberMsgs = new List<short>();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (Exception exz)
            {
                MessageBox.Show($"Failed loading messages: {exz.Message} Are you sure your ROM is decompressed?");
                return;
            }
        }


        public Importer(string tableFileName, string messageDataFileName, ROMVer Version, bool Credits = false)
        {
            m_messageList = new ObservableCollection<Message>();

            List<TableRecord> tableRecordList = new List<TableRecord>();

            try
            {

                //Read in message table records
                using (FileStream stream = new FileStream(tableFileName, FileMode.Open))
                {
                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                    while (reader.BaseStream.Position != reader.BaseStream.Length && reader.PeekReadInt16() != -1)
                    {
                        TableRecord mesRecord = new TableRecord(reader);

                        if (!Properties.Settings.Default.IgnoreDuplicatedMsg && tableRecordList.Find(x => x.MessageID == mesRecord.MessageID) != null)
                            throw new Exception("Duplicate message entry.");

                        tableRecordList.Add(mesRecord);
                    }
                }

                //Read in message data
                using (FileStream stream = new FileStream(messageDataFileName, FileMode.Open))
                {
                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                    foreach (var mesgRecord in tableRecordList)
                    {
                        if (mesgRecord.Offset >= reader.BaseStream.Length)
                            continue;

                        reader.BaseStream.Position = mesgRecord.Offset;
                        Message mes = new Message(reader, mesgRecord, Credits, Version);

                        long byteSz = reader.BaseStream.Position - mesgRecord.Offset;

                        if (byteSz > Properties.Settings.Default.MsgMaxSize)
                            throw new Exception("Entry exceeded maximum message size.");

                        if (App.charMap != null)
                            mes.TextData = Converters.CharMapTextConverter.RemapTextTo(mes.TextData);

                        m_messageList.Add(mes);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (Exception exz)
            {
                MessageBox.Show($"Failed loading messages: {exz.Message} Are you sure you chose the right game?");
                return;
            }
        }

        public ObservableCollection<Message> GetMessageList()
        {
            return m_messageList;
        }

        public List<short> GetBomberMsgsList()
        {
            return lBomberMsgs ?? new List<short>();
        }

        public MemoryStream GetInputFile()
        {
            return m_inputFile;
        }
    }
}
