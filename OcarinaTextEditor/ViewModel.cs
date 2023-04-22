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

        private Enums.EditorMode _Mode = EditorMode.None;
        public Enums.EditorMode Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsSaveAvailable");
                NotifyPropertyChanged("IsSaveAsEnabled");
                NotifyPropertyChanged("IsPPFAvailable");
            }
        }

        public bool IsSaveAsEnabled
        {
            get { return Mode == EditorMode.ROMMode || Mode == EditorMode.FilesMode; }
        }

        public bool IsPPFAvailable
        {
            get { return Mode == EditorMode.ROMMode; }
        }

        public bool IsSaveAvailable
        {
            get { return Mode != EditorMode.None; }
        }

        public string Path1 { get; set; }
        public string Path2 { get; set; }

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

        public ROMVer Version = ROMVer.Unknown;

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

        public ICommand OnRequestRefresh
        {
            get { return new RelayCommand(x => Refresh(), x => MessageList != null); }
        }

        public ICommand OnRequestSave
        {
            get { return new RelayCommand(x => Save(), x => MessageList != null); }
        }

        public ICommand OnRequestSaveAs
        {
            get { return new RelayCommand(x => SaveAs(), x => MessageList != null); }
        }

        public ICommand OnRequestSaveAsFiles
        {
            get { return new RelayCommand(x => SaveToFiles(), x => MessageList != null); }
        }

        #endregion

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

                foreach (var i in ROMInfo.ROMBuildDatesOffsets)
                {
                    reader.BaseStream.Seek(i.Value, 0);
                    reader.Read(Buffer, 0, 8);
                    string Date = Encoding.ASCII.GetString(Buffer);

                    ROMVer Out = ROMInfo.GetROMVerFromDate(i.Key, Date);

                    if (Out != ROMVer.Unknown)
                    {
                        return Out;
                    }
                }

                System.Windows.MessageBox.Show("ROM unsupported. Make sure to decompress your ROM first!");
                return ROMVer.Unknown;
            }
        }

        #region Input/Output
        private void Open(string PathD = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "All known formats|*.n64;*.z64;*.zzrpl;*.toml;*.zzrp|N64 ROMs (*.n64, *.z64)|*.n64;*.z64|zzrtl Projects (*.zzrpl)|*.zzrpl|Z64ROM Config File (*.toml)|*.toml|zzromtool Projects (*.zzrp)|*.zzrp|All files|*";

            if (PathD != "" || openFile.ShowDialog() == true)
            {
                if (PathD != "")
                    openFile.FileName = PathD;
            }

            switch (Path.GetExtension(openFile.FileName))
            {
                case ".zzrpl":
                    OpenZZRPL(openFile.FileName); break;
                case ".toml":
                    OpenZ64ROM(openFile.FileName); break;
                case ".zzrp":
                    OpenZZRP(openFile.FileName); break;
                case ".n64":
                case ".z64":
                default:
                    OpenROM(openFile.FileName); break;
            }
        }

        private void OpenROM(string PathD = "")
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

                Mode = EditorMode.ROMMode;

                Importer file = new Importer(openFile.FileName, Mode, Version, CreditsMode);
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

                Mode = EditorMode.ZZRPLMode;

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

                Mode = EditorMode.Z64ROMMode;

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

                Mode = EditorMode.ZZRPMode;

                Importer file = new Importer(openFile.FileName, Mode, Version);
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

            Mode = EditorMode.FilesMode;

            Importer file = new Importer(tableFileName, messageDataFileName);
            MessageList = file.GetMessageList();

            ViewSource.Source = MessageList;
            SelectedMessage = MessageList[0];

            WindowTitle = string.Format("{0} - Ocarina of Time Text Editor", tableFileName);


            Path1 = tableFileName;
            Path2 = messageDataFileName;
        }

        private void SaveToNewRom()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "N64 ROMs (*.n64, *.z64)|*.n64;*.z64|All files|*";

            if (saveFile.ShowDialog() == true)
            {
                Exporter export = new Exporter(m_messageList, saveFile.FileName, Enums.ExportType.NewROM, m_inputFile, Version, CreditsMode);
                m_inputFileName = saveFile.FileName;
                WindowTitle = string.Format("{0} - Ocarina of Time Text Editor", m_inputFileName);
            }
        }

        private void Save()
        {
            switch (Mode)
            {
                case EditorMode.ROMMode:
                    SaveToOriginalRom(); break;
                case EditorMode.Z64ROMMode:
                    SaveZ64ROM(); break;
                case EditorMode.ZZRPLMode:
                    SaveZZRPL(); break;
                case EditorMode.ZZRPMode:
                    SaveZZRP(); break;
                case EditorMode.FilesMode:
                    SaveToFiles(); break;
            }
        }

        private void SaveAs()
        {
            switch (Mode)
            {
                case EditorMode.ROMMode:
                    SaveToNewRom(); break;
                case EditorMode.FilesMode:
                    SaveToFiles(); break;
            }
        }

        private void SaveToOriginalRom()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.OriginalROM, m_inputFile, Version, CreditsMode);
        }

        private void SaveZZRP()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.ZZRP, m_inputFile, Version);
        }

        private void SaveZZRPL()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.ZZRPL, m_inputFile, Version);
        }

        private void SaveZ64ROM()
        {
            Exporter export = new Exporter(m_messageList, m_inputFileName, Enums.ExportType.Z64ROM, m_inputFile, Version);
        }

        private void Refresh()
        {
            try
            {
                short Cur = 0;
                Cur = SelectedMessage.MessageID;

                switch (Mode)
                {
                    case EditorMode.ZZRPLMode:
                        OpenZZRPL(Path1); break;
                    case EditorMode.ZZRPMode:
                        OpenZZRP(Path1); break;
                    case EditorMode.Z64ROMMode:
                        OpenZ64ROM(Path1); break;
                    case EditorMode.ROMMode:
                        OpenZ64ROM(Path1); break;
                    case EditorMode.FilesMode:
                        OpenData(Path1, Path2); break;
                }

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
                Exporter export = new Exporter(m_messageList, ofd.FileName, Enums.ExportType.File, null, Version);
            }
        }

        private void Close()
        {
            MessageList = null;
            m_inputFile = null;
            m_inputFileName = "";
            ViewSource.Source = null;
            WindowTitle = "Ocarina of Time Text Editor";
            Mode = EditorMode.None;

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
