using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda64TextEditor
{
    public static class Dicts
    {
        public static string OoTSFXesFilename = $"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}/SFX_Ocarina.csv";
        public static string MMSFXesFilename = $"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}/SFX_Majora.csv";
        public static Dictionary<string, int> SFXes = GetDictionary(OoTSFXesFilename);

        public static void ReloadDict(string Filename, ref Dictionary<string, int> Dict)
        {
            Dict = GetDictionary(Filename);
        }

        public static void ReloadSfxesDict(string Filename, string hPath, ref Dictionary<string, int> Dict)
        {
            Dict = GetDictionary(Filename);

            if (hPath != "")
            {
                if (File.Exists(hPath))
                {
                    string[] lines = File.ReadAllLines(hPath);
                    ushort cnt = 0;

                    foreach (string line in lines)
                    {
                        string trimmedline = line.Trim().Replace(",", "");
                        if (trimmedline.Contains("SOUND_"))
                        {
                            Dict.Add(trimmedline, cnt + 0xF000);
                            cnt++;
                        }
                    }
                }
            }
        }

        public static Dictionary<string, int> GetDictionary(string Filename)
        {
            Dictionary<string, int> Dict = new Dictionary<string, int>();

            string OffendingRow = "";

            try
            {
                string[] RawData = File.ReadAllLines(Filename);

                foreach (string Row in RawData)
                {
                    OffendingRow = Row;
                    string[] NameAndID = Row.Split(',');
                    Dict.Add(NameAndID[1], Convert.ToInt32(NameAndID[0]));
                }

                return Dict;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");
                return Dict;
            }
        }
    }
}
