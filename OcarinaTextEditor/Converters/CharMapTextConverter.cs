using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda64TextEditor.Converters
{
    public static class CharMapTextConverter
    {
        public static string RemapTextFrom(string s)
        {
            string text = "";
            bool Skip = false;


            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '<')
                    Skip = true;

                if (Skip)
                    text += s[i];
                else
                    if (App.charMap.ContainsKey(s[i]))
                        text += App.charMap[s[i]];
                else
                    text += s[i];

                if (s[i] == '>')
                    Skip = false;
            }

            return text;
        }

        public static string RemapTextTo(string s)
        {
            string text = "";
            bool Skip = false;


            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '<')
                    Skip = true;

                if (Skip)
                    text += s[i];
                else
                    if (App.charMap.ContainsValue(s[i]))
                    text += App.charMap.First(x => x.Value == s[i]).Key;
                else
                    text += s[i];

                if (s[i] == '>')
                    Skip = false;
            }

            return text;
        }

    }
}
