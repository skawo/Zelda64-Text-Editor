using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zelda64TextEditor.Enums;
using Path = System.IO.Path;

namespace Zelda64TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsMajoraMode = false;
        private List<ZeldaMsgPreview.Textbox> BoxesData = new List<ZeldaMsgPreview.Textbox>();
        private Bitmap CurrentPreview;

        private float[] FontWidths = null;
        private byte[] FontData = null;

        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(OcarinaTextboxType)).Cast<OcarinaTextboxType>();
            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
            MajoraIconCombo.ItemsSource = Enum.GetValues(typeof(ZeldaMsgPreview.MajoraIcon)).Cast<ZeldaMsgPreview.MajoraIcon>().OrderByDescending(x => x);

            DockTextBoxOptions.Height = 95;
            textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);

            ViewModel view = (ViewModel)DataContext;
            view.MessageAdded += View_MessageAdded;
            view.ROMVersionChanged += View_ROMVersionChanged;

            msgPreview.MouseRightButtonUp += MsgPreview_MouseRightButtonUp;

            ConstructContextMenu();

            try
            {
                if (System.IO.File.Exists("font.width_table"))
                {
                    byte[] widths = System.IO.File.ReadAllBytes("font.width_table");
                    FontWidths = new float[widths.Length / 4];

                    for (int i = 0; i < widths.Length; i += 4)
                    {
                        byte[] width = widths.Skip(i).Take(4).Reverse().ToArray();
                        FontWidths[i / 4] = BitConverter.ToSingle(width, 0);
                    }
                }
            }
            catch (Exception)
            { }

            try
            {
                if (System.IO.File.Exists("font.font_static"))
                {
                    FontData = System.IO.File.ReadAllBytes("font.font_static");
                }
            }
            catch (Exception)
            {
                FontData = null;
            }

            foreach (string arg in App.startupEventArgs.Args)
            {
                if (File.Exists(arg))
                    switch (Path.GetExtension(arg).ToLower())
                    {
                        case ".zzrpl":
                        case ".toml":
                        case ".zzrp":
                        case ".n64":
                        case ".z64":
                            {
                                view.Open(arg);
                                break;
                            }
                    }
            }
        }

        private void MsgPreview_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
            sf.DefaultExt = ".png";
            sf.FileName = "textbox";
            
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var fileStream = new FileStream(sf.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(msgPreview.Source as BitmapSource));
                    encoder.Save(fileStream);
                }
            }
        }

        private void View_ROMVersionChanged()
        {
            ViewModel view = (ViewModel)DataContext;

            if (view.MajoraMaskMode)
            {
                if (!IsMajoraMode)
                {
                    IsMajoraMode = true;
                    BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(MajoraTextboxType)).Cast<MajoraTextboxType>();
                    BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>();
                    DockTextBoxOptions.Height = 215;
                    textBoxMsgDock.Margin = new Thickness(0, 241, 0, 10);
                    ConstructContextMenu();
                }


                Dicts.ReloadSfxesDict(Dicts.MMSFXesFilename, view.sfxHPath, ref Dicts.SFXes);
            }
            else if (!view.MajoraMaskMode)
            {
                if (IsMajoraMode)
                {
                    IsMajoraMode = false;
                    BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(OcarinaTextboxType)).Cast<OcarinaTextboxType>();
                    BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
                    DockTextBoxOptions.Height = 95;
                    textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);
                    ConstructContextMenu();
                }

                Dicts.ReloadSfxesDict(Dicts.OoTSFXesFilename, view.sfxHPath, ref Dicts.SFXes);
            }


        }

        private void View_MessageAdded(Message AddedMsg)
        {
            messageListView.UpdateLayout();
            int Index = messageListView.Items.IndexOf(AddedMsg);

            if (Index >= 0)
            {
                messageListView.ScrollIntoView(messageListView.Items[Index]);
                messageListView.SelectedItem = messageListView.Items[Index];
            }
        }

        private void ConstructContextMenu()
        {
            ViewModel view = (ViewModel)DataContext;

            textBoxMsg.ContextMenu = new ContextMenu();

            _ = textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Cut",   Command = ApplicationCommands.Cut });
            _ = textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Copy",  Command = ApplicationCommands.Copy });
            _ = textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Paste", Command = ApplicationCommands.Paste });

            MenuItem CopyAsCItem = new MenuItem() { Header = "Copy C String" };
            CopyAsCItem.Click += CopyAsC;
            textBoxMsg.ContextMenu.Items.Add(CopyAsCItem);

            MenuItem PasteAsCItem = new MenuItem() { Header = "Paste C String" };
            PasteAsCItem.Click += PasteAsCItem_Click;
            textBoxMsg.ContextMenu.Items.Add(PasteAsCItem);

            if (IsMajoraMode)
            {
                MenuItem ControlTagsMenu = new MenuItem() {     Header = "Control Tags..." };

                MenuItem ColorTagMenu = new MenuItem() {        Header = "Color",               ToolTip = "Text until the next Color tag will be of this color. The color will persist even to the next textbox." };
                MenuItem ButtonTagMenu = new MenuItem() {       Header = "Buttons",             ToolTip = "Add a button icon to the textbox." };
                MenuItem ScoreTagMenu = new MenuItem() {        Header = "Scores and timers",   ToolTip = "Various scores and timers." };
                MenuItem PromptTagMenu = new MenuItem() {       Header = "Prompts",             ToolTip = "Tags relating to player input." };
                MenuItem CompletionTagMenu = new MenuItem() {   Header = "Completion-related",  ToolTip = "Tags relating to quest completion." };
                MenuItem SoundEffectMenu = new MenuItem() {     Header = "Sound...",            ToolTip = "Plays a sound effect." };

                AddTagControlsToMenu(ColorTagMenu, ContextMenuData.MajoraColors);
                AddTagControlsToMenu(ButtonTagMenu, ContextMenuData.ButtonsMajora);
                AddTagControlsToMenu(ScoreTagMenu, ContextMenuData.ScoresMajora);
                AddTagControlsToMenu(CompletionTagMenu, ContextMenuData.CompletionMajora);
                AddTagControlsToMenu(PromptTagMenu, ContextMenuData.PromptsMajora);
                AddTagControlsToMenu(ControlTagsMenu, ContextMenuData.GenericTagMajora);

                _ = ControlTagsMenu.Items.Add(ColorTagMenu);
                _ = ControlTagsMenu.Items.Add(ButtonTagMenu);
                _ = ControlTagsMenu.Items.Add(ScoreTagMenu);
                _ = ControlTagsMenu.Items.Add(PromptTagMenu);
                _ = ControlTagsMenu.Items.Add(CompletionTagMenu);

                SoundEffectMenu.Click += SoundEffectMenu_Click;
                _ = ControlTagsMenu.Items.Add(SoundEffectMenu);

                _ = textBoxMsg.ContextMenu.Items.Add(ControlTagsMenu);
            }
            else
            {
                MenuItem ControlTagsMenu = new MenuItem() {     Header = "Control Tags..." };

                MenuItem ColorTagMenu = new MenuItem() {        Header = "Color",       ToolTip = "Text until the next Color tag (or until the end of the textbox if none are present) will be of this color." };
                MenuItem HighScoreTagMenu = new MenuItem() {    Header = "High Score",  ToolTip = "Prints a player's high score." };
                MenuItem ButtonTagMenu = new MenuItem() {       Header = "Buttons",     ToolTip = "Add a button icon to the textbox." };
                MenuItem ScoreTagMenu = new MenuItem() {        Header = "Score",       ToolTip = "Prints a player's score." };
                MenuItem IconTagMenu = new MenuItem() {         Header = "Icon...",     ToolTip = "Draws specified icon inside the textbox." };
                MenuItem SoundEffectMenu = new MenuItem() {     Header = "Sound...",    ToolTip = "Plays a sound effect. Only one sound effect can be played per textbox." };

                AddTagControlsToMenu(ColorTagMenu, ContextMenuData.ColorsOcarina);
                AddTagControlsToMenu(HighScoreTagMenu, ContextMenuData.HighScoresOcarina);
                AddTagControlsToMenu(ButtonTagMenu, ContextMenuData.ButtonsOcarina);
                AddTagControlsToMenu(ScoreTagMenu, ContextMenuData.ScoresOcarina);
                AddTagControlsToMenu(ControlTagsMenu, ContextMenuData.GenericTagOcarina);

                _ = ControlTagsMenu.Items.Add(ColorTagMenu);
                _ = ControlTagsMenu.Items.Add(HighScoreTagMenu);
                _ = ControlTagsMenu.Items.Add(ButtonTagMenu);
                _ = ControlTagsMenu.Items.Add(ScoreTagMenu);

                List<string> Icons = new List<string>();

                foreach (object Icon in Enum.GetValues(typeof(ZeldaMsgPreview.OcarinaIcon)))
                    Icons.AddRange(new string[] { Icon.ToString(), $"{ZeldaMsgPreview.OcarinaControlCode.ICON}:{Icon}", "" });

                AddTagControlsToMenu(IconTagMenu, Icons.ToArray());
                _ = ControlTagsMenu.Items.Add(IconTagMenu);

                SoundEffectMenu.Click += SoundEffectMenu_Click;
                _ = ControlTagsMenu.Items.Add(SoundEffectMenu);


                _ = textBoxMsg.ContextMenu.Items.Add(ControlTagsMenu);
            }


        }

        private void PasteAsCItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel view = (ViewModel)DataContext;

                string s = Clipboard.GetText();
                s = s.TrimStart('\"').TrimEnd('\"');

                List<byte> b = new List<byte>();

                if (IsMajoraMode)
                    b.AddRange(new byte[11]);

                for (int i = 0; i < s.Length; i++)
                {
                    char ch = s[i];

                    if (ch == '\\')
                    {
                        // Skip the backslash and 'x'
                        i += 2;

                        if (i + 1 >= s.Length)
                            throw new ArgumentException("Invalid escape sequence");

                        // Parse the hex digits
                        char[] arr = new char[] { s[i], s[i + 1] };
                        string st = new string(arr);
                        b.Add((byte)int.Parse(st, System.Globalization.NumberStyles.HexNumber));

                        // Move past the hex digits
                        i += 2;

                        // Skip any whitespace
                        while (i < s.Length && char.IsWhiteSpace(s[i]))
                        {
                            i++;
                        }

                        // Skip the closing quote if present
                        if (i < s.Length && s[i] == '\"')
                        {
                            i++;

                            // Skip any whitespace after the quote
                            while (i < s.Length && char.IsWhiteSpace(s[i]))
                                i++;

                            // Skip the opening quote if present
                            if (i < s.Length && s[i] == '\"')
                                i++;
                        }

                        i--;
                        continue;
                    }
                    else
                    {
                        b.Add((byte)s[i]);
                    }
                }

                Message msg = new Message(b.ToArray(), new TableRecord(), view.CreditsMode, view.Version);
                textBoxMsg.Text = msg.TextData;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error parsing message string: {ex.Message}");
            }
        }

        private void CopyAsC(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            Message msg = new Message();

            msg.TextData = textBoxMsg.SelectedText;

            if (msg.TextData == "")
                msg = (messageListView.SelectedItem as Message);

            string outS = msg.ConvertToCString(view.Version, view.CreditsMode, true);

            if (outS != "")
                Clipboard.SetText(outS);
        }

        private void SoundEffectMenu_Click(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            NPC_Maker.PickableList SFX =
                view.Mode == EditorMode.Z64ROMMode
                ? new NPC_Maker.PickableList(Dicts.OoTSFXesFilename, view.sfxHPath, true, null, Path.GetDirectoryName(view.Path1) + "/include/sfx_enum.h")
                : new NPC_Maker.PickableList(view.MajoraMaskMode ? Dicts.MMSFXesFilename : Dicts.OoTSFXesFilename, "", true);
            System.Windows.Forms.DialogResult DR = SFX.ShowDialog();

            if (DR == System.Windows.Forms.DialogResult.OK)
                InsertControlCode($"SOUND:{SFX.Chosen.Name}");
        }

        private void AddTagControlsToMenu(MenuItem DstItem, string[] Tags)
        {
            for (int i = 0; i < Tags.Length; i += 3)
            {
                MenuItem t = new MenuItem
                {
                    Header = Tags[i],
                    Tag = Tags[i + 1]
                };

                if (Tags[i + 2] != "")
                    t.ToolTip = Tags[i + 2];

                t.Click += InsertControlCode;
                _ = DstItem.Items.Add(t);
            }
        }

        private void InsertControlCode(string s)
        {
            ViewModel view = (ViewModel)DataContext;
            view.SelectedMessage.TextData = view.SelectedMessage.TextData.Insert(view.TextboxPosition, string.Format("<{0}>", s));
        }

        private void InsertControlCode(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            view.SelectedMessage.TextData = view.SelectedMessage.TextData.Insert(view.TextboxPosition, string.Format("<{0}>", ((sender as Control).Tag as string)));
        }

        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();


                return bitmapimage;
            }
        }

        private void SetMsgBackground(int Type)
        {
            if (Type == 4)
                dockMsgPreview.Background = System.Windows.Media.Brushes.Black;
            else
                dockMsgPreview.Background = System.Windows.Media.Brushes.White;
        }

        private void BoxTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BoxTypeCombo.SelectedItem == null)
                return;

            ViewModel view = (ViewModel)DataContext;

            if (view.MajoraMaskMode)
                view.SelectedMessage.MajoraBoxType = (MajoraTextboxType)BoxTypeCombo.SelectedItem;
            else
                view.SelectedMessage.BoxType = (OcarinaTextboxType)BoxTypeCombo.SelectedItem;

            CurrentPreview = null;

            textBoxMsg.TextChanged -= TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged -= BoxTypeCombo_SelectionChanged;

            SetMsgBackground(BoxTypeCombo.SelectedIndex);
            TextBoxMsg_TextChanged(null, null);

            textBoxMsg.TextChanged += TextBoxMsg_TextChanged;
            BoxTypeCombo.SelectionChanged += BoxTypeCombo_SelectionChanged;
        }

        private void Majora_RenderPreview()
        {
            
            ViewModel view = (ViewModel)DataContext;
            Bitmap bmpOut;

            Message mes = Message.MakeCopy(view.SelectedMessage);

            mes.TextData = Converters.CharMapTextConverter.RemapTextFrom(mes.TextData);

            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();
            bool IsInBombers = view.BomberMsgsList.Contains(mes.MessageID);

            if (outD.Length > 1280)
                msgSizeWarn.Visibility = Visibility.Visible;
            else
                msgSizeWarn.Visibility = Visibility.Hidden;


            ZeldaMsgPreview.Message mesP = new ZeldaMsgPreview.Message(ZeldaMsgPreview.Game.Majora, outD, (ZeldaMsgPreview.TextboxPosition)view.SelectedMessage.BoxPosition,
                                                                      (ZeldaMsgPreview.TextboxType)view.SelectedMessage.BoxType, FontData, FontWidths, view.CreditsMode, true, IsInBombers);

            bmpOut = mesP.GetPreview();

            if (bmpOut == null)
                throw new Exception("Error getting preview");

            BoxesData = mesP.Textboxes;
            CurrentPreview = bmpOut;
            msgPreview.Source = BitmapToImageSource(bmpOut);
        }

        private void Ocarina_RenderPreview()
        {
            ViewModel view = (ViewModel)DataContext;
            Bitmap bmpTemp;
            Bitmap bmpOut;

            string remapped = Converters.CharMapTextConverter.RemapTextFrom(textBoxMsg.Text);

            Message mes = new Message(remapped, (OcarinaTextboxType)BoxTypeCombo.SelectedIndex);
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

            if (outD.Length > 1280)
                msgSizeWarn.Visibility = Visibility.Visible;
            else
                msgSizeWarn.Visibility = Visibility.Hidden;

            ZeldaMsgPreview.Message mesP = new ZeldaMsgPreview.Message(view.Version == ROMVer.Debug ? ZeldaMsgPreview.Game.Ocarina_Debug : ZeldaMsgPreview.Game.Ocarina, outD, 
                                                                      (ZeldaMsgPreview.TextboxPosition)view.SelectedMessage.BoxPosition, 
                                                                      (ZeldaMsgPreview.TextboxType)view.SelectedMessage.BoxType, FontData, FontWidths, view.CreditsMode, true);

            if (mesP.Textboxes.Count == BoxesData.Count && CurrentPreview != null)
            {
                using (Graphics grfx = Graphics.FromImage(CurrentPreview))
                {
                    for (int i = 0; i < BoxesData.Count; i++)
                    {
                        if (mesP.Textboxes[i].DecodedData.Count != BoxesData[i].DecodedData.Count || !Enumerable.SequenceEqual(mesP.Textboxes[i].DecodedData, BoxesData[i].DecodedData))
                        {
                            bmpTemp = mesP.Textboxes[i].GetPreview();

                            if (bmpTemp == null)
                                throw new Exception("Error getting preview");

                            grfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                            grfx.FillRectangle(System.Drawing.Brushes.Transparent, 0, bmpTemp.Height * i, bmpTemp.Width, bmpTemp.Height);
                            grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                        }
                    }
                }

                bmpOut = CurrentPreview;
            }
            else
            {
                bmpOut = mesP.GetPreview();

                if (bmpOut == null)
                    throw new Exception("Error getting preview");
            }

            BoxesData = mesP.Textboxes;
            CurrentPreview = bmpOut;
            msgPreview.Source = BitmapToImageSource(bmpOut);
        }

        private void TextBoxMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                msgPreview.Opacity = 1;
                ViewModel view = (ViewModel)DataContext;

                if (view.SelectedMessage == null)
                    return;

                if (view.MajoraMaskMode)
                    Majora_RenderPreview();
                else
                    Ocarina_RenderPreview();
            }
            catch 
            {
                msgPreview.Opacity = 0.5;
            }

        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            TextBox box = sender as TextBox;
            view.TextboxPosition = box.SelectionStart;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPreview = null;
            BoxesData = new List<ZeldaMsgPreview.Textbox>();

            ViewModel view = (ViewModel)DataContext;

            if (view.SelectedMessage != null)
            {
                if (view.MajoraMaskMode)
                {
                    MajoraIconCombo.SelectionChanged -= MajoraIconCombo_SelectionChanged;
                    MajoraJumpToTextBox.TextChanged -= MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged -= MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged -= MajoraSecondPriceTextBox_TextChanged;

                    BoxTypeCombo.SelectedItem = view.SelectedMessage.MajoraBoxType;
                    MajoraIconCombo.SelectedItem = (ZeldaMsgPreview.MajoraIcon)view.SelectedMessage.MajoraIcon;

                    MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.White;

                    MajoraJumpToTextBox.Text = "0x" + Convert.ToString((ushort)view.SelectedMessage.MajoraNextMessage, 16).ToUpper();
                    MajoraFirstPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraFirstItemPrice);
                    MajoraSecondPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraSecondItemPrice);

                    MajoraJumpToTextBox.TextChanged += MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged += MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged += MajoraSecondPriceTextBox_TextChanged;
                    MajoraIconCombo.SelectionChanged += MajoraIconCombo_SelectionChanged;
                }
                else if (!view.MajoraMaskMode)
                {
                    BoxTypeCombo.SelectedItem = view.SelectedMessage.BoxType;
                }
            }
        }

        private void MajoraIconCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            CurrentPreview = null;

            view.SelectedMessage.MajoraIcon = (byte)(ZeldaMsgPreview.MajoraIcon)MajoraIconCombo.SelectedItem;
            TextBoxMsg_TextChanged(null, null);
        }

        private void MajoraJumpToTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraNextMessage = Convert.ToInt16(MajoraJumpToTextBox.Text.TrimStart(new char[] { '0', 'x' }), 16);
                MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraNextMessage = -1;
                MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void MajoraFirstPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraFirstItemPrice = Convert.ToInt16(MajoraFirstPriceTextBox.Text);
                MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraFirstItemPrice = -1;
                MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void MajoraSecondPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

            try
            {
                view.SelectedMessage.MajoraSecondItemPrice = Convert.ToInt16(MajoraSecondPriceTextBox.Text);
                MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception)
            {
                view.SelectedMessage.MajoraFirstItemPrice = -1;
                MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            int Index = messageListView.Items.IndexOf(view.SelectedMessage);

            if (Index >= 0)
            {
                messageListView.ScrollIntoView(messageListView.Items[Index]);
                messageListView.SelectedItem = messageListView.Items[Index];
            }
        }

        private void MessageListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ViewModel view = (ViewModel)DataContext;

                if (view.SelectedMessage != null)
                    view.RemoveMessage();

            }
        }

        private void MajoraBoxCenter_Checked(object sender, RoutedEventArgs e)
        {
            MajoraBoxCenter.Checked -= MajoraBoxCenter_Checked;
            MajoraBoxCenter.Unchecked -= MajoraBoxCenter_Checked;
            TextBoxMsg_TextChanged(null, null);
            MajoraBoxCenter.Unchecked += MajoraBoxCenter_Checked;
            MajoraBoxCenter.Unchecked += MajoraBoxCenter_Checked;
        }
    }
}
