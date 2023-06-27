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




            ConstructContextMenu();

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

            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Cut", Command = ApplicationCommands.Cut });
            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Copy", Command = ApplicationCommands.Copy });
            textBoxMsg.ContextMenu.Items.Add(new MenuItem() { Header = "Paste", Command = ApplicationCommands.Paste });

            if (IsMajoraMode)
            {
                MenuItem ControlTagsMenu = new MenuItem() { Header = "Control Tags..." };

                MenuItem ColorTagMenu = new MenuItem() { Header = "Color", ToolTip = "Text until the next Color tag will be of this color. The color will persist even to the next textbox." };

                string[] Colors = new string[] { "Default",   $"{MajoraMsgColor.D}",    "Will appear white in most cases, but black in 'None_White' and 'Bomber Notebook'-type textboxes. Will also appear black inside the Bomber's Notebook itself.",
                                                 "Red",       $"{MajoraMsgColor.R}",    "Appears orange in 'Wooden'-type textboxes",
                                                 "Green",     $"{MajoraMsgColor.G}",    "Inexplicably appears blue inside Bomber's Notebook",
                                                 "Blue",      $"{MajoraMsgColor.B}",    "",
                                                 "Yellow",    $"{MajoraMsgColor.Y}",    "",
                                                 "Navy",      $"{MajoraMsgColor.N}",    "",
                                                 "Silver",    $"{MajoraMsgColor.S}",    "",
                                                 "Orange",    $"{MajoraMsgColor.O}",    ""};

                AddTagControlsToMenu(ColorTagMenu, Colors);
                ControlTagsMenu.Items.Add(ColorTagMenu);

                MenuItem ButtonTagMenu = new MenuItem() { Header = "Buttons", ToolTip = "Add a button icon to the textbox." };

                string[] Button = new string[] { "A Button",         $"{MajoraControlCode.A_BUTTON}",           "",
                                                 "B Button",         $"{MajoraControlCode.B_BUTTON}",           "",
                                                 "C Button",         $"{MajoraControlCode.C_BUTTON}",           "",
                                                 "C-Up Button",      $"{MajoraControlCode.C_UP}",               "",
                                                 "C-Down Button",    $"{MajoraControlCode.C_DOWN}",             "",
                                                 "C-Left Button",    $"{MajoraControlCode.C_LEFT}",             "",
                                                 "C-Right Button",   $"{MajoraControlCode.C_RIGHT}",            "",
                                                 "L Button",         $"{MajoraControlCode.L_BUTTON}",           "",
                                                 "R Button",         $"{MajoraControlCode.R_BUTTON}",           "",
                                                 "Z Button",         $"{MajoraControlCode.Z_BUTTON}",           "",
                                                 "Triangle",         $"{MajoraControlCode.TRIANGLE}",           "",
                                                 "Control Stick",    $"{MajoraControlCode.CONTROL_STICK}",      "",
                                                 "D-Pad",            $"{MajoraControlCode.D_PAD}",              "Crashes the game",};

                AddTagControlsToMenu(ButtonTagMenu, Button);
                ControlTagsMenu.Items.Add(ButtonTagMenu);

                MenuItem ScoreTagMenu = new MenuItem() { Header = "Scores and timers", ToolTip = "Various scores and timers." };

                string[] Scores = new string[] { "Required Swamp Cruise Hits",               $"{MajoraControlCode.SWAMP_CRUISE_HITS}",              "Prints the number of hits required to win the Swamp Cruise reward.",
                                                 "Stray Fairies",                            $"{MajoraControlCode.STRAY_FAIRY_SCORE}",              "Amount of Stray Fairies collected in the current dungeon.",
                                                 "Gold Skulltulas",                          $"{MajoraControlCode.GOLD_SKULLTULAS}",                "Amount of Gold Skulltula tokens collected in the current spider house.",
                                                 "Postman Minigame Time",                    $"{MajoraControlCode.POSTMAN_RESULTS}",                "Time score attained in the postman minigame.",
                                                 "Timer",                                    $"{MajoraControlCode.TIMER}",                          "Prints time shown on the last timer.",
                                                 "Moon Crash Time Left",                     $"{MajoraControlCode.MOON_CRASH_TIME}",                "Print remaining time until Moon Crash (as on the Clock Tower roof)",
                                                 "Deku Flying Time",                         $"{MajoraControlCode.DEKU_RESULTS}",                   "Print time attained in the Deku Flying minigame",
                                                 "Town Shooting Gallery High Score",         $"{MajoraControlCode.TOWN_SHOOTING_HIGHSCORE}",        "Print the Town Shooting Gallery High Score",
                                                 "Shooting Gallery Result",                  $"{MajoraControlCode.SHOOTING_GALLERY_RESULT}",        "Print score attained in the Shooting Gallery",
                                                 "Swamp Cruise Score",                       $"{MajoraControlCode.SWAMP_CRUISE_RESULT}",            "Print score attained in the Swamp Cruise",
                                                 "Winning Lottery Number",                   $"{MajoraControlCode.WINNING_LOTTERY_NUM}",            "Print the winning lottery number",
                                                 "Player's Lottery Number",                  $"{MajoraControlCode.PLAYER_LOTTERY_NUM}",             "Print the player's lottery number",
                                                 "Time remains",                             $"{MajoraControlCode.MOON_CRASH_TIME_REMAINS}",        "Print time remaining in hours & minutes ",
                                                 "Hours remain",                             $"{MajoraControlCode.MOON_CRASH_HOURS_REMAIN}",        "Print time remaining in hours",
                                                 "Hours remain until morning",               $"{MajoraControlCode.UNTIL_MORNING}",                  "Print time remaining until sunrise in hours & minutes",
                                                 "Horseback Archery High Score",             $"{MajoraControlCode.EPONA_ARCHERY_HIGHSCORE}",        "Print the Epona Archery high score (Romani Ranch Balloon Game)",
                                                 "Deku Flying Highscore 1",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY1}",            "Print the Deku Flying Highscore from Day 1)",
                                                 "Deku Flying Highscore 2",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY2}",            "Print the Deku Flying Highscore from Day 2)",
                                                 "Deku Flying Highscore 3",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY3}",            "Print the Deku Flying Highscore from Day 3)",
                };

                AddTagControlsToMenu(ScoreTagMenu, Scores);
                ControlTagsMenu.Items.Add(ScoreTagMenu);



                MenuItem PromptTagMenu = new MenuItem() { Header = "Prompts", ToolTip = "Tags relating to player input." };

                string[] Prompts = new string[] { "Bank Prompt",                              $"{MajoraControlCode.BANK_PROMPT}",                    "Print the withdraw/deposit rupees prompt",
                                                  "Rupees Entered in Prompt",                 $"{MajoraControlCode.RUPEES_ENTERED}",                 "Print the amount of rupees entered in the withdraw/deposit prompt",
                                                  "Rupees in bank",                           $"{MajoraControlCode.RUPEES_IN_BANK}",                 "Print the amount of rupees deposited in the bank or won by betting",
                                                  "Bet Rupees Prompt",                        $"{MajoraControlCode.BET_RUPEES_PROMPT}",              "Print the rupee bet prompt",
                                                  "Lottery Number Prompt",                    $"{MajoraControlCode.LOTTERY_NUMBER_PROMPT}",          "Print the Lottery Number prompt",
                                                  "Bomber's Code Prompt",                     $"{MajoraControlCode.BOMBER_CODE_PROMPT}",             "Print the Bomber's Code prompt",
                                                  "Item prompt",                              $"{MajoraControlCode.ITEM_PROMPT}",                    "Used in the Open-Menu-And-Choose-An-Item Message",
                                                  "Song of Soaring Destination",              $"{MajoraControlCode.SOARING_DESTINATION}",            "Print Song of Soaring destination chosen",
                };

                AddTagControlsToMenu(PromptTagMenu, Prompts);
                ControlTagsMenu.Items.Add(PromptTagMenu);


                MenuItem CompletionTagMenu = new MenuItem() { Header = "Completion-related", ToolTip = "Tags relating to quest completion." };

                string[] Completion = new string[] { "Oceanside House Order",                    $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER}",           "Unused: print the entire Oceanside House Mask order",
                                                     "Oceanside House Order 1",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_1}",         "Print the first Oceanside House Mask color",
                                                     "Oceanside House Order 2",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_2}",         "Print the second Oceanside House Mask color",
                                                     "Oceanside House Order 3",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_3}",         "Print the third Oceanside House Mask color",
                                                     "Oceanside House Order 4",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_4}",         "Print the fourth Oceanside House Mask color",
                                                     "Oceanside House Order 5",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_5}",         "Print the fifth Oceanside House Mask color",
                                                     "Remaining Woodfall Fairies",               $"{MajoraControlCode.WOODFALL_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                     "Remaining Snowhead Fairies",               $"{MajoraControlCode.SNOWHEAD_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                     "Remaining Great Bay Fairies",              $"{MajoraControlCode.BAY_FAIRIES_REMAIN}",              "Print the amount of fairies left at the Great Bay Temple",
                                                     "Remaining Stone Tower Fairies",            $"{MajoraControlCode.IKANA_FAIRIES_REMAIN}",            "Print the amount of fairies left at the Stone Tower Temple",
                                                     "Bomber's Code",                            $"{MajoraControlCode.BOMBER_CODE}",                     "Print the Bomber\'s Code",
                };

                AddTagControlsToMenu(CompletionTagMenu, Completion);
                ControlTagsMenu.Items.Add(CompletionTagMenu);


                MenuItem SoundEffectMenu = new MenuItem() { Header = "Sound...", ToolTip = "Plays a sound effect." };
                SoundEffectMenu.Click += SoundEffectMenu_Click;


                string[] GenericTag = new string[] { "Null character",                       $"{MajoraControlCode.NULL_CHAR}",              "Prints nothing, causing the text routine to print out slower.",
                                                     "New textbox",                          $"{MajoraControlCode.NEW_BOX}",                "Starts a new message.",
                                                     "New textbox and center",               $"{MajoraControlCode.NEW_BOX_INCOMPL}",        "Starts a new message and ignores any extraneous linebreaks if the message has less than 4 lines",
                                                     "Reset cursor",                         $"{MajoraControlCode.RESET_CURSOR}",           "Used as a filler when there are fewer than four lines of text.",
                                                     "Offset",                               $"{MajoraControlCode.SHIFT}:0",                "Insert the specified number of spaces into the textbox.",
                                                     "No skip",                              $"{MajoraControlCode.NOSKIP}",                 "Disallows skipping the message box it's inserted into using the B button.",
                                                     "No skip with sound",                   $"{MajoraControlCode.NOSKIP_SOUND}",           "Disallows skipping the message box it's inserted into using the B button, and plays the 'sound finished' sound at the end.",
                                                     "Player name",                          $"{MajoraControlCode.PLAYER}",                 "Writes out the player's name (set on the file selection screen).",
                                                     "Draw instantly",                       $"{MajoraControlCode.DI}",                     "Prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                     "Draw per-character",                   $"{MajoraControlCode.DC}",                     "Prints whatever follows this tag one character at a time. This is the default typing mode.",
                                                     "Persistent",                           $"{MajoraControlCode.PERSISTENT}",             "Prevents the player from closing the textbox in any way. Used for shop descriptions.",
                                                     "Delay, then DI",                       $"{MajoraControlCode.DELAY_DI}",               "Inserts a pause in the text, and then prints whatever follows this tag one character at a time.",
                                                     "Delay, then DC",                       $"{MajoraControlCode.DELAY_DC}",               "Inserts a pause in the text, and then prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                     "Delay, then end",                      $"{MajoraControlCode.DELAY_END}",              "Inserts a pause in the text before closing the textbox.",
                                                     "Fade",                                 $"{MajoraControlCode.FADE}",                   "Waits for the specified number of frames until ending the textbox.",
                                                     "Two choices",                          $"{MajoraControlCode.TWO_CHOICES}",            "Displays a prompt which lets the player choose between two choices.",
                                                     "Three choices",                        $"{MajoraControlCode.THREE_CHOICES}",          "Displays a prompt which lets the player choose between three choices.",
                                                     "Item Value",                           $"{MajoraControlCode.ITEM_VALUE}",             "Displays the item value, taken from the message field.",
                                                     "End Conversation",                     $"{MajoraControlCode.END_CONVERSATION}",       "Should be used at the end of NPC message, otherwise they become impossible to talk to again.",
                                                   };

                AddTagControlsToMenu(ControlTagsMenu, GenericTag);
                textBoxMsg.ContextMenu.Items.Add(ControlTagsMenu);





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


                MenuItem HighScoreTagMenu = new MenuItem() { Header = "High Score", ToolTip = "Prints a player's high score." };

                string[] HighScores = new string[] { "Archery",                      $"{OcarinaControlCode.HIGH_SCORE}:ARCHERY",           "",
                                                     "Poe Salesman Points",          $"{OcarinaControlCode.HIGH_SCORE}:POE_POINTS",        "",
                                                     "Fish weight",                  $"{OcarinaControlCode.HIGH_SCORE}:FISHING",           "",
                                                     "Horse race time",              $"{OcarinaControlCode.HIGH_SCORE}:HORSE_RACE",        "",
                                                     "Running Man's marathon",       $"{OcarinaControlCode.HIGH_SCORE}:MARATHON",          "",
                                                     "Dampe race",                   $"{OcarinaControlCode.HIGH_SCORE}:DAMPE_RACE",        ""};

                AddTagControlsToMenu(HighScoreTagMenu, HighScores);
                ControlTagsMenu.Items.Add(HighScoreTagMenu);

                MenuItem ButtonTagMenu = new MenuItem() { Header = "Buttons", ToolTip = "Add a button icon to the textbox." };

                string[] Button = new string[] { "A Button",         $"{OcarinaControlCode.A_BUTTON}",           "",
                                                 "B Button",         $"{OcarinaControlCode.B_BUTTON}",           "",
                                                 "C Button",         $"{OcarinaControlCode.C_BUTTON}",           "",
                                                 "C-Up Button",      $"{OcarinaControlCode.C_UP}",               "",
                                                 "C-Down Button",    $"{OcarinaControlCode.C_DOWN}",             "",
                                                 "C-Left Button",    $"{OcarinaControlCode.C_LEFT}",             "",
                                                 "C-Right Button",   $"{OcarinaControlCode.C_RIGHT}",            "",
                                                 "L Button",         $"{OcarinaControlCode.L_BUTTON}",           "",
                                                 "R Button",         $"{OcarinaControlCode.R_BUTTON}",           "",
                                                 "Z Button",         $"{OcarinaControlCode.Z_BUTTON}",           "",
                                                 "Triangle",         $"{OcarinaControlCode.TRIANGLE}",           "",
                                                 "Control Stick",    $"{OcarinaControlCode.CONTROL_STICK}",      "",
                                                 "D-Pad",            $"{OcarinaControlCode.D_PAD}",              "",};

                AddTagControlsToMenu(ButtonTagMenu, Button);
                ControlTagsMenu.Items.Add(ButtonTagMenu);

                MenuItem ScoreTagMenu = new MenuItem() { Header = "Score", ToolTip = "Prints a player's score." };

                string[] Scores = new string[] { "Running Man's time",               $"{OcarinaControlCode.MARATHON_TIME}",    "Running Man's marathon time result.",
                                                 "Timer",                            $"{OcarinaControlCode.RACE_TIME}",        "Prints time shown on the last timer.",
                                                 "Archery points",                   $"{OcarinaControlCode.POINTS}",           "Horseback Archery points result.",
                                                 "Gold skulltulas",                  $"{OcarinaControlCode.GOLD_SKULLTULAS}",  "Current amount of Gold Skulltulas owned.",
                                                 "Fish weight",                      $"{OcarinaControlCode.FISH_WEIGHT}",      "Caught fish's weight.",
                                               };

                AddTagControlsToMenu(ScoreTagMenu, Scores);
                ControlTagsMenu.Items.Add(ScoreTagMenu);

                MenuItem IconTagMenu = new MenuItem() { Header = "Icon...", ToolTip = "Draws specified icon inside the textbox." };
                List<string> Icons = new List<string>();

                foreach (var Icon in Enum.GetValues(typeof(Enums.OcarinaIcon)))
                {
                    Icons.Add(Icon.ToString());
                    Icons.Add($"{OcarinaControlCode.ICON}:{Icon}");
                    Icons.Add("");
                }

                AddTagControlsToMenu(IconTagMenu, Icons.ToArray());
                ControlTagsMenu.Items.Add(IconTagMenu);

                MenuItem SoundEffectMenu = new MenuItem() { Header = "Sound...", ToolTip = "Plays a sound effect. Only one sound effect can be played per textbox." };
                SoundEffectMenu.Click += SoundEffectMenu_Click;

                string[] GenericTag = new string[] { "Delay",                   $"{OcarinaControlCode.DELAY}:0",            "Waits for the specified number of frames until switching to the next textbox.",
                                                     "Fade",                    $"{OcarinaControlCode.FADE}:0",             "Waits for the specified number of frames until ending the textbox.",
                                                     "Fade2",                   $"{OcarinaControlCode.FADE2}:0",            "Waits for the specified number of frames until ending the textbox. The duration can be made longer than with the FADE tag.",
                                                     "Offset",                  $"{OcarinaControlCode.SHIFT}:0",            "Insert the specified number of spaces into the textbox.",
                                                     "New textbox",             $"{OcarinaControlCode.NEW_BOX}",            "Starts a new message.",
                                                     "Jump",                    $"{OcarinaControlCode.JUMP}:0",             "Jumps to the specified message ID.",
                                                     "Player name",             $"{OcarinaControlCode.PLAYER}",             "Writes out the player's name (set on the file selection screen).",
                                                     "No skip",                 $"{OcarinaControlCode.NS}",                 "Disallows skipping the message box it's inserted into using the B button.",
                                                     "Speed",                   $"{OcarinaControlCode.SPEED}:0",            "Sets the amount of frames spent waiting between typing out each character.",
                                                     "Persistent",              $"{OcarinaControlCode.PERSISTENT}",         "Prevents the player from closing the textbox in any way. Used for shop descriptions.",
                                                     "Event",                   $"{OcarinaControlCode.EVENT}",              "Prevents the textbox from closing until a programmed event does so.",
                                                     "Background",              $"{OcarinaControlCode.BACKGROUND}:0",       "Used to draw the failure X whenever player plays a song wrong. The variable seems to control the color.",
                                                     "Draw instantly",          $"{OcarinaControlCode.DI}",                 "Prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                     "Draw per-character",      $"{OcarinaControlCode.DC}",                 "Prints whatever follows this tag one character at a time. This is the default typing mode.",
                                                     "Button prompt",           $"{OcarinaControlCode.AWAIT_BUTTON}",       "Waits until the player presses a button.",
                                                     "Two choices",             $"{OcarinaControlCode.TWO_CHOICES}",        "Displays a prompt which lets the player choose between two choices.",
                                                     "Three choices",           $"{OcarinaControlCode.THREE_CHOICES}",      "Displays a prompt which lets the player choose between three choices.",
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
            view.SelectedMessage.TextData = view.SelectedMessage.TextData.Insert(view.TextboxPosition, string.Format("<{0}>", s.Replace("_", " ")));
        }

        private void InsertControlCode(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            view.SelectedMessage.TextData = view.SelectedMessage.TextData.Insert(view.TextboxPosition, string.Format("<{0}>", ((sender as Control).Tag as string).Replace("_", " ")));
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
