using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GVideo
{
    class Global
    {
        public const String BATNAME = "temp.bat";
        public const String TRACKSINFO = "tracks.text";
        public static string ph;
        public static String batPh;

        static Global() {
            ph = Environment.CommandLine;
            ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
            if (ph[0] == '"')
                ph = ph.Substring(1);
            batPh = ph + BATNAME;
        }

        public static String[] SplitFilePathName(String fileString) {
            String[] result = new String[5];
            Match m = Regex.Match(fileString, @"([\s\S]+\\)(([\s\S]*?\[)\d{2}(][\s\S]*?)(\.[\S]*))$");
            if (!m.Success) {
                return null;
            }
            //path
            result[0] = m.Groups[1].Value;
            //three parts of name
            result[1] = m.Groups[3].Value;
            result[2] = m.Groups[4].Value;
            result[3] = m.Groups[5].Value;
            //name
            result[4] = m.Groups[2].Value;
            return result;
        }
    }
}
