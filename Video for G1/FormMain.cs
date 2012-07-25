using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Video_for_G1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            listView.Columns.Add("file", -2, HorizontalAlignment.Left);
            listView.Columns.Add("statue", -2, HorizontalAlignment.Left);
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] stringTemp = (string[])e.Data.GetData(DataFormats.FileDrop);           
            textBoxOutput.Text = stringTemp[0].Substring(0, stringTemp[0].LastIndexOf('\\'));
            for (int i = 0; i < stringTemp.Length; i++)
            {
                ListViewItem it = new ListViewItem();
                it.Text = stringTemp[i];
                it.SubItems.Add("waiting");
                listView.Items.Add(it);
            }
            listView.Columns[0].Width = -1;
            listView.Columns[1].Width = -1;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listView.Clear();
            listView.Columns.Add("file", -2, HorizontalAlignment.Left);
            listView.Columns.Add("statue", -2, HorizontalAlignment.Left);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxOutput.Text = dlg.SelectedPath;
            }
            else
            {
                return;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int n = listView.SelectedItems.Count;
            for (int i = 0; i < n; i++)
            {
                listView.Items.Remove(listView.SelectedItems[0]);
            }

        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (String.IsNullOrEmpty(output.Data)) return;
            changeTitle(output.Data);
        }



        private void buttonStart_Click(object sender, EventArgs e)
        {
            int count = listView.Items.Count;
            string[] video = new string[count];
            string[] state = new string[count];
            string options = textBoxOptions.Text;
            string output = textBoxOutput.Text;
            for (int i = 0; i < count; i++)
            {
                video[i] = this.listView.Items[i].SubItems[0].Text;
                state[i] = this.listView.Items[i].SubItems[1].Text;
            }

            new Thread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if (state[i] == "waiting")
                    {
                        changeStatue(i, "processing");
                        Process p = new Process();
                        string path = " \"" + output + video[i].Substring(video[i].LastIndexOf('\\'), video[i].LastIndexOf('.') - video[i].LastIndexOf('\\'));
                        string vo = path + "_v.mp4\" ";
                        string ao = path + "_a.m4a\" ";
                        string avo = path + "_enc.mp4\" ";
                        string vi = " \"" + video[i] + "\" ";
                        string ph = Environment.CommandLine;
                        ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
                        if (ph[0] == '"')
                            ph = ph.Substring(1);

                        string strBatPath = ph + "temp.bat";
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
                        Stream st = new FileStream(strBatPath, FileMode.OpenOrCreate);
                        using (StreamWriter sw = new StreamWriter(st))
                        {
                            sw.Write(sBuilder.ToString());
                            sw.Close();
                            st.Dispose();
                            st.Close();
                        }

                        p.StartInfo.FileName = "\"" + ph + "temp.bat\"";
                        p.StartInfo.RedirectStandardError = true;
                        p.ErrorDataReceived += new DataReceivedEventHandler(Output);
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        p.BeginErrorReadLine();
                        p.WaitForExit();
                        changeStatue(i, "done");
                        p.Close();
                        p.Dispose();
                    }
                }
                changeTitle("Video for G1");
            }).Start();

        }
        private void changeStatue(int i, string s)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                listView.Items[i].SubItems[1].Text = s;
            }));
        }

        private void changeTitle(string s)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                this.Text = s;
            }));
        }
    }
}
