using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Video_for_G1
{
    class VideoInfo
    {
        private string _Path;
        public string Path {
            get {
                return _Path;
            }
            set {
                _Path = value;
            }
        }
        public TimeSpan Duration { get; set; }
        public double BitRate { get; set; }
        public double VideoBitRate { get; set; }
        public double AudioBitRate { get; set; }
        public string RawAudioFormat { get; set; }
        public string AudioFormat { get; set; }
        public string RawVideoFormat { get; set; }
        public string VideoFormat { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double FrameRate { get; set; }
        public long TotalFrames { get; set; }
        public string RawInfo { get; set; }
        public bool infoGathered { get; set; }


        public VideoInfo(String path) {
            string Params = string.Format("-i \"{0}\"", path);
            string output = RunProcess(Params);

            RawInfo = output;
            Duration = ExtractDuration(RawInfo);
            BitRate = ExtractBitrate(RawInfo);
            RawAudioFormat = ExtractRawAudioFormat(RawInfo);
            AudioFormat = ExtractAudioFormat(RawAudioFormat);
            RawVideoFormat = ExtractRawVideoFormat(RawInfo);
            VideoFormat = ExtractVideoFormat(RawVideoFormat);
            Width = ExtractVideoWidth(RawInfo);
            Height = ExtractVideoHeight(RawInfo);
            FrameRate = ExtractFrameRate(RawVideoFormat);
            TotalFrames = ExtractTotalFrames(Duration, FrameRate);
            AudioBitRate = ExtractAudioBitRate(RawAudioFormat);
            VideoBitRate = ExtractVideoBitRate(RawVideoFormat);

            infoGathered = true;
        }

        private string RunProcess(string Parameters) {
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo("ffmpeg.exe", Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //Create the output
            string output = null;

            //try the process
            try {
                //run the process
                Process proc = System.Diagnostics.Process.Start(oInfo);

                //now put it in a string
                //This needs to be before WaitForExit() to prevent deadlock, for details: http://msdn.microsoft.com/en-us/library/system.diagnostics.process.standardoutput%28v=VS.80%29.aspx
                output = proc.StandardError.ReadToEnd();

                //Wait for exit
                proc.WaitForExit();

                //Release resources
                proc.Close();
            } catch (Exception) {
                output = string.Empty;
            }

            return output;
        }

        #region Extraction methods
        private TimeSpan ExtractDuration(string rawInfo) {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success) {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4) {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]),
                        Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]),
                        Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }
        private double ExtractBitrate(string rawInfo) {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success) {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }
        private string ExtractRawAudioFormat(string rawInfo) {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success) {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }
        private string ExtractAudioFormat(string rawAudioFormat) {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }
        private string ExtractRawVideoFormat(string rawInfo) {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success) {
                v = m.Value;
            }
            return v.Replace("Video: ", ""); ;
        }
        private string ExtractVideoFormat(string rawVideoFormat) {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }
        private int ExtractVideoWidth(string rawInfo) {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success) {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }
        private int ExtractVideoHeight(string rawInfo) {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success) {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }
        private double ExtractFrameRate(string rawVideoFormat) {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);

            double dFPS = 0;

            foreach (string p in parts) {
                if (p.ToLower().Contains("fps")) {
                    Double.TryParse(p.ToLower().Replace("fps", "").Replace(".", ",").Trim(), out dFPS);

                    break;
                } else if (p.ToLower().Contains("tbr")) {
                    Double.TryParse(p.ToLower().Replace("tbr", "").Replace(".", ",").Trim(), out dFPS);

                    break;
                }
            }

            //Audio: mp3, 44100 Hz, 2 channels, s16, 140 kb/s

            return dFPS;
        }
        private double ExtractAudioBitRate(string rawAudioFormat) {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);

            double dABR = 0;

            foreach (string p in parts) {
                if (p.ToLower().Contains("kb/s")) {
                    Double.TryParse(p.ToLower().Replace("kb/s", "").Replace(".", ",").Trim(), out dABR);

                    break;
                }
            }

            return dABR;
        }
        private double ExtractVideoBitRate(string rawVideoFormat) {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);

            double dVBR = 0;

            foreach (string p in parts) {
                if (p.ToLower().Contains("kb/s")) {
                    Double.TryParse(p.ToLower().Replace("kb/s", "").Replace(".", ",").Trim(), out dVBR);

                    break;
                }
            }

            return dVBR;
        }
        private long ExtractTotalFrames(TimeSpan duration, double frameRate) {
            return (long)Math.Round(duration.TotalSeconds * frameRate, 0);
        }
        #endregion
    }
}
