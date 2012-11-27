using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video_for_G1
{
    class Global
    {
        public const String BATNAME = "temp.bat";
        public static string ph;
        public static String batPh;

        static Global()
        {
            ph = Environment.CommandLine;
            ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
            if (ph[0] == '"')
                ph = ph.Substring(1);
            batPh = ph + BATNAME;
        }
    }
}
