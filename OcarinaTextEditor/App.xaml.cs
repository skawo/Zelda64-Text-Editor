using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace Zelda64TextEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public static Dictionary<char, char> charMap = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            System.Windows.Forms.Application.EnableVisualStyles();


            if (File.Exists("codepoint.txt"))
            {
                string[] codePoint = File.ReadAllLines("codepoint.txt");

                if (codePoint.Length >= 145)
                {
                    charMap = new Dictionary<char, char>();

                    for (int i = 2; i < 145; i++)
                    {
                        if (codePoint.Length >= 1)
                            charMap.Add(codePoint[i].ToCharArray()[0], (char)(0x20 + i - 1));
                        else
                            charMap.Add((char)(0x20 + i - 1), (char)(0x20 + i - 1));
                    }
                }
            }

            if (File.Exists("CharMap.csv"))
            {
                string[] CharMap = File.ReadAllLines("CharMap.csv");
                charMap = new Dictionary<char, char>();

                foreach (string l in CharMap)
                {
                    string[] s = l.Split(';');

                    if (s.Length != 2)
                        continue;

                    charMap.Add(s[0].ToCharArray()[0], s[1].ToCharArray()[0]);
                }
            }

        }
    }
}
