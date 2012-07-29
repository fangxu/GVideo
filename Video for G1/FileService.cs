using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace Video_for_G1
{
    public class FileService
    {
        static String[] exeFile = new String[] { "x264.exe", "ffmpeg.exe", "neroAacEnc.exe" };

        public static void checkExe()
        {
            string ph = Environment.CommandLine;//check x264.exe , ffmpeg.exe and neroaacenc.exe exist.            
            ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
            if (ph[0] == '"')
                ph = ph.Substring(1);
            
            for (int i = 0; i < 3; i++)
            {
                string exePath = ph + exeFile[i];
                if (!File.Exists(exePath))
                {
                    MessageBox.Show(exeFile[i] + " not exist!");
                    System.Environment.Exit(0);
                }
            }
        }


        public static void createBat(String pathFile, String pathBat, String options, String output)
        {
            String pathName = " \"" + output + pathFile.Substring(pathFile.LastIndexOf('\\'), pathFile.LastIndexOf('.') - pathFile.LastIndexOf('\\'));
            string vo = pathName + "_v.mp4\" ";
            string ao = pathName + "_a.m4a\" ";
            string avo = pathName + "_enc.mp4\" ";
            string vi = " \"" + pathFile + "\" ";
            string strBatPath = pathBat + "temp.bat";
            if (File.Exists(strBatPath))
            {
                File.Delete(strBatPath);
            }
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("x264 " + options + " -o " + vo + vi);
            sBuilder.AppendLine("ffmpeg -i " + vi + " -f wav - | neroaacenc -q 0.28 -if - -ignorelength -of " + ao);
            sBuilder.AppendLine("ffmpeg -i " + vo + " -i " + ao + " -vcodec copy -acodec copy " + avo);
            sBuilder.AppendLine("del " + vo);
            sBuilder.AppendLine("del " + ao);
            StreamWriter sw = new StreamWriter(strBatPath, false, Encoding.Default);
            sw.Write(sBuilder.ToString());
            sw.Close();
        }
    }
}
