using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zelda64TextEditor.Enums;

namespace Zelda64TextEditor
{
    public static class ContextMenuData
    {

        public static string[] MajoraColors = new string[] { "Default",   $"{MajoraMsgColor.D}",    "Will appear white in most cases, but black in 'None_White' and 'Bomber Notebook'-type textboxes. Will also appear black inside the Bomber's Notebook itself.",
                                                             "Red",       $"{MajoraMsgColor.R}",    "Appears orange in 'Wooden'-type textboxes",
                                                             "Green",     $"{MajoraMsgColor.G}",    "Inexplicably appears blue inside Bomber's Notebook",
                                                             "Blue",      $"{MajoraMsgColor.B}",    "",
                                                             "Yellow",    $"{MajoraMsgColor.Y}",    "",
                                                             "Navy",      $"{MajoraMsgColor.N}",    "",
                                                             "Silver",    $"{MajoraMsgColor.S}",    "",
                                                             "Orange",    $"{MajoraMsgColor.O}",    ""
        };


        public static string[] ButtonsMajora = new string[] { "A Button",         $"{MajoraControlCode.A_BUTTON}",           "",
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
                                                              "D-Pad",            $"{MajoraControlCode.D_PAD}",              "Crashes the game"
        };



        public static string[] ScoresMajora = new string[] { "Required Swamp Cruise Hits",               $"{MajoraControlCode.SWAMP_CRUISE_HITS}",              "Print the number of hits required to win the Swamp Cruise reward.",
                                                             "Stray Fairies",                            $"{MajoraControlCode.STRAY_FAIRY_SCORE}",              "Print the amount of Stray Fairies collected in the current dungeon.",
                                                             "Gold Skulltulas",                          $"{MajoraControlCode.GOLD_SKULLTULAS}",                "Print the amount of Gold Skulltula tokens collected in the current spider house.",
                                                             "Postman Minigame Time",                    $"{MajoraControlCode.POSTMAN_RESULTS}",                "Print the time score attained in the postman minigame.",
                                                             "Timer",                                    $"{MajoraControlCode.TIMER}",                          "Print the time shown on the last timer.",
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
                                                             "Fish weight",                              $"{MajoraControlCode.FISH_WEIGHT}",                    "Print the caught fish's weight. Unused two-digit minigame score.",
                                                             "Deku Flying Highscore 1",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY1}",            "Print the Deku Flying high score from Day 1)",
                                                             "Deku Flying Highscore 2",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY2}",            "Print the Deku Flying high score from Day 2)",
                                                             "Deku Flying Highscore 3",                  $"{MajoraControlCode.DEKU_HIGHSCORE_DAY3}",            "Print the Deku Flying high score from Day 3)"
        };

        public static string[] PromptsMajora = new string[] { "Bank Prompt",                              $"{MajoraControlCode.BANK_PROMPT}",                    "Print the withdraw/deposit rupees prompt",
                                                              "Rupees Entered in Prompt",                 $"{MajoraControlCode.RUPEES_ENTERED}",                 "Print the amount of rupees entered in the withdraw/deposit prompt",
                                                              "Rupees in bank",                           $"{MajoraControlCode.RUPEES_IN_BANK}",                 "Print the amount of rupees deposited in the bank or won by betting",
                                                              "Bet Rupees Prompt",                        $"{MajoraControlCode.BET_RUPEES_PROMPT}",              "Print the rupee bet prompt",
                                                              "Lottery Number Prompt",                    $"{MajoraControlCode.LOTTERY_NUMBER_PROMPT}",          "Print the Lottery Number prompt",
                                                              "Bomber's Code Prompt",                     $"{MajoraControlCode.BOMBER_CODE_PROMPT}",             "Print the Bomber's Code prompt",
                                                              "Item prompt",                              $"{MajoraControlCode.ITEM_PROMPT}",                    "Used in the Open-Menu-And-Choose-An-Item Message",
                                                              "Song of Soaring Destination",              $"{MajoraControlCode.SOARING_DESTINATION}",            "Print the chosen Song of Soaring destination"
        };

        public static string[] CompletionMajora = new string[] { "Oceanside House Order",                    $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER}",           "Unused: print the entire Oceanside House Mask order",
                                                                 "Oceanside House Order 1",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_1}",         "Print the first Oceanside House Mask color",
                                                                 "Oceanside House Order 2",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_2}",         "Print the second Oceanside House Mask color",
                                                                 "Oceanside House Order 3",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_3}",         "Print the third Oceanside House Mask color",
                                                                 "Oceanside House Order 4",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_4}",         "Print the fourth Oceanside House Mask color",
                                                                 "Oceanside House Order 5",                  $"{MajoraControlCode.OCEANSIDE_HOUSE_ORDER_5}",         "Print the fifth Oceanside House Mask color",
                                                                 "Remaining Woodfall Fairies",               $"{MajoraControlCode.WOODFALL_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                                 "Remaining Snowhead Fairies",               $"{MajoraControlCode.SNOWHEAD_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                                 "Remaining Great Bay Fairies",              $"{MajoraControlCode.BAY_FAIRIES_REMAIN}",              "Print the amount of fairies left at the Great Bay Temple",
                                                                 "Remaining Stone Tower Fairies",            $"{MajoraControlCode.IKANA_FAIRIES_REMAIN}",            "Print the amount of fairies left at the Stone Tower Temple",
                                                                 "Bomber's Code",                            $"{MajoraControlCode.BOMBER_CODE}",                     "Print the Bomber\'s Code"
       };


        public static string[] GenericTagMajora = new string[] { "Null character",                       $"{MajoraControlCode.NULL_CHAR}",              "Prints nothing, causing the text routine to print out slower.",
                                                                 "New textbox",                          $"{MajoraControlCode.NEW_BOX}",                "Starts a new message.",
                                                                 "New textbox and center",               $"{MajoraControlCode.NEW_BOX_CENTER}",         "Starts a new message and ignores any extraneous linebreaks if the message has less than 4 lines",
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
                                                                 "End Conversation",                     $"{MajoraControlCode.END_CONVERSATION}",       "Should be used at the end of NPC message, otherwise they become impossible to talk to again."

        };






        public static string[] ColorsOcarina = new string[] { "White",          $"{OcarinaMsgColor.W}",         "Will appear black in 'None_White'-type textboxes",
                                                              "Red",            $"{OcarinaMsgColor.R}",         "Appears orange in 'Wooden'-type textboxes",
                                                              "Green",          $"{OcarinaMsgColor.G}",         "",
                                                              "Blue",           $"{OcarinaMsgColor.B}",         "",
                                                              "Cyan",           $"{OcarinaMsgColor.C}",         "",
                                                              "Magenta",        $"{OcarinaMsgColor.M}",         "",
                                                              "Yellow",         $"{OcarinaMsgColor.Y}",         "",
                                                              "Black",          $"{OcarinaMsgColor.BLK}",       ""
        };


        public static string[] HighScoresOcarina = new string[] { "Archery",                      $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.ARCHERY}",           "",
                                                                  "Poe Salesman Points",          $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.POE_POINTS}",        "",
                                                                  "Fish weight",                  $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.FISH_WEIGHT}",       "",
                                                                  "Horse race time",              $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.HORSE_RACE}",        "",
                                                                  "Running Man's marathon",       $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.MARATHON}",          "",
                                                                  "Dampe race",                   $"{OcarinaControlCode.HIGH_SCORE}:{OcarinaHighScore.DAMPE_RACE}",        ""
        };

        public static string[] ButtonsOcarina = new string[] { "A Button",         $"{OcarinaControlCode.A_BUTTON}",           "",
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
                                                               "D-Pad",            $"{OcarinaControlCode.D_PAD}",              ""
        };

        public static string[] ScoresOcarina = new string[] { "Running Man's time",               $"{OcarinaControlCode.MARATHON_TIME}",    "Running Man's marathon time result.",
                                                              "Timer",                            $"{OcarinaControlCode.RACE_TIME}",        "Prints time shown on the last timer.",
                                                              "Archery points",                   $"{OcarinaControlCode.POINTS}",           "Horseback Archery points result.",
                                                              "Gold skulltulas",                  $"{OcarinaControlCode.GOLD_SKULLTULAS}",  "Current amount of Gold Skulltulas owned.",
                                                              "Fish weight",                      $"{OcarinaControlCode.FISH_WEIGHT}",      "Caught fish's weight."
        };

        public static string[] GenericTagOcarina = new string[] { "Delay",                   $"{OcarinaControlCode.DELAY}:0",            "Waits for the specified number of frames until switching to the next textbox.",
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
                                                                  "Three choices",           $"{OcarinaControlCode.THREE_CHOICES}",      "Displays a prompt which lets the player choose between three choices."
        };


    }
}
