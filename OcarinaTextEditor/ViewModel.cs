using OcarinaTextEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using OcarinaTextEditor.Enums;
using System.IO;
using GameFormatReader.Common;

namespace OcarinaTextEditor
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged overhead
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region public List<Message> MessageList
        public ObservableCollection<Message> MessageList
        {
            get { return m_messageList; }
            set
            {
                if (value != m_messageList)
                {
                    m_messageList = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<Message> m_messageList;
        #endregion

        #region CreditsMode

        private Boolean _CreditsMode;
        public Boolean CreditsMode
        {
            get { return _CreditsMode; }
            set
            {
                _CreditsMode = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region ZZRPMode

        private Boolean _ZZRPMode;
        public Boolean ZZRPMode
        {
            get { return _ZZRPMode; }
            set
            {
                _ZZRPMode = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region ZZRPLMode

        private Boolean _ZZRPLMode;
        public Boolean ZZRPLMode
        {
            get { return _ZZRPLMode; }
            set
            {
                _ZZRPLMode = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Z64ROMMode

        private Boolean _Z64ROMMode;
        public Boolean Z64ROMMode
        {
            get { return _Z64ROMMode; }
            set
            {
                _Z64ROMMode = value;
                NotifyPropertyChanged();
            }
        }

        public string Path1 { get; set; }
        public string Path2 { get; set; }

        #endregion

        #region OldMode

        private Boolean _OldMode;
        public Boolean OldMode
        {
            get { return _OldMode; }
            set
            {
                _OldMode = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region public Message SelectedMessage
        public Message SelectedMessage
        {
            get { return m_selectedMessage; }
            set
            {
                if (value != m_selectedMessage)
                {
                    m_selectedMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private Message m_selectedMessage;
        #endregion

        #region public string WindowTitle
        public string WindowTitle
        {
            get { return m_windowTitle; }
            set
            {
                if (value != m_windowTitle)
                {
                    m_windowTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string m_windowTitle = "Ocarina of Time Text Editor";
        #endregion

        #region public CollectionViewSource ViewSource
        public CollectionViewSource ViewSource
        {
            get { return m_viewSource; }
            set
            {
                if (value != m_viewSource)
                {
                    m_viewSource = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private CollectionViewSource m_viewSource;
        #endregion

        #region public string SearchFilter
        public string SearchFilter
        {
            get { return m_searchFilter; }

            set
            {
                m_searchFilter = value;

                if (!string.IsNullOrEmpty(SearchFilter))
                    AddFilter();

                ViewSource.View.Refresh();

                NotifyPropertyChanged("SearchFilter");
            }
        }
        private string m_searchFilter;
        #endregion

        public Dictionary<ControlCode, string> m_controlCodes;

        ROMVer Version = ROMVer.Unknown;

        public int TextboxPosition;

        private MemoryStream m_inputFile;
        private string m_inputFileName;

        #region Command Callbacks
        public ICommand OnRequestOpenFile
        {
            get { return new RelayCommand(x => Open(), x => true); }
        }
        public ICommand OnRequestOpenData
        {
            get { return new RelayCommand(x => OpenData(), x => true); }
        }
        public ICommand OnRequestCloseFile
        {
            get { return new RelayCommand(x => Close(), x => MessageList != null); }
        }
        public ICommand OnRequestSaveFileNewROM
        {
            get { return new RelayCommand(x => SaveToNewRom(), x => MessageList != null); }
        }
        public ICommand OnRequestSaveFileOriginalROM
        {
            get { return new RelayCommand(x => SaveToOriginalRom(), x => MessageList != null); }
        }
        public ICommand OnRequestSaveFileFiles
        {
            get { return new RelayCommand(x => SaveToFiles(), x => MessageList != null); }
        }
        public ICommand OnRequestSaveFilePatch
        {
            get { return new RelayCommand(x => SaveToPatch(), x => MessageList != null); }
        }
        public ICommand OnRequestAddMessage
        {
            get { return new RelayCommand(x => AddMessage(), x => MessageList != null); }
        }
        public ICommand OnRequestRemoveMessage
        {
            get { return new RelayCommand(x => RemoveMessage(), x => MessageList != null); }
        }
        public ICommand OnRequestAddControl
        {
            get { return new RelayCommand(x => InsertControlCode((string)x), x => SelectedMessage != null); }
        }
        public ICommand OnRequestOpenZZRPL
        {
            get { return new RelayCommand(x => OpenZZRPL(), x => true); }
        }

        public ICommand OnRequestOpenZ64ROM
        {
            get { return new RelayCommand(x => OpenZ64ROM(), x => true); }
        }

        public ICommand OnRequestOpenZZRP
        {
            get { return new RelayCommand(x => OpenZZRP(), x => true); }
        }
        public ICommand OnRequestSaveZZRP
        {
            get { return new RelayCommand(x => SaveZZRP(), x => MessageList != null); }
        }

        public ICommand OnRequestSaveZZRPL
        {
            get { return new RelayCommand(x => SaveZZRPL(), x => MessageList != null); }
        }

        public ICommand OnRequestSaveZ64ROM
        {
            get { return new RelayCommand(x => SaveZ64ROM(), x => MessageList != null); }
        }
        #endregion

        public ICommand OnRequestRefresh
        {
            get { return new RelayCommand(x => Refresh(), x => MessageList != null); }
        }

        public ViewModel()
        {
            ViewSource = new CollectionViewSource();
        }

        private ROMVer CheckRomVersion(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                m_inputFile = new MemoryStream();
                stream.CopyTo(m_inputFile);

                EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Big);

                byte[] Buffer = new byte[8];
                reader.BaseStream.Seek(0x740C, 0);
                reader.Read(Buffer, 0, 8);
                string Date = Encoding.ASCII.GetString(Buffer);

                if (Date == "98-10-21")
                {
                    System.Windows.MessageBox.Show("Warning: 1.0 Support is largely untested!");
                    return ROMVer.N1_0;
                }
                else
                {
                    reader.BaseStream.Seek(0x12F50, 0);
                    reader.Read(Buffer, 0, 8);
                    Date = Encoding.ASCII.GetString(Buffer);

                    if (Date == "03-02-21")
                        return ROMVer.Debug;
                    else
                    {
                        System.Windows.MessageBox.Show("ROM unsupported; supply a clean uncompressed NTSC 1.0 ROM, or the Debug ROM.");
                        return ROMVer.Unknown;
                    }
                }
            }
        }

        #region Input/Output
        private void Open(string PathD = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "N64 ROMs (*.n64, *.z64)|*.n64;*.z64|All files|*";

            if (PathD != "" || openFile.ShowDialog() == true)
            {
                if (PathD != "")
                    openFile.FileName = PathD;

                Version = CheckRomVersion(openFile.FileName);

                if (Version == ROMVer.Unknown)
                    return;


                CreditsMode = Keyboard.IsKeyDown(Key.LeftCtrl);

                Importer file = new Importer(openFile.FileName, EditMode.ROM, Version == ROMVer.Debug, CreditsMode);
                MessageList = file.GetMessageList();

                // If message list is null, we failed to open a ROM
                if (MessageList == null)
                    return;

                Path1 = openFile.FileName;

                m_inputFileName = openFile.FileName;
                m_inputFile = file.GetInputFile();

                ViewSource.Source = MessageList;
                SelectedMessage = MessageList[0];

                WindowTitle = string.Format("{0} - Ocarina of Time Text Editor", openFile.FileName);

                OldMode = true;
                ZZRPMode = ZZRPLMode = Z64ROMMode = false;
            }
        }

        private void OpenZZRPL(string PathD = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "zzrtl Projects (*.zzrpl)|*.zzrpl";

            if (PathD != "" || openFile.ShowDialog() == true)
            {
                if (PathD != "")
                    openFile.FileName = PathD;

                string zzrplFolder = Path.GetDirectoryName(openFile.FileName);
                string msgDataEd = Path.Combine(zzrplFolder, "messages", "StringData.bin");
                string tableEd = Path.Combine(zzrplFolder, "messages", "MessageTable.tbl");

                if ((File.Exists(msgDataEd) && !File.Exists(tableEd)) || (!File.Exists(msgDataEd) && File.Exists(tableEd)))
                {
                    System.Windows.Forms.MessageBox.Show("Error: Partial edited data found. Stopping in order to avoid loss of work.");
                    return;
                }

                if (!File.Exists(msgDataEd) || !File.Exists(tableEd))
                {
                    string msgData = Path.Combine(zzrplFolder, "messages", "_vanilla-1.0", "StringData.bin");
                    string table = Path.Combine(zzrplFolder, "messages", "_vanilla-1.0", "MessageTable.tbl");
                    string msgDataDeb = Path.Combine(zzrplFolder, "messages", "_vanilla-debug", "StringData.bin");
                    string tableDeb = Path.Combine(zzrplFolder, "messages", "_vanilla-debug", "MessageTable.tbl");

                    if ((!File.Exists(msgData) || !File.Exists(table)) && (!File.Exists(msgDataDeb) || !File.Exists(tableDeb)))
                    {
                        System.Windows.Forms.MessageBox.Show("Not a ZZRTL-Audio filesystem.");
                        return;
                    }

                    Path1 = openFile.FileName;

                    if (File.Exists(msgData))
                    {
                        File.Copy(msgData, msgDataEd);
                        File.Copy(table, tableEd);
                    }
                    else if (File.Exists(msgDataDeb))
                    {
                        File.Copy(msgDataDeb, msgDataEd);
                        File.Copy(tableDeb, tableEd);
                    }
                }

                Path1 = openFile.FileName;
                Path2 = "";

                Importer file = new Importer(tableEd, msgDataEd);
                MessageList = file.GetMessageList();

                // If message list is null, we failed to parse.
                if (MessageList == null)
                    return;

                m_inputFileName = openFile.FileName;
                m_inputFile = file.GetInputFile();


                ViewSource.Source = MessageList;
                SelectedMessage = MessageList[0];


                WindowTitle = Path.GetFileNameWithoutExtension(openFile.FileName) + " - Ocarina of Time Text Editor";

                ZZRPMode = OldMode = Z64ROMMode = false;
                ZZRPLMode = true;
            }
        }

        private void OpenZ64ROM(string PathD = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "Z64ROM Config File (*.toml)|*.toml";

            if (PathD != "" || openFile.ShowDialog() == true)
            {
                if (PathD != "")
                    openFile.FileName = PathD;

                string cfgFolder = Path.GetDirectoryName(openFile.FileName);
                string[] cfg = File.ReadAllLines(openFile.FileName);
                string vanillaFolderName = ".vanilla";

                foreach (string s in cfg)
                {
                    string[] setting = s.Split('=');

                    if (setting.Length == 2 && setting[0] == "z_vanilla")
                        vanillaFolderName = setting[1];
                }

                string staticFolder = Path.Combine(cfgFolder, "rom", "system", "static");
                string msgDataEd = Path.Combine(staticFolder, "message_data_static_NES.bin");
                string tableEd = Path.Combine(staticFolder, "message_data_static_NES.tbl");

                if ((File.Exists(msgDataEd) && !File.Exists(tableEd)) || (!File.Exists(msgDataEd) && File.Exists(tableEd)))
                {
                    System.Windows.Forms.MessageBox.Show("Error: Partial edited data found. Stopping in order to avoid loss of work.");
                    return;
                }

                if (!File.Exists(msgDataEd) || !File.Exists(tableEd))
                {
                    string msgData = Path.Combine(staticFolder, vanillaFolderName, "message_data_static_NES.bin");
                    string table = Path.Combine(staticFolder, vanillaFolderName, "message_data_static_NES.tbl");

                    if (!File.Exists(msgData) || !File.Exists(table))
                    {
                        System.Windows.Forms.MessageBox.Show("Could not find the extracted files.");
                        return;
                    }
                    else
                    {
                        File.Copy(msgData, msgDataEd);
                        File.Copy(table, tableEd);
                    }
                }

                Importer file = new Importer(tableEd, msgDataEd);
                MessageList = file.GetMessageList();

                // If message list is null, we failed to parse.
                if (MessageList == null)
                    return;

                m_inputFileName = openFile.FileName;
                m_inputFile = file.GetInputFile();

                Path1 = openFile.FileName;
                Path2 = "";


                ViewSource.Source = MessageList;
                SelectedMessage = MessageList[0];

                WindowTitle = Path.GetFileNameWithoutExtension(openFile.FileName) + " - Ocarina of Time Text Editor";

                ZZRPMode = OldMode = ZZRPLMode = false;
                Z64ROMMode = true;
            }
        }

        private void OpenZZRP(string PathD = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "zzromtool Projects (*.zzrp)|*.zzrp";

            if (PathD != "" || openFile.ShowDialog() == true)
            {
                if (PathD != "")
                    openFile.FileName = PathD;

                string zzrpFolder = Path.GetDirectoryName(openFile.FileName);
                string codeFile = Path.Combine(zzrpFolder, "system", "code");
                string msgData = Path.Combine(zzrpFolder, "misc", "nes_message_data_static");

                if (!File.Exists(codeFile) || !File.Exists(msgData))
                {
                    System.Windows.Forms.MessageBox.Show("This doesn't look to be a zzromtool filesystem...");
                    return;
                }

                Importer file = new Importer(openFile.FileName, EditMode.ZZRT, Version == ROMVer.Debug);
                MessageList = file.GetMessageList();

                // If message list is null, we failed to parse.
                if (MessageList == null)
                    return;

                Path1 = openFile.FileName;
                Path2 = "";

                m_inputFileName = openFile.FileName;
                m_inputFile = file.GetInputFile();

                ViewSource.Source = MessageList;
                SelectedMessage = MessageList[0];

                WindowTitle = Path.GetFileNameWithoutExtension(openFile.FileName) + " - Ocarina of Time Text Editor";

                ZZRPMode = true;
                OldMode = ZZRPLMode = Z64ROMMode = false;
            }
        }


        private void OpenData(string PathD1 = "", string PathD2 = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();
            string tableFileName;
            string messageDataFileName;

            if (PathD1 == "" || PathD2 == "")
            {
                openFile.Filter = "Table Data (*.tbl)|*.tbl|All files|*";
                openFile.Title = "Select the MessageTable.tbl file";

                if (openFile.ShowDialog() != true)
                    return;

                tableFileName = openFile.FileName;

                openFile.Filter = "String Data (*.bin)|*.bin|All files|*";
                openFile.Title = "Select the StringData.bin file";
                openFile.FilterIndex = 0;

                if (openFile.ShowDialog() != true)
                    return;

                messageDataFileName = openFile.FileName;
            }
            else
            {
                tableFileName = PathD1;
                messageDataFileName = PathD2;
            }


            Importer file = new Importer(tableFileName, messageDataFileName);
            MessageList = file.GetMessageList();

            ViewSource.Source = MessageList;
            SelectedMessage = MessageList[0];

            WindowTitle = string.Format("{0} - Ocarina of Time Text Editor", tableFileName);

            OldMode = true;
            ZZRPMode = ZZRPLMode = Z64ROMMode = false;

            Path1 = tableFileName;
            Path2 = messageDataFileName;
        }

        private void SaveToNewRom()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "N64 ROMs (*.n64, *.z64)|*.n64;*.z64|All files|*";

            if (saveFile.ShowDialog() == true)
            {
                Exporter export = new Exporter(m_messageList, saveFile.FileName, Enums.ExportType.NewROM, m_inputFile, Version == ROMVer.Debug, CreditsMode);
                m_inputFileName = saveFile.FileName;
                WindowTitle = string.Format("{0} - Ocarina of Time Text Editor", m_inputFileName);
            }
        }

        private void SaveToOriginalRom()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.OriginalROM, m_inputFile, Version == ROMVer.Debug, CreditsMode);
        }

        private void SaveZZRP()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.ZZRP, m_inputFile, Version == ROMVer.Debug);
        }

        private void SaveZZRPL()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.ZZRPL, m_inputFile, Version == ROMVer.Debug);
        }

        private void SaveZ64ROM()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.Z64ROM, m_inputFile, Version == ROMVer.Debug);
        }

        private void Refresh()
        {
            try
            {
                short Cur = 0;
                Cur = SelectedMessage.MessageID;

                if (ZZRPLMode)
                    OpenZZRPL(Path1);
                else if (ZZRPMode)
                    OpenZZRP(Path1);
                else if (OldMode && Path2 == "")
                    OpenZ64ROM(Path1);
                else if (OldMode && Path2 != "")
                    OpenData(Path1, Path2);

                SelectedMessage = MessageList.First(x => x.MessageID == Cur);
            }
            catch (Exception)
            { }
        }

        private void SaveToFiles()
        {
            var ofd = new CommonOpenFileDialog();
            ofd.Title = "Choose Directory";
            ofd.IsFolderPicker = true;
            ofd.AddToMostRecentlyUsedList = false;
            ofd.AllowNonFileSystemItems = false;
            ofd.EnsureFileExists = true;
            ofd.EnsurePathExists = true;
            ofd.EnsureReadOnly = false;
            ofd.EnsureValidNames = true;
            ofd.Multiselect = false;
            ofd.ShowPlacesList = true;

            if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Exporter export = new Exporter(m_messageList, ofd.FileName, Enums.ExportType.File, Version == ROMVer.Debug);
            }
        }

        private void SaveToPatch()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Playstation Patch Format files (*.ppf)|*.ppf|All files|*";

            if (saveFile.ShowDialog() == true)
            {
                Exporter export = new Exporter(m_messageList, saveFile.FileName, Enums.ExportType.Patch, Version == ROMVer.Debug);
            }
        }

        private void Close()
        {
            MessageList = null;
            m_inputFile = null;
            m_inputFileName = "";
            ViewSource.Source = null;
            WindowTitle = "Ocarina of Time Text Editor";

            OldMode = false;
            ZZRPMode = false;

        }
        #endregion

        #region Adding and Removing Messages
        private void AddMessage()
        {
            Message newMes = new Message();
            newMes.MessageID = GetHighestID();
            MessageList.Insert(MessageList.Count - 1, newMes);
            ViewSource.View.Refresh();
        }

        private void RemoveMessage()
        {
            int selectedIndex = MessageList.IndexOf(SelectedMessage);
            MessageList.Remove(SelectedMessage);

            if (MessageList.Count == 0)
                MessageList.Add(new Message());

            if (selectedIndex == 0)
                SelectedMessage = MessageList[0];
            else
                SelectedMessage = MessageList[selectedIndex - 1];
        }

        private short GetHighestID()
        {
            short highest = short.MinValue;

            foreach (Message mes in MessageList)
            {
                if (highest < mes.MessageID)
                    highest = mes.MessageID;
            }

            return (short)(highest + 1);
        }
        #endregion

        #region Search Filtering
        private void Filter(object sender, FilterEventArgs e)
        {
            short findId;

            // see Notes on Filter Methods:
            var src = e.Item as Message;

            if (src == null)
                e.Accepted = false;

            //test if textbox message doesn't match our filter
            if (src.TextData != null && !src.TextData.ToUpper().Contains(SearchFilter.ToUpper()))
                e.Accepted = false;

            //test if filter matches a textbox id
            if (SearchFilter.StartsWith("0x")
                && short.TryParse(SearchFilter.Substring(2), System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.InvariantCulture, out findId))
            {
                if (src.MessageID == findId)
                    e.Accepted = true;
            }
        }

        private void AddFilter()
        {
            ViewSource.Filter -= new FilterEventHandler(Filter);
            ViewSource.Filter += new FilterEventHandler(Filter);
        }
        #endregion

        private void InsertControlCode(string code)
        {
            SelectedMessage.TextData = SelectedMessage.TextData.Insert(TextboxPosition, string.Format("<{0}>", code));
        }

        private RelayCommand onRequestOpenSFXesMenu;

        public ICommand OnRequestOpenSFXesMenu
        {
            get
            {
                if (onRequestOpenSFXesMenu == null)
                {
                    onRequestOpenSFXesMenu = new RelayCommand(PerformOnRequestOpenSFXesMenu);
                }

                return onRequestOpenSFXesMenu;
            }
        }

        private void PerformOnRequestOpenSFXesMenu(object commandParameter)
        {
            NPC_Maker.PickableList SFX = new NPC_Maker.PickableList(Dicts.SFXesFilename, true);
            System.Windows.Forms.DialogResult DR = SFX.ShowDialog();

            if (DR == System.Windows.Forms.DialogResult.OK)
            {
                InsertControlCode($"SOUND:{SFX.Chosen.Name}");
            }
        }

    }
}
