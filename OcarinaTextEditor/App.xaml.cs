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
        public static Dictionary<char, char> charMap = new Dictionary<char, char>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            System.Windows.Forms.Application.EnableVisualStyles();


            if (File.Exists("CharMap.csv"))
            {
                string[] CharMap = File.ReadAllLines("CharMap.csv");

                foreach (string l in CharMap)
                {
                    string[] s = l.Split(';');

                    if (s.Length != 2)
                        continue;

                    charMap.Add(s[0].ToCharArray()[0], s[1].ToCharArray()[0]);
                }
            }
            else
                charMap = null;
        }
    }
}
