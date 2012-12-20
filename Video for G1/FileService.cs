using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace Video_for_G1
{
    public static class FileService
    {
        static String[] exeFile = new String[] { "x264.exe", "ffmpeg.exe", "neroAacEnc.exe" };
        static String[] extractExeFile = new String[] { "mkvextract.exe", "mkvinfo.exe" };
        public static void checkExe() {
            for (int i = 0; i < 3; i++) {
                string exePath = Global.ph + exeFile[i];
                if (!File.Exists(exePath)) {
                    MessageBox.Show(exeFile[i] + " not exist!");
                    System.Environment.Exit(0);
                }
            }

            for (int i = 0; i < 2; i++) {
                string exePath = Global.ph + extractExeFile[i];
                if (!File.Exists(exePath)) {
                    MessageBox.Show(extractExeFile[i] + " not exist!", "Extract can not work!");
                }
            }
        }

        //--tune animation --crf 23
        public static void createBat(String pathFile, String options, String output) {
            String pathName = " \"" + output + pathFile.Substring(pathFile.LastIndexOf('\\'),
                pathFile.LastIndexOf('.') - pathFile.LastIndexOf('\\'));
            string vo = pathName + "_v.mp4\" ";
            string ao = pathName + "_a.m4a\" ";
            string avo = pathName + "_enc.mp4\" ";
            string vi = " \"" + pathFile + "\" ";
            bool isAvs = pathFile.EndsWith(".avs");

            if (File.Exists(Global.batPh)) {
                File.Delete(Global.batPh);
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("x264 --profile baseline --level 3 " + options
                + " --vbv-bufsize 2500 --vbv-maxrate 2500 "
                + (isAvs ? "" : "--vf resize:480,320,,both,,spline")
                + " -o " + vo + vi);
            if (!isAvs) {
                sBuilder.AppendLine("ffmpeg -i " + vi + " -f wav - | neroaacenc -q 0.28 -if - -ignorelength -of " + ao);
                sBuilder.AppendLine("ffmpeg -i " + vo + " -i " + ao + " -vcodec copy -acodec copy " + avo);
                sBuilder.AppendLine("del " + vo);
                sBuilder.AppendLine("del " + ao);
            }

            using (StreamWriter sw = new StreamWriter(Global.batPh, false, Encoding.Default)) {
                sw.Write(sBuilder.ToString());
            }
        }

        internal static void deleteBat() {
            if (File.Exists(Global.batPh)) {
                File.Delete(Global.batPh);
            }
        }
    }
}
