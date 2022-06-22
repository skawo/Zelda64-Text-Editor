using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcarinaTextEditor
{
    public static class Dicts
    {
        public static string SFXesFilename = $"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}/SFX.csv";
        public static Dictionary<string, int> SFXes = GetDictionary(SFXesFilename);

        public static void ReloadDict(string Filename, ref Dictionary<string, int> Dict)
        {
            Dict = GetDictionary(Filename);
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
