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
        //Video项目
        HashSet<VideoItem> videos;
        //压制完成后的动作
        AfterDone afterDone;

        public FormMain()
        {
            //初始化数据
            videos = new HashSet<VideoItem>(new SynonymComparer());
            afterDone = AfterDone.nothing;
            InitializeComponent();
            //检查x264.exe,ffmpeg.exe,neroaacenc.exe三个文件是否存在
            FileService.checkExe();
            //默认置顶
            checkBoxTop.Checked = true;
            this.TopMost = true;
            //添加完成后选项，默认nothing
            comboBoxAfterDone.Items.AddRange(new String[] {
                AfterDone.nothing.ToString(),AfterDone.close.ToString(),AfterDone.shutdown.ToString() });
            comboBoxAfterDone.SelectedIndex = 0;
        }

        /************************************************************************/
        /* 视频文件拖入窗体                                                     */
        /************************************************************************/
        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        /************************************************************************/
        /* 视频文件在窗体内释放，添加到videos中                                 */
        /************************************************************************/
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] stringTemp = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (textBoxOutput.Text == "")
            {
                textBoxOutput.Text = stringTemp[0].Substring(0, stringTemp[0].LastIndexOf('\\'));
            }
            for (int i = 0; i < stringTemp.Length; i++)
            {
                videos.Add(new VideoItem(stringTemp[i]));
            }
            updateListView();
        }

        /************************************************************************/
        /* 刷新列表                                                             */
        /************************************************************************/
        public void updateListView()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    listView.Items.Clear();
                    foreach (VideoItem vi in videos)
                    {
                        ListViewItem it = new ListViewItem();
                        it.Text = vi.getName();
                        it.SubItems.Add(vi.getStatus().ToString());
                        listView.Items.Add(it);
                    }
                    if (videos.Count != 0)
                    {
                        listView.Columns[0].Width = -1;
                    }
                }));
            }
            else
            {
                listView.Items.Clear();
                foreach (VideoItem vi in videos)
                {
                    ListViewItem it = new ListViewItem();
                    it.Text = vi.getName();
                    it.SubItems.Add(vi.getStatus().ToString());
                    listView.Items.Add(it);
                }
                if (videos.Count != 0)
                {
                    listView.Columns[0].Width = -1;
                }
            }

        }

        /************************************************************************/
        /* 清空按钮响应                                                         */
        /************************************************************************/
        private void buttonClear_Click(object sender, EventArgs e)
        {
            videos.Clear();
            updateListView();
        }

        /************************************************************************/
        /* 打开按钮响应，选择输出目录                                           */
        /************************************************************************/
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                String outPath = dlg.SelectedPath;
                if (outPath.EndsWith("\\"))
                {
                    outPath = outPath.Substring(0, outPath.Length - 1);
                }
                textBoxOutput.Text = outPath;
            }
        }

        /************************************************************************/
        /* 清空按钮响应                                                         */
        /************************************************************************/
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int n = listView.SelectedItems.Count;
            String selected = null;
            for (int i = 0; i < n; i++)
            {
                selected = listView.SelectedItems[i].Text;
                videos.Remove(new VideoItem(selected));
            }
            updateListView();
        }

        /************************************************************************/
        /* 开始按钮响应，开始压制                                               */
        /************************************************************************/
        private void buttonStart_Click(object sender, EventArgs e)
        {
            String options, output;
            options = textBoxOptions.Text;
            output = textBoxOutput.Text;
            VideoItem v = null;

            Thread t = new Thread(() =>
            {
                for (; ; )
                {
                    v = videos.FirstOrDefault(x => x.getStatus() == Status.waitting);
                    if (v == null)
                    {
                        break;
                    }
                    v.setStatus(Status.processing);
                    updateListView();

                    FileService.createBat(v.getPath(), options, output);
                    Process p = new Process();
                    p.StartInfo.FileName = "\"" + Global.batPh + "\"";
                    p.StartInfo.RedirectStandardError = true;
                    p.ErrorDataReceived += new DataReceivedEventHandler(Output);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                    p.Close();
                    p.Dispose();
                    v.setStatus(Status.done);
                    updateListView();
                }
                changeTitle("Video for G1");
                afterEncode();
            });
            t.IsBackground = true;
            t.Start();
        }

        /************************************************************************/
        /* 完成后动作判断                                                       */
        /************************************************************************/
        private void afterEncode()
        {
            switch (afterDone)
            {
                case AfterDone.nothing:
                    break;
                case AfterDone.close:
                    Environment.Exit(0);
                    break;
                case AfterDone.shutdown:
                    Process bootProcess = new Process();
                    bootProcess.StartInfo.FileName = "shutdown";
                    bootProcess.StartInfo.Arguments = "/s";
                    bootProcess.Start();
                    break;
                default:
                    break;
            }
        }

        /************************************************************************/
        /* 异步显示压制信息                                                     */
        /************************************************************************/
        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (String.IsNullOrEmpty(output.Data)) return;
            changeTitle(output.Data);
        }

        /************************************************************************/
        /* 置顶选项选择响应                                                     */
        /************************************************************************/
        private void checkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxTop.Checked;
        }

        /************************************************************************/
        /* 完成后选项选择响应                                                   */
        /************************************************************************/
        private void comboBoxAfterDone_SelectedIndexChanged(object sender, EventArgs e)
        {
            afterDone = (AfterDone)comboBoxAfterDone.SelectedIndex;
        }

        /************************************************************************/
        /* 更改窗体标题，用于显示压制信息                                       */
        /************************************************************************/
        public void changeTitle(String title)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                            {
                                this.Text = title;

                            }));
            }
            else
            {
                this.Text = title;
            }
        }

        /************************************************************************/
        /* 帮助按钮响应，弹出帮助窗口                                           */
        /************************************************************************/
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            new Help().Show();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FileService.deleteBat();
        }
    }
}
