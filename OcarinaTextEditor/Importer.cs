using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using System.IO;
using OcarinaTextEditor.Enums;
using System.Windows;

namespace OcarinaTextEditor
{
    class Importer
    {
        private ObservableCollection<Message> m_messageList;
        private MemoryStream m_inputFile;

        public Importer()
        {
            m_messageList = new ObservableCollection<Message>();
        }

        public Importer(string fileName, Dictionary<ControlCode, string> controlCodeDict, bool ZZRP, bool Debug)
        {
            List<TableRecord> tableRecordList = new List<TableRecord>();

            string zzrpFolder = "";
            string codeFilePath = "";
            string msgDataPath = "";

            if (ZZRP)
            {
                zzrpFolder = Path.GetDirectoryName(fileName);
                codeFilePath = Path.Combine(zzrpFolder, "system", "code");
                msgDataPath = Path.Combine(zzrpFolder, "misc", "nes_message_data_static");
            }

            long offset = ZZRP ? 0x0012E4C0 : Debug ? 0x00BC24C0 : 0x00B849EC;
            long msgOffset = ZZRP ? 0 : Debug ? 0x8C6000 : 0x92D000;

            try
            {
                using (FileStream stream = new FileStream(ZZRP ? codeFilePath : fileName, FileMode.Open, FileAccess.Read))
                {
                    m_inputFile = new MemoryStream();
                    stream.CopyTo(m_inputFile);

                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);
                    reader.BaseStream.Seek(offset, 0);

                    //Read in message table records
                    while (reader.PeekReadInt16() != -1)
                    {
                        TableRecord mesRecord = new TableRecord(reader);
                        tableRecordList.Add(mesRecord);
                    }
                }

                using (FileStream stream = new FileStream(ZZRP ? msgDataPath : fileName, FileMode.Open, FileAccess.Read))
                {
                    m_messageList = new ObservableCollection<Message>();
                    EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                    foreach (var mesgRecord in tableRecordList)
                    {
                        reader.BaseStream.Position = msgOffset + mesgRecord.Offset;
                        Message mes = new Message(reader, mesgRecord, controlCodeDict);
                        m_messageList.Add(mes);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed loading messages. Note: ROMs built by zzromtool are not supported directly!");
                return;
            }
        }

        public Importer(string tableFileName, string messageDataFileName, Dictionary<ControlCode, string> controlCodeDict)
        {
            m_messageList = new ObservableCollection<Message>();

            List<TableRecord> tableRecordList = new List<TableRecord>();

            //Read in message table records
            using (FileStream stream = new FileStream(tableFileName, FileMode.Open))
            {
                EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                while (reader.PeekReadInt16() != -1)
                {
                    TableRecord mesRecord = new TableRecord(reader);
                    tableRecordList.Add(mesRecord);
                }
            }

            //Read in message data
            using (FileStream stream = new FileStream(messageDataFileName, FileMode.Open))
            {
                EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                foreach (var mesgRecord in tableRecordList)
                {
                    reader.BaseStream.Position = mesgRecord.Offset;
                    Message mes = new Message(reader, mesgRecord, controlCodeDict);

                    m_messageList.Add(mes);
                }
            }
        }

        public ObservableCollection<Message> GetMessageList()
        {
            return m_messageList;
        }

        public MemoryStream GetInputFile()
        {
            return m_inputFile;
        }
    }
}
