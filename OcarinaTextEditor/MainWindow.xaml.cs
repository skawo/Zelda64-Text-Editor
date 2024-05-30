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

namespace Zelda64TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsMajoraMode = false;
        private List<List<byte>> BoxesData = new List<List<byte>>();
        private Bitmap CurrentPreview;

        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(OcarinaTextboxType)).Cast<OcarinaTextboxType>();
            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
            MajoraIconCombo.ItemsSource = Enum.GetValues(typeof(MajoraIcon)).Cast<MajoraIcon>().OrderByDescending(x => x);

            DockTextBoxOptions.Height = 95;
            textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);

            ViewModel view = (ViewModel)DataContext;
            view.MessageAdded += View_MessageAdded;
            view.ROMVersionChanged += View_ROMVersionChanged;

            msgPreview.MouseRightButtonUp += MsgPreview_MouseRightButtonUp;

            ConstructContextMenu();
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

                Dicts.ReloadDict(Dicts.MMSFXesFilename, ref Dicts.SFXes);
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

                Dicts.ReloadDict(Dicts.OoTSFXesFilename, ref Dicts.SFXes);
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

                foreach (object Icon in Enum.GetValues(typeof(Enums.OcarinaIcon)))
                    Icons.AddRange(new string[] { Icon.ToString(), $"{OcarinaControlCode.ICON}:{Icon}", "" });

                AddTagControlsToMenu(IconTagMenu, Icons.ToArray());
                _ = ControlTagsMenu.Items.Add(IconTagMenu);

                SoundEffectMenu.Click += SoundEffectMenu_Click;
                _ = ControlTagsMenu.Items.Add(SoundEffectMenu);


                _ = textBoxMsg.ContextMenu.Items.Add(ControlTagsMenu);
            }


        }

        private void SoundEffectMenu_Click(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            NPC_Maker.PickableList SFX = new NPC_Maker.PickableList(view.MajoraMaskMode ? Dicts.MMSFXesFilename : Dicts.OoTSFXesFilename, true);
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
            Bitmap bmpTemp;
            Bitmap bmpOut;

            Message mes = view.SelectedMessage;
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

            ZeldaMessage.MessagePreviewMajora mp = new ZeldaMessage.MessagePreviewMajora(outD, view.BomberMsgsList.Contains(mes.MessageID));

            if (mp.Message.Count == BoxesData.Count && CurrentPreview != null && !mp.InBombersNotebook)
            {
                using (Graphics grfx = Graphics.FromImage(CurrentPreview))
                {
                    for (int i = 0; i < BoxesData.Count; i++)
                    {
                        if (mp.Message[i].Count != BoxesData[i].Count || !Enumerable.SequenceEqual(mp.Message[i], BoxesData[i]))
                        {
                            bmpTemp = mp.GetPreview(i, true, 1.5f);
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
                bmpTemp = mp.GetPreview(0, true, 1.5f);

                bmpOut = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
                bmpOut.MakeTransparent();

                using (Graphics grfx = Graphics.FromImage(bmpOut))
                {
                    grfx.DrawImage(bmpTemp, 0, 0);

                    for (int i = 1; i < mp.MessageCount; i++)
                    {
                        bmpTemp = mp.GetPreview(i, true, 1.5f);
                        grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                    }
                }
            }

            BoxesData = mp.Message;
            CurrentPreview = bmpOut;
            msgPreview.Source = BitmapToImageSource(bmpOut);
        }

        private void Ocarina_RenderPreview()
        {
            ViewModel view = (ViewModel)DataContext;
            Bitmap bmpTemp;
            Bitmap bmpOut;

            Message mes = new Message(textBoxMsg.Text, (OcarinaTextboxType)BoxTypeCombo.SelectedIndex);
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)BoxTypeCombo.SelectedIndex, outD);

            if (mp.Message.Count == BoxesData.Count && CurrentPreview != null)
            {
                using (Graphics grfx = Graphics.FromImage(CurrentPreview))
                {
                    for (int i = 0; i < BoxesData.Count; i++)
                    {
                        if (mp.Message[i].Count != BoxesData[i].Count || !Enumerable.SequenceEqual(mp.Message[i], BoxesData[i]))
                        {
                            bmpTemp = mp.GetPreview(i, true, 1.5f);
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
                bmpTemp = mp.GetPreview(0, true, 1.5f);

                bmpOut = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
                bmpOut.MakeTransparent();

                using (Graphics grfx = Graphics.FromImage(bmpOut))
                {
                    grfx.DrawImage(bmpTemp, 0, 0);

                    for (int i = 1; i < mp.MessageCount; i++)
                    {
                        bmpTemp = mp.GetPreview(i, true, 1.5f);
                        grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                    }
                }
            }

            BoxesData = mp.Message;
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
            catch (Exception)
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
            BoxesData = new List<List<byte>>();

            ViewModel view = (ViewModel)DataContext;

            if (view.SelectedMessage != null)
            {
                if (view.MajoraMaskMode)
                {
                    BoxTypeCombo.SelectedItem = view.SelectedMessage.MajoraBoxType;
                    MajoraIconCombo.SelectedItem = (MajoraIcon)view.SelectedMessage.MajoraIcon;

                    MajoraJumpToTextBox.TextChanged -= MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged -= MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged -= MajoraSecondPriceTextBox_TextChanged;

                    MajoraJumpToTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraFirstPriceTextBox.Background = System.Windows.Media.Brushes.White;
                    MajoraSecondPriceTextBox.Background = System.Windows.Media.Brushes.White;

                    MajoraJumpToTextBox.Text = "0x" + Convert.ToString((ushort)view.SelectedMessage.MajoraNextMessage, 16).ToUpper();
                    MajoraFirstPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraFirstItemPrice);
                    MajoraSecondPriceTextBox.Text = Convert.ToString(view.SelectedMessage.MajoraSecondItemPrice);

                    MajoraJumpToTextBox.TextChanged += MajoraJumpToTextBox_TextChanged;
                    MajoraFirstPriceTextBox.TextChanged += MajoraFirstPriceTextBox_TextChanged;
                    MajoraSecondPriceTextBox.TextChanged += MajoraSecondPriceTextBox_TextChanged;
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

            view.SelectedMessage.MajoraIcon = (byte)(MajoraIcon)MajoraIconCombo.SelectedItem;
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
    }
}
