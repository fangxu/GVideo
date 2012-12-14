using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Video_for_G1
{
    public partial class Extract : Form
    {
        List<String> tracks;
        public Extract()
        {
            InitializeComponent();
            tracks = new List<String>();
        }

        private void Extract_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Extract_DragDrop(object sender, DragEventArgs e)
        {
            String stringTemp = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            textBoxOne.Text = stringTemp;
            textBoxOut.Text = stringTemp.Remove(stringTemp.LastIndexOf('\\') + 1);
            tracks = AnalyseTracks(stringTemp);
            checkedListBox1.Items.Clear();
            foreach (String s in tracks)
            {
                checkedListBox1.Items.Add(s);
            }

        }

        private String[] GetFileNames(String file)
        {
            String[] nameParts = Global.SplitFilePathName(file);
            int num = (int)numericUpDown1.Value;
            String[] fileNames = new String[num];
            for (int i = 1; i <= num; i++)
            {
                fileNames[i - 1] = nameParts[0] + nameParts[1] + i.ToString("D2") + nameParts[2] + nameParts[3];
            }
            return fileNames;
        }

        private List<String> AnalyseTracks(String file)
        {
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
            using (FileStream fs = new FileStream(Global.TRACKSINFO, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    String line = sr.ReadLine();
                    String temp = null;
                    bool hasTrack = false;
                    while (line != null)
                    {
                        if (line.Equals("| + A track"))
                        {
                            if (hasTrack)
                            {
                                tracks.Add(temp);
                            }
                            else
                            {
                                hasTrack = true;
                            }
                        }
                        else if (line.StartsWith("|  + Track number") && hasTrack)
                        {
                            temp = line[line.Length - 2] + ":";
                        }
                        else if (line.Contains("Track type") && hasTrack)
                        {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        }
                        else if (line.Contains("Name") && hasTrack)
                        {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        }
                        else if (line.Contains("Codec ID") && hasTrack)
                        {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        }
                        else if (line.Contains("Language") && hasTrack)
                        {
                            temp += line.Substring(line.LastIndexOf(':')) + " ";
                        }
                        else if (line.StartsWith("|+") && hasTrack)
                        {
                            tracks.Add(temp);
                            hasTrack = false;
                        }

                        line = sr.ReadLine();
                    }
                }
            }
            if (File.Exists(Global.TRACKSINFO))
            {
                File.Delete(Global.TRACKSINFO);
            }
            return tracks;
        }

        private void buttonFileNameAnalyse_Click(object sender, EventArgs e)
        {
            String[] names = GetFileNames(textBoxOne.Text);
            String total = "";
            foreach (String s in names)
            {
                total += s + "\n";
            }
            MessageBox.Show(total);
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {

        }

        private void ExtractTracks()
        {

        }
    }
}
