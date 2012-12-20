using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Video_for_G1
{
    public partial class Extract : Form
    {
        List<String> tracks;
        String[] fileParts;
        String progressRate;
        public Extract() {
            InitializeComponent();
            tracks = new List<String>();
        }

        private void Extract_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Extract_DragDrop(object sender, DragEventArgs e) {
            String stringTemp = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            textBoxOne.Text = stringTemp;
            textBoxOut.Text = stringTemp.Remove(stringTemp.LastIndexOf('\\') + 1);
            tracks = AnalyseTracks(stringTemp);
            checkedListBox1.Items.Clear();
            foreach (String s in tracks) {
                checkedListBox1.Items.Add(s);
            }
            fileParts = Global.SplitFilePathName(stringTemp);
        }

        private String[] GetFileNames(String file) {
            if (fileParts == null) {
                return null;
            }
            int num = (int)numericUpDown1.Value;
            String[] fileNames = new String[num];
            for (int i = 1; i <= num; i++) {
                fileNames[i - 1] = fileParts[0] + fileParts[1] + i.ToString("D2") + fileParts[2] + fileParts[3];
            }
            return fileNames;
        }

        private List<String> AnalyseTracks(String file) {
            Process p = new Process();
            p.StartInfo.FileName = "\"" + Global.ph + "mkvinfo.exe\"";
            p.StartInfo.Arguments = "\"" + file + "\"" + " --redirect-output " + Global.TRACKSINFO;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
            p.Close();
            p.Dispose();

            List<String> tracks = new List<String>();
            using (FileStream fs = new FileStream(Global.TRACKSINFO, FileMode.Open)) {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default)) {
                    String line = sr.ReadLine();
                    String temp = null;
                    bool hasTrack = false;
                    while (line != null) {
                        if (line.Equals("| + A track")) {
                            if (hasTrack) {
                                tracks.Add(temp);
                            } else {
                                hasTrack = true;
                            }
                        } else if (line.StartsWith("|  + Track number") && hasTrack) {
                            temp = line[line.Length - 2] + ":";
                        } else if (line.Contains("Track type") && hasTrack) {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        } else if (line.Contains("Name") && hasTrack) {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        } else if (line.Contains("Codec ID") && hasTrack) {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        } else if (line.Contains("Language") && hasTrack) {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        } else if (line.StartsWith("|+") && hasTrack) {
                            tracks.Add(temp);
                            hasTrack = false;
                        }

                        line = sr.ReadLine();
                    }
                }
            }
            if (File.Exists(Global.TRACKSINFO)) {
                File.Delete(Global.TRACKSINFO);
            }
            return tracks;
        }

        private void buttonFileNameAnalyse_Click(object sender, EventArgs e) {
            String[] names = GetFileNames(textBoxOne.Text);
            if (names == null) {
                return;
            }
            String total = "";
            foreach (String s in names) {
                total += s + "\n";
            }
            MessageBox.Show(total);
        }

        private void buttonExtract_Click(object sender, EventArgs e) {
            if (checkedListBox1.CheckedIndices.Count == 0) {
                return;
            }
            String outPath = textBoxOut.Text;
            List<String> checkedItem = new List<String>();
            foreach (object item in checkedListBox1.CheckedItems) {
                String track = item.ToString();
                if (track.Contains("audio")) {
                    if (track.Contains("A_AAC")) {
                        checkedItem.Add(track[0] + ":.aac\"");
                    } else {
                        checkedItem.Add(track[0] + ":.raw\"");
                    }

                } else if (track.Contains("subtitles")) {
                    if (track.Contains("S_TEXT/ASS")) {
                        checkedItem.Add(track[0] + ":.ass\"");
                    } else if (track.Contains("S_TEXT/UTF8")) {
                        checkedItem.Add(track[0] + ":.srt\"");
                    } else {
                        checkedItem.Add(track[0] + ":.raw\"");
                    }
                } else {
                    return;
                }
            }

            int num = (int)numericUpDown1.Value;

            Thread t = new Thread(() =>
            {
                for (int i = 1; i <= num; i++) {
                    String file = fileParts[0] + fileParts[1] + i.ToString("D2") + fileParts[2] + fileParts[3];
                    String outTracks = " ";
                    for (int s = 0; s < checkedItem.Count; s++) {
                        outTracks += " " + checkedItem[s].Replace(":", ":\"" + fileParts[0]
                            + fileParts[1] + i.ToString("D2") + fileParts[2] + "_" + s);
                    }
                    Process p = new Process();
                    p.StartInfo.FileName = "\"" + Global.ph + "mkvextract.exe\"";
                    p.StartInfo.Arguments = "tracks \"" + file + "\" " + outTracks;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.OutputDataReceived += new DataReceivedEventHandler(ExtractTracks);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    progressRate = i + "/" + num;
                    p.Start();
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                    p.Close();
                    p.Dispose();
                }
                ResetTitle();
            });
            t.IsBackground = true;
            t.Start();
        }

        private void ResetTitle() {
            changeTitle("Extract Mkv Tracks");
        }

        private void ExtractTracks(object sendProcess, DataReceivedEventArgs output) {
            if (String.IsNullOrEmpty(output.Data)) return;
            changeTitle(progressRate + " : " + output.Data);
            Console.WriteLine(output.Data);
        }

        private void changeTitle(string title) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.Text = title;
                }));
            } else {
                this.Text = title;
            }
        }
    }
}
