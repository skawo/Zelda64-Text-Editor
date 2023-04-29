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

        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(OcarinaTextboxType)).Cast<OcarinaTextboxType>();
            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
            MajoraIconCombo.ItemsSource = Enum.GetValues(typeof(MajoraIcon)).Cast<MajoraIcon>().OrderByDescending(x => x);

            DockTextBoxOptions.Height = 95;
            textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);

            ConstructContextMenu();

        }


        private void ConstructContextMenu()
        {
            ViewModel view = (ViewModel)DataContext;

            textBoxMsg.ContextMenu = new ContextMenu();

            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Cut", Command = ApplicationCommands.Cut });
            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Copy", Command = ApplicationCommands.Copy });
            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Paste", Command = ApplicationCommands.Paste });

            if (IsMajoraMode)
            {
                textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Cut", Command = ApplicationCommands.Cut });
                textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Copy", Command = ApplicationCommands.Copy });
                textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Paste", Command = ApplicationCommands.Paste });

            }
            else
            {
                MenuItem ControlTagsMenu = new MenuItem() { Header = "Control Tags..." };

                MenuItem ColorTagMenu = new MenuItem() { Header = "Color", ToolTip = "Text until the next Color tag (or until the end of the textbox if none are present) will be of this color." };

                string[] Colors = new string[] { "White",   "W",    "Will appear black in 'None_White'-type textboxes",
                                                 "Red",     "R",    "Appears orange in 'Wooden'-type textboxes",
                                                 "Green",   "G",    "",
                                                 "Blue",    "B",    "",
                                                 "Cyan",    "C",    "",
                                                 "Magenta", "M",    "",
                                                 "Yellow",  "Y",    "",
                                                 "Black",   "BLK",  ""};

                AddTagControlsToMenu(ColorTagMenu, Colors);
                ControlTagsMenu.Items.Add(ColorTagMenu);


                MenuItem HighScoreTagMenu = new MenuItem() { Header = "High Score", ToolTip = "Prints one of the High Scores." };

                string[] HighScores = new string[] { "Archery",                 "HIGH SCORE:ARCHERY",           "",
                                                     "Poe Salesman Points",     "HIGH SCORE:POE_POINTS",        "",
                                                     "Fishing",                 "HIGH SCORE:FISHING",           "",
                                                     "Horse Race",              "HIGH SCORE:HORSE_RACE",        "",
                                                     "Running Man's Marathon",  "HIGH SCORE:MARATHON",          "",
                                                     "Dampe Race",              "HIGH SCORE:DAMPE_RACE",        ""};

                AddTagControlsToMenu(HighScoreTagMenu, HighScores);
                ControlTagsMenu.Items.Add(HighScoreTagMenu);

                MenuItem IconTagMenu = new MenuItem() { Header = "Icon...", ToolTip = "Draws specified icon inside the textbox." };
                List<string> Icons = new List<string>();

                foreach (var Icon in Enum.GetValues(typeof(Enums.OcarinaIcon)))
                {
                    Icons.Add(Icon.ToString());
                    Icons.Add($"ICON:{Icon}");
                    Icons.Add("");
                }

                AddTagControlsToMenu(IconTagMenu, Icons.ToArray());
                ControlTagsMenu.Items.Add(IconTagMenu);

                MenuItem SoundEffectMenu = new MenuItem() { Header = "Sound...", ToolTip = "Plays a sound effect. Only one sound effect can be played per textbox." };
                SoundEffectMenu.Click += SoundEffectMenu_Click;

                string[] GenericTag = new string[] { "Delay",                   "DELAY:0",      "Waits for the specified number of frames until switching to the next textbox.",
                                                     "Fade",                    "FADE:0",       "Waits for the specified number of frames until ending the textbox.",
                                                     "Fade2",                   "FADE2:0",      "Waits for the specified number of frames until ending the textbox. The duration can be made longer than with the FADE tag.",
                                                     "Offset",                  "SHIFT:0",      "Insert the specified number of spaces into the textbox.",
                                                     "New textbox",             "NEW_BOX",      "Starts a new message.",
                                                     "Jump",                    "JUMP:0",       "Jumps to the specified message ID.",
                                                     "Player name",             "PLAYER",       "Writes out the player's name (set on the file selection screen).",
                                                     "No skip",                 "NS",           "Disallows skipping the message box it's inserted into using the B button.",
                                                     "Speed",                   "SPEED:0",      "Sets the amount of frames spent waiting between typing out each character.",
                                                     "Persistent",              "PERSISTENT",   "Prevents the player from closing the textbox in any way. Used for shop descriptions.",
                                                     "Event",                   "EVENT",        "Prevents the textbox from closing until a programmed event does so.",
                                                     "Background",              "BACKGROUND:0", "Used to draw the failure X whenever player plays a song wrong. The variable seems to control the color.",
                                                     "Draw instantly",          "DI",           "Prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                     "Draw per-character",      "DC",           "Prints whatever follows this tag one character at a time. This is the default typing mode.",
                                                     "Button prompt",           "AWAIT_BUTTON", "Waits until the player presses a button.",
                                                     "Two choices",             "TWO_CHOICES",  "Displays a prompt which lets the player choose between two choices.",
                                                     "Three choices",           "THREE_CHOICES","Displays a prompt which lets the player choose between three choices." 
                                                   };

                AddTagControlsToMenu(ControlTagsMenu, GenericTag);
                textBoxMsg.ContextMenu.Items.Add(ControlTagsMenu);
            }


        }

        private void SoundEffectMenu_Click(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            NPC_Maker.PickableList SFX = new NPC_Maker.PickableList(view.MajoraMaskMode ? Dicts.MMSFXesFilename : Dicts.OoTSFXesFilename, true);
            System.Windows.Forms.DialogResult DR = SFX.ShowDialog();

            if (DR == System.Windows.Forms.DialogResult.OK)
            {
                InsertControlCode($"SOUND:{SFX.Chosen.Name}");
            }
        }

        private void AddTagControlsToMenu(MenuItem DstItem, string[] Tags)
        {
            for (int i = 0; i < Tags.Length; i += 3)
            {
                MenuItem t = new MenuItem();
                t.Header = Tags[i];
                t.Tag = Tags[i + 1];
                
                if (Tags[i + 2] != "")
                    t.ToolTip = Tags[i + 2];

                t.Click += InsertControlCode;
                DstItem.Items.Add(t);
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
            view.SelectedMessage.TextData = view.SelectedMessage.TextData.Insert(view.TextboxPosition, string.Format("<{0}>", (sender as Control).Tag));
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

            Message mes = view.SelectedMessage;
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

            ZeldaMessage.MessagePreviewMajora mp = new ZeldaMessage.MessagePreviewMajora(outD, view.BomberMsgsList.Contains(mes.MessageID));
            Bitmap bmpTemp = mp.GetPreview(0, true, 1.5f);

            Bitmap bmp = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
            bmp.MakeTransparent();

            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                grfx.DrawImage(bmpTemp, 0, 0);

                for (int i = 1; i < mp.MessageCount; i++)
                {
                    bmpTemp = mp.GetPreview(i, true, 1.5f);
                    grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                }
            }

            msgPreview.Dispatcher.Invoke(() =>
            {
                msgPreview.Source = BitmapToImageSource(bmp);
            });
        }

        private void Ocarina_RenderPreview()
        {
            ViewModel view = (ViewModel)DataContext;

            Message mes = new Message(textBoxMsg.Text, (OcarinaTextboxType)BoxTypeCombo.SelectedIndex);
            byte[] outD = mes.ConvertTextData(view.Version, view.CreditsMode, false).ToArray();

            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)BoxTypeCombo.SelectedIndex, outD);
            Bitmap bmpTemp = mp.GetPreview(0, true, 1.5f);

            Bitmap bmp = new Bitmap(bmpTemp.Width, mp.MessageCount * bmpTemp.Height);
            bmp.MakeTransparent();

            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                grfx.DrawImage(bmpTemp, 0, 0);

                for (int i = 1; i < mp.MessageCount; i++)
                {
                    bmpTemp = mp.GetPreview(i, true, 1.5f);
                    grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                }
            }

            msgPreview.Dispatcher.Invoke(() => 
            {
                msgPreview.Source = BitmapToImageSource(bmp);
            });
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
            ViewModel view = (ViewModel)DataContext;

            if (view.SelectedMessage != null)
            {
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
                    if (IsMajoraMode)
                    {
                        IsMajoraMode = false;
                        BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(OcarinaTextboxType)).Cast<OcarinaTextboxType>();
                        BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(TextboxPosition)).Cast<TextboxPosition>().Where(x => x <= TextboxPosition.Bottom);
                        DockTextBoxOptions.Height = 95;
                        textBoxMsgDock.Margin = new Thickness(0, 118, 0, 10);
                        ConstructContextMenu();
                    }

                    BoxTypeCombo.SelectedItem = view.SelectedMessage.BoxType;
                }
            }
        }

        private void MajoraIconCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;

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
    }
}
