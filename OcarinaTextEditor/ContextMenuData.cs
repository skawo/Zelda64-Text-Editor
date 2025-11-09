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

        public static string[] MajoraColors = new string[] { "Default",   $"{ZeldaMsgPreview.MajoraMsgColor.D}",    "Will appear white in most cases, but black in 'None_White' and 'Bomber Notebook'-type textboxes. Will also appear black inside the Bomber's Notebook itself.",
                                                             "Red",       $"{ZeldaMsgPreview.MajoraMsgColor.R}",    "Appears orange in 'Wooden'-type textboxes",
                                                             "Green",     $"{ZeldaMsgPreview.MajoraMsgColor.G}",    "Inexplicably appears blue inside Bomber's Notebook",
                                                             "Blue",      $"{ZeldaMsgPreview.MajoraMsgColor.B}",    "",
                                                             "Yellow",    $"{ZeldaMsgPreview.MajoraMsgColor.Y}",    "",
                                                             "Navy",      $"{ZeldaMsgPreview.MajoraMsgColor.N}",    "",
                                                             "Silver",    $"{ZeldaMsgPreview.MajoraMsgColor.S}",    "",
                                                             "Orange",    $"{ZeldaMsgPreview.MajoraMsgColor.O}",    ""
        };

        public static string[] ButtonsMajora = new string[] { "A Button",         $"{ZeldaMsgPreview.MajoraControlCode.A_BUTTON}",           "",
                                                              "B Button",         $"{ZeldaMsgPreview.MajoraControlCode.B_BUTTON}",           "",
                                                              "C Button",         $"{ZeldaMsgPreview.MajoraControlCode.C_BUTTON}",           "",
                                                              "C-Up Button",      $"{ZeldaMsgPreview.MajoraControlCode.C_UP}",               "",
                                                              "C-Down Button",    $"{ZeldaMsgPreview.MajoraControlCode.C_DOWN}",             "",
                                                              "C-Left Button",    $"{ZeldaMsgPreview.MajoraControlCode.C_LEFT}",             "",
                                                              "C-Right Button",   $"{ZeldaMsgPreview.MajoraControlCode.C_RIGHT}",            "",
                                                              "L Button",         $"{ZeldaMsgPreview.MajoraControlCode.L_BUTTON}",           "",
                                                              "R Button",         $"{ZeldaMsgPreview.MajoraControlCode.R_BUTTON}",           "",
                                                              "Z Button",         $"{ZeldaMsgPreview.MajoraControlCode.Z_BUTTON}",           "",
                                                              "Triangle",         $"{ZeldaMsgPreview.MajoraControlCode.TRIANGLE}",           "",
                                                              "Control Stick",    $"{ZeldaMsgPreview.MajoraControlCode.CONTROL_STICK}",      "",
                                                              "D-Pad",            $"{ZeldaMsgPreview.MajoraControlCode.D_PAD}",              "Crashes the game"
        };



        public static string[] ScoresMajora = new string[] { "Required Swamp Cruise Hits",               $"{ZeldaMsgPreview.MajoraControlCode.HS_BOAT_ARCHERY}",                "Print the Swamp Cruise Archery High Score.",
                                                             "Stray Fairies",                            $"{ZeldaMsgPreview.MajoraControlCode.STRAY_FAIRIES}",                  "Print the current Stray Fairy collected in the current dungeon.",
                                                             "Gold Skulltulas",                          $"{ZeldaMsgPreview.MajoraControlCode.GOLD_SKULLTULAS}",                "Print the amount of Gold Skulltula tokens collected in the current spider house.",
                                                             "Postman Timer",                            $"{ZeldaMsgPreview.MajoraControlCode.TIMER_POSTMAN}",                  "Print the timer used in the postman sidequest.",
                                                             "Minigame Timer 1",                         $"{ZeldaMsgPreview.MajoraControlCode.TIMER_MINIGAME1}",                "Print the timer used in the Deku Butler Chase, Spirit House and Swamp Shooting Gallery.",
                                                             "Unused Timer",                             $"{ZeldaMsgPreview.MajoraControlCode.TIMER2}",                         "Print an unused timer.",
                                                             "Moon Crash Time Left",                     $"{ZeldaMsgPreview.MajoraControlCode.TIMER_MOON_CRASH}",               "Print the timer used for the final Moon Crash countdown.",
                                                             "Minigame Timer 2",                         $"{ZeldaMsgPreview.MajoraControlCode.TIMER_MINIGAME2}",                "Print timer used in the Deku Flying, Honey and Darling, Fisherman Jump, Gorman Race, Beaver Bros Race, Romani Archery, Goron Race, Treasure Gal Game and general room timers",
                                                             "Evironmental Hazard Timer",                $"{ZeldaMsgPreview.MajoraControlCode.TIMER_ENV_HAZARD}",               "Print remaining time until the player succumbs to an environmental hazard.",
                                                             "Current Time",                             $"{ZeldaMsgPreview.MajoraControlCode.TIME}",                           "Print current time.",
                                                             "Town Shooting Gallery High Score",         $"{ZeldaMsgPreview.MajoraControlCode.HS_TOWN_SHOOTING_GALLERY}",       "Print the Town Shooting Gallery High Score",
                                                             "Shooting Gallery Result",                  $"{ZeldaMsgPreview.MajoraControlCode.SHOOTING_GALLERY_RESULT}",        "Print score attained in the Shooting Gallery",
                                                             "Swamp Cruise Score",                       $"{ZeldaMsgPreview.MajoraControlCode.BOAT_ARCHERY_RESULT}",            "Print score attained in the Swamp Cruise",
                                                             "Winning Lottery Number",                   $"{ZeldaMsgPreview.MajoraControlCode.WINNING_LOTTERY_NUM}",            "Print the winning lottery number",
                                                             "Player's Lottery Number",                  $"{ZeldaMsgPreview.MajoraControlCode.PLAYER_LOTTERY_NUM}",             "Print the player's lottery number",
                                                             "Time remains",                             $"{ZeldaMsgPreview.MajoraControlCode.MOON_CRASH_TIME_REMAINS}",        "Print time remaining in hours & minutes ",
                                                             "Hours remain",                             $"{ZeldaMsgPreview.MajoraControlCode.MOON_CRASH_HOURS_REMAIN}",        "Print time remaining in hours",
                                                             "Hours remain until morning",               $"{ZeldaMsgPreview.MajoraControlCode.UNTIL_MORNING}",                  "Print time remaining until sunrise in hours & minutes",
                                                             "Romani Archery High Score",                $"{ZeldaMsgPreview.MajoraControlCode.HS_ROMANI_ARCHERY}",              "Print the Epona Archery high score (Romani Ranch Balloon Game)",
                                                             "Points (Tens)",                            $"{ZeldaMsgPreview.MajoraControlCode.POINTS_TENS}",                    "Print the minigame score up to 99. Unused, was meant to be used for the Fish Weight.",
                                                             "Points (Thousands)",                       $"{ZeldaMsgPreview.MajoraControlCode.POINTS_THOUSANDS}",               "Print the minigame score up to 9999. Unused.",
                                                             "Deku Flying Highscore 1",                  $"{ZeldaMsgPreview.MajoraControlCode.HS_DEKU_PLAYGROUND_DAY1}",        "Print the Deku Flying high score from Day 1",
                                                             "Deku Flying Highscore 2",                  $"{ZeldaMsgPreview.MajoraControlCode.HS_DEKU_PLAYGROUND_DAY2}",        "Print the Deku Flying high score from Day 2",
                                                             "Deku Flying Highscore 3",                  $"{ZeldaMsgPreview.MajoraControlCode.HS_DEKU_PLAYGROUND_DAY3}",        "Print the Deku Flying high score from Day 3",
                                                             "Deku Flying Name 1",                       $"{ZeldaMsgPreview.MajoraControlCode.DEKU_PLAYGROUND_PLAYER_DAY1}",    "Print the Player Name stored separately by Deku Flying on Day 1",
                                                             "Deku Flying Name 2",                       $"{ZeldaMsgPreview.MajoraControlCode.DEKU_PLAYGROUND_PLAYER_DAY2}",    "Print the Player Name stored separately by Deku Flying on Day 2",
                                                             "Deku Flying Name 3",                       $"{ZeldaMsgPreview.MajoraControlCode.DEKU_PLAYGROUND_PLAYER_DAY3}",    "Print the Player Name stored separately by Deku Flying on Day 3",
                                                             "Boat Archery High Score (Time?)",          $"{ZeldaMsgPreview.MajoraControlCode.HS_TIME_BOAT_ARCHERY}",           "Print the Boat Archery High Score as a timer...?",
                                                             "Romani Archery High Score (Time?)",        $"{ZeldaMsgPreview.MajoraControlCode.HS_TIME_ROMANI_ARCHERY}",         "Print the Romani Archery High Score as a timer...?",
                                                             "Player's Lottery Number (Time?)",          $"{ZeldaMsgPreview.MajoraControlCode.HS_TIME_PLAYER_LOTTERY}",         "Print the Player Lottery Ticket Number as a timer...?",
        };

        public static string[] PromptsMajora = new string[] { "Bank Prompt",                              $"{ZeldaMsgPreview.MajoraControlCode.BANK_PROMPT}",                    "Print the withdraw/deposit rupees prompt",
                                                              "Rupees Entered in Prompt",                 $"{ZeldaMsgPreview.MajoraControlCode.RUPEES_ENTERED}",                 "Print the amount of rupees entered in the withdraw/deposit prompt",
                                                              "Rupees in bank",                           $"{ZeldaMsgPreview.MajoraControlCode.RUPEES_IN_BANK}",                 "Print the amount of rupees deposited in the bank or won by betting",
                                                              "Bet Rupees Prompt",                        $"{ZeldaMsgPreview.MajoraControlCode.DOG_RACE_BET_PROMPT}",            "Print the Doggy Racetrack rupee bet prompt",
                                                              "Lottery Number Prompt",                    $"{ZeldaMsgPreview.MajoraControlCode.LOTTERY_NUMBER_PROMPT}",          "Print the Lottery Number prompt",
                                                              "Bomber's Code Prompt",                     $"{ZeldaMsgPreview.MajoraControlCode.BOMBER_CODE_PROMPT}",             "Print the Bomber's Code prompt",
                                                              "Item prompt",                              $"{ZeldaMsgPreview.MajoraControlCode.ITEM_PROMPT}",                    "Used in the Open-Menu-And-Choose-An-Item Message",
                                                              "Song of Soaring Destination",              $"{ZeldaMsgPreview.MajoraControlCode.SOARING_DESTINATION}",            "Print the chosen Song of Soaring destination"
        };

        public static string[] CompletionMajora = new string[] { "Oceanside House Order",                    $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER}",           "Unused: print the entire Oceanside House Mask order",
                                                                 "Oceanside House Order 1",                  $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER_1}",         "Print the first Oceanside House Mask color",
                                                                 "Oceanside House Order 2",                  $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER_2}",         "Print the second Oceanside House Mask color",
                                                                 "Oceanside House Order 3",                  $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER_3}",         "Print the third Oceanside House Mask color",
                                                                 "Oceanside House Order 4",                  $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER_4}",         "Print the fourth Oceanside House Mask color",
                                                                 "Oceanside House Order 5",                  $"{ZeldaMsgPreview.MajoraControlCode.OCEANSIDE_HOUSE_ORDER_5}",         "Print the fifth Oceanside House Mask color",
                                                                 "Remaining Woodfall Fairies",               $"{ZeldaMsgPreview.MajoraControlCode.WOODFALL_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                                 "Remaining Snowhead Fairies",               $"{ZeldaMsgPreview.MajoraControlCode.SNOWHEAD_FAIRIES_REMAIN}",         "Print the amount of fairies left at the Woodfall Temple",
                                                                 "Remaining Great Bay Fairies",              $"{ZeldaMsgPreview.MajoraControlCode.BAY_FAIRIES_REMAIN}",              "Print the amount of fairies left at the Great Bay Temple",
                                                                 "Remaining Stone Tower Fairies",            $"{ZeldaMsgPreview.MajoraControlCode.IKANA_FAIRIES_REMAIN}",            "Print the amount of fairies left at the Stone Tower Temple",
                                                                 "Bomber's Code",                            $"{ZeldaMsgPreview.MajoraControlCode.BOMBER_CODE}",                     "Print the Bomber\'s Code",
                                                                 "Time Speed",                               $"{ZeldaMsgPreview.MajoraControlCode.TIME_SPEED}",                      "Print the current time speed (affected by Reverse Song of Time)"
       };


        public static string[] GenericTagMajora = new string[] { "(Broken) Text Speed",                  $"{ZeldaMsgPreview.MajoraControlCode.TEXT_SPEED}",             "Meant to be like the OoT TEXT_SPEED tag, but it doesn't work due to a bug.",
                                                                 "New textbox",                          $"{ZeldaMsgPreview.MajoraControlCode.NEW_BOX}",                "Starts a new message.",
                                                                 "New textbox and center",               $"{ZeldaMsgPreview.MajoraControlCode.NEW_BOX_CENTER}",         "Starts a new message and ignores any extraneous linebreaks if the message has less than 4 lines",
                                                                 "Carriage return",                      $"{ZeldaMsgPreview.MajoraControlCode.CR}",                     "Used as a filler when there are fewer than four lines of text.",
                                                                 "Offset",                               $"{ZeldaMsgPreview.MajoraControlCode.SHIFT}:0",                "Insert the specified number of spaces into the textbox.",
                                                                 "Continue",                             $"{ZeldaMsgPreview.MajoraControlCode.CONTINUE}",               "Used at the end of messages that define the next message ID to jump tp.",
                                                                 "Player name",                          $"{ZeldaMsgPreview.MajoraControlCode.PLAYER}",                 "Writes out the player's name (set on the file selection screen).",
                                                                 "Draw instantly",                       $"{ZeldaMsgPreview.MajoraControlCode.DI}",                     "Prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                                 "Draw per-character",                   $"{ZeldaMsgPreview.MajoraControlCode.DC}",                     "Prints whatever follows this tag one character at a time. This is the default typing mode.",
                                                                 "Persistent",                           $"{ZeldaMsgPreview.MajoraControlCode.PERSISTENT}",             "Prevents the player from closing the textbox in any way. Used for shop descriptions.",
                                                                 "Delay, then new textbox",              $"{ZeldaMsgPreview.MajoraControlCode.NEW_BOX_DELAY}:0",        "Inserts a pause in the text, then opens new textbox.",
                                                                 "Delay",                                $"{ZeldaMsgPreview.MajoraControlCode.DELAY}:0",                "Inserts a pause in the text.",
                                                                 "Skippable Fade",                       $"{ZeldaMsgPreview.MajoraControlCode.FADE_SKIPPABLE}:0",       "Waits for the specified number of frames until ending the textbox, but the message can be closed instantly by pressing A.",
                                                                 "Fade",                                 $"{ZeldaMsgPreview.MajoraControlCode.FADE}:0",                 "Waits for the specified number of frames until ending the textbox.",
                                                                 "Two choices",                          $"{ZeldaMsgPreview.MajoraControlCode.TWO_CHOICES}",            "Displays a prompt which lets the player choose between two choices.",
                                                                 "Three choices",                        $"{ZeldaMsgPreview.MajoraControlCode.THREE_CHOICES}",          "Displays a prompt which lets the player choose between three choices.",
                                                                 "Item Value",                           $"{ZeldaMsgPreview.MajoraControlCode.ITEM_VALUE}",             "Displays the item value, taken from the message field.",
                                                                 "Event",                                $"{ZeldaMsgPreview.MajoraControlCode.EVENT}",                  "Used in select NPC messages. Message ends with a TRIANGLE Ending Marker.",
                                                                 "End Event",                            $"{ZeldaMsgPreview.MajoraControlCode.EVENT_END}",              "Used at the end of select NPC messages - otherwise they become impossible to talk to again. Message ends with a SQUARE Ending Marker."

        };






        public static string[] ColorsOcarina = new string[] { "Default",        $"{ZeldaMsgPreview.OcarinaMsgColor.D}",         "Will appear black in 'None_White'-type textboxes, white otherwise",
                                                              "Red",            $"{ZeldaMsgPreview.OcarinaMsgColor.R}",         "Appears orange in 'Wooden'-type textboxes",
                                                              "Green",          $"{ZeldaMsgPreview.OcarinaMsgColor.G}",         "",
                                                              "Blue",           $"{ZeldaMsgPreview.OcarinaMsgColor.B}",         "",
                                                              "Cyan",           $"{ZeldaMsgPreview.OcarinaMsgColor.C}",         "",
                                                              "Magenta",        $"{ZeldaMsgPreview.OcarinaMsgColor.M}",         "",
                                                              "Yellow",         $"{ZeldaMsgPreview.OcarinaMsgColor.Y}",         "",
                                                              "Black",          $"{ZeldaMsgPreview.OcarinaMsgColor.BLK}",       ""
        };


        public static string[] HighScoresOcarina = new string[] { "Archery",                      $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.ARCHERY}",           "",
                                                                  "Poe Salesman Points",          $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.POE_POINTS}",        "",
                                                                  "Fish weight",                  $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.FISHING}",           "",
                                                                  "Horse race time",              $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.HORSE_RACE}",        "",
                                                                  "Running Man's marathon",       $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.MARATHON}",          "",
                                                                  "Dampe race",                   $"{ZeldaMsgPreview.OcarinaControlCode.HIGH_SCORE}:{ZeldaMsgPreview.OcarinaHighScore.DAMPE_RACE}",        ""
        };

        public static string[] ButtonsOcarina = new string[] { "A Button",         $"{ZeldaMsgPreview.OcarinaControlCode.A_BUTTON}",           "",
                                                               "B Button",         $"{ZeldaMsgPreview.OcarinaControlCode.B_BUTTON}",           "",
                                                               "C Button",         $"{ZeldaMsgPreview.OcarinaControlCode.C_BUTTON}",           "",
                                                               "C-Up Button",      $"{ZeldaMsgPreview.OcarinaControlCode.C_UP}",               "",
                                                               "C-Down Button",    $"{ZeldaMsgPreview.OcarinaControlCode.C_DOWN}",             "",
                                                               "C-Left Button",    $"{ZeldaMsgPreview.OcarinaControlCode.C_LEFT}",             "",
                                                               "C-Right Button",   $"{ZeldaMsgPreview.OcarinaControlCode.C_RIGHT}",            "",
                                                               "L Button",         $"{ZeldaMsgPreview.OcarinaControlCode.L_BUTTON}",           "",
                                                               "R Button",         $"{ZeldaMsgPreview.OcarinaControlCode.R_BUTTON}",           "",
                                                               "Z Button",         $"{ZeldaMsgPreview.OcarinaControlCode.Z_BUTTON}",           "",
                                                               "Triangle",         $"{ZeldaMsgPreview.OcarinaControlCode.TRIANGLE}",           "",
                                                               "Control Stick",    $"{ZeldaMsgPreview.OcarinaControlCode.CONTROL_STICK}",      "",
                                                               "D-Pad",            $"{ZeldaMsgPreview.OcarinaControlCode.D_PAD}",              ""
        };

        public static string[] ScoresOcarina = new string[] { "Running Man's time",               $"{ZeldaMsgPreview.OcarinaControlCode.MARATHON_TIME}",    "Running Man's marathon time result.",
                                                              "Timer",                            $"{ZeldaMsgPreview.OcarinaControlCode.RACE_TIME}",        "Prints time shown on the last timer.",
                                                              "Archery points",                   $"{ZeldaMsgPreview.OcarinaControlCode.POINTS}",           "Horseback Archery points result.",
                                                              "Gold skulltulas",                  $"{ZeldaMsgPreview.OcarinaControlCode.GOLD_SKULLTULAS}",  "Current amount of Gold Skulltulas owned.",
                                                              "Fish weight",                      $"{ZeldaMsgPreview.OcarinaControlCode.FISH_WEIGHT}",      "Caught fish's weight."
        };

        public static string[] GenericTagOcarina = new string[] { "Delay",                   $"{ZeldaMsgPreview.OcarinaControlCode.DELAY}:0",            "Waits for the specified number of frames until switching to the next textbox.",
                                                                  "Fade",                    $"{ZeldaMsgPreview.OcarinaControlCode.FADE}:0",             "Waits for the specified number of frames until ending the textbox.",
                                                                  "Fade2",                   $"{ZeldaMsgPreview.OcarinaControlCode.FADE2}:0",            "Waits for the specified number of frames until ending the textbox. The duration can be made longer than with the FADE tag.",
                                                                  "Offset",                  $"{ZeldaMsgPreview.OcarinaControlCode.SHIFT}:0",            "Insert the specified number of spaces into the textbox.",
                                                                  "New textbox",             $"{ZeldaMsgPreview.OcarinaControlCode.NEW_BOX}",            "Starts a new message.",
                                                                  "Jump",                    $"{ZeldaMsgPreview.OcarinaControlCode.JUMP}:0",             "Jumps to the specified message ID.",
                                                                  "Player name",             $"{ZeldaMsgPreview.OcarinaControlCode.PLAYER}",             "Writes out the player's name (set on the file selection screen).",
                                                                  "No skip",                 $"{ZeldaMsgPreview.OcarinaControlCode.NS}",                 "Disallows skipping the message box it's inserted into using the B button.",
                                                                  "Speed",                   $"{ZeldaMsgPreview.OcarinaControlCode.SPEED}:0",            "Sets the amount of frames spent waiting between typing out each character.",
                                                                  "Persistent",              $"{ZeldaMsgPreview.OcarinaControlCode.PERSISTENT}",         "Prevents the player from closing the textbox in any way. Used for shop descriptions.",
                                                                  "Event",                   $"{ZeldaMsgPreview.OcarinaControlCode.EVENT}",              "Prevents the textbox from closing until a programmed event does so.",
                                                                  "Background",              $"{ZeldaMsgPreview.OcarinaControlCode.BACKGROUND}:0",       "Used to draw the failure X whenever player plays a song wrong. The variable seems to control the color.",
                                                                  "Draw instantly",          $"{ZeldaMsgPreview.OcarinaControlCode.DI}",                 "Prints whatever follows this tag instantly until a Draw-Per-Character tag is present.",
                                                                  "Draw per-character",      $"{ZeldaMsgPreview.OcarinaControlCode.DC}",                 "Prints whatever follows this tag one character at a time. This is the default typing mode.",
                                                                  "Button prompt",           $"{ZeldaMsgPreview.OcarinaControlCode.AWAIT_BUTTON}",       "Waits until the player presses a button.",
                                                                  "Two choices",             $"{ZeldaMsgPreview.OcarinaControlCode.TWO_CHOICES}",        "Displays a prompt which lets the player choose between two choices.",
                                                                  "Three choices",           $"{ZeldaMsgPreview.OcarinaControlCode.THREE_CHOICES}",      "Displays a prompt which lets the player choose between three choices."
        };


    }
}
