using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Video_for_G1
{
    public static class FileService
    {
        static String[] exeFile = new String[] { "x264.exe", "ffmpeg.exe", "neroAacEnc.exe" };
        static String[] mkvExeFile = new String[] { "mkvextract.exe", "mkvinfo.exe" };
        const String reFileNamePatternFile = "ReFileName.txt";
        public static bool checkExe() {
            for (int i = 0; i < 3; i++) {
                string exePath = Global.ph + exeFile[i];
                if (!File.Exists(exePath)) {
                    MessageBox.Show(exeFile[i] + " not exist!");
                    System.Environment.Exit(0);
                }
            }
            bool hasExtract = true;
            for (int i = 0; i < 2; i++) {
                string exePath = Global.ph + mkvExeFile[i];
                if (!File.Exists(exePath)) {
                    MessageBox.Show(mkvExeFile[i] + " not exist!", "Extract can not work!");
                    hasExtract = false;
                }
            }
            return hasExtract;
        }

        public static List<String> getReFileNamePattern() {
            if (!File.Exists(reFileNamePatternFile)) {
                File.CreateText(reFileNamePatternFile);
                return null;
            }
            List<String> pattens = new List<String>();
            using (StreamReader sr = new StreamReader(reFileNamePatternFile)) {
                while (!sr.EndOfStream) {
                    pattens.Add(sr.ReadLine());
                }
            }
            return pattens;
        }

        public static void reFileName(String file) {
            if (!File.Exists(file)) {
                return;
            }
            List<String> patterns = getReFileNamePattern();
            if (patterns == null || patterns.Count == 0) {
                return;
            }
            String[] fileParts = Global.SplitFilePathName(file);
            String filePath = fileParts[0];
            String fileName = fileParts[4];
            String newFileName = fileName;
            foreach (String pattern in patterns) {
                newFileName = Regex.Replace(newFileName, pattern, "");
            }
            File.Move(filePath + fileName, filePath + newFileName);
        }
        //--tune animation --crf 23
        public static String createBat(String pathFile, String options, String output) {
            String pathName = " \"" + output + pathFile.Substring(pathFile.LastIndexOf('\\'),
                pathFile.LastIndexOf('.') - pathFile.LastIndexOf('\\'));
            bool isAvs = pathFile.EndsWith(".avs");
            bool isBat = pathFile.EndsWith(".bat");
            string vo = pathName + (isAvs ? "" : "_v") + ".mp4\" ";
            string ao = pathName + "_a.m4a\" ";
            string avo = pathName + "_enc.mp4\" ";
            string vi = " \"" + pathFile + "\" ";

            if (File.Exists(Global.batPh)) {
                File.Delete(Global.batPh);
            }
            if (isBat) {
                File.Copy(pathFile, Global.batPh, true);
            } else {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.AppendLine("x264 --profile baseline --level 3 " + options
                    + " --vbv-bufsize 2500 --vbv-maxrate 2500 "
                    + (isAvs ? "" : "--vf resize:480,320,,both,,spline")
                    + " -o " + vo + vi);
                if (!isAvs) {
                    sBuilder.AppendLine("ffmpeg -i " + vi + " -ar 48000 -f wav - | neroaacenc -q 0.28 -if - -ignorelength -of " + ao);
                    sBuilder.AppendLine("ffmpeg -i " + vo + " -i " + ao + " -vcodec copy -acodec copy " + avo);
                    sBuilder.AppendLine("del " + vo);
                    sBuilder.AppendLine("del " + ao);
                }

                using (StreamWriter sw = new StreamWriter(Global.batPh, false, Encoding.Default)) {
                    sw.Write(sBuilder.ToString());
                }
            }
            String outFile = avo.Substring(2);
            outFile = outFile.Remove(outFile.LastIndexOf('\"'));
            return outFile;
        }

        internal static void deleteBat() {
            if (File.Exists(Global.batPh)) {
                File.Delete(Global.batPh);
            }
        }
    }
}
