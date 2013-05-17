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
using System.Reflection;

namespace GVideo
{
    public partial class FormMain : Form
    {
        //Video项目
        HashSet<VideoItem> videos;
        //压制完成后的动作
        AfterDone afterDone;
        //Extract窗口
        Extract extract;
        //Bat窗口
        Bat bat;

        #region 构造函数

        public FormMain() {
            //初始化数据
            videos = new HashSet<VideoItem>(new SynonymComparer());
            afterDone = AfterDone.nothing;
            InitializeComponent();
            //检查x264.exe,ffmpeg.exe,neroaacenc.exe三个文件是否存在
            //mkvextract.exe和mkvinfo.exe不存在是提示Extract功能不可用
            FileService.checkExe();
            //默认不置顶
            checkBoxTop.Checked = false;
            this.TopMost = false;
            //添加完成后选项，默认nothing
            comboBoxAfterDone.Items.AddRange(new String[] {
                AfterDone.nothing.ToString(),AfterDone.close.ToString(),AfterDone.shutdown.ToString() });
            comboBoxAfterDone.SelectedIndex = 0;
            //使用上次的配置信息
            textBoxOutput.Text = Properties.Settings.Default.MainForm_Output;
            textBoxOptions.Text = Properties.Settings.Default.MainForm_Args;
            if (textBoxOptions.Text == "") {
                textBoxOptions.Text = "--tune animation --crf 23";
            }
            initTitle();
        }
        #endregion

        #region 按键响应
        /************************************************************************/
        /* 清空按钮响应                                                         */
        /************************************************************************/
        private void buttonClear_Click(object sender, EventArgs e) {
            videos.Clear();
            updateListView();
        }

        /************************************************************************/
        /* 打开按钮响应，选择输出目录                                           */
        /************************************************************************/
        private void buttonOpen_Click(object sender, EventArgs e) {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK) {
                String outPath = dlg.SelectedPath;
                if (outPath.EndsWith("\\")) {
                    outPath = outPath.Substring(0, outPath.Length - 1);
                }
                textBoxOutput.Text = outPath;
            }
        }

        /************************************************************************/
        /* 清空按钮响应                                                         */
        /************************************************************************/
        private void buttonRemove_Click(object sender, EventArgs e) {
            int n = listView.SelectedItems.Count;
            String selected = null;
            for (int i = 0; i < n; i++) {
                selected = listView.SelectedItems[i].Text;
                videos.Remove(new VideoItem(selected));
            }
            updateListView();
        }

        /************************************************************************/
        /* 开始按钮响应，开始压制                                               */
        /************************************************************************/
        private void buttonStart_Click(object sender, EventArgs e) {
            String options, output;
            options = textBoxOptions.Text;
            output = textBoxOutput.Text;
            VideoItem v = null;

            Thread t = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (; ; ) {
                    v = videos.FirstOrDefault(x => x.getStatus() == Status.waitting);
                    if (v == null) {
                        break;
                    }
                    v.setStatus(Status.processing);
                    updateListView();

                    String outFile = FileService.createBat(v.getPath(), options, output);
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
                    if (checkBoxReName.Checked) {
                        FileService.reFileName(outFile);
                    }

                }
                sw.Stop();                
                initTitle(sw.Elapsed.ToString());
                afterEncode();
            });
            t.IsBackground = true;
            t.Start();
        }

        /************************************************************************/
        /* 帮助按钮响应，弹出帮助窗口                                           */
        /************************************************************************/
        private void buttonHelp_Click(object sender, EventArgs e) {
            new HelpX264().Show();
        }



        /************************************************************************/
        /* 打开Bat窗口                                                          */
        /************************************************************************/
        private void buttonBat_Click(object sender, EventArgs e) {
            if (bat == null || bat.IsDisposed) {
                bat = new Bat();
                bat.Owner = this;
            }
            if (bat.Visible) {
                bat.Close();
                return;
            }
            bat.Show();
            if (extract != null && extract.Visible) {
                bat.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y + extract.Height);
            } else {
                bat.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y);
            }

        }

        /************************************************************************/
        /* 打开Extract窗口                                                      */
        /************************************************************************/
        private void buttonExtract_Click(object sender, EventArgs e) {
            if (!FileService.checkExe()) {
                return;
            }
            if (extract == null || extract.IsDisposed) {
                extract = new Extract();
                extract.Owner = this;
            }
            if (extract.Visible) {
                extract.Close();
                return;
            }
            extract.Show();
            if (bat != null && bat.Visible) {
                extract.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y + bat.Height);
            } else {
                extract.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y);
            }
        }

        #endregion


        #region 事件响应

        /************************************************************************/
        /* 视频文件拖入窗体                                                     */
        /************************************************************************/
        private void FormMain_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        /************************************************************************/
        /* 视频文件在窗体内释放，添加到videos中                                 */
        /************************************************************************/
        private void FormMain_DragDrop(object sender, DragEventArgs e) {
            string[] stringTemp = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (textBoxOutput.Text == "") {
                textBoxOutput.Text = stringTemp[0].Substring(0, stringTemp[0].LastIndexOf('\\'));
            }
            for (int i = 0; i < stringTemp.Length; i++) {
                videos.Add(new VideoItem(stringTemp[i]));
            }
            updateListView();
        }

        /************************************************************************/
        /* 置顶选项选择响应                                                     */
        /************************************************************************/
        private void checkBoxTop_CheckedChanged(object sender, EventArgs e) {
            this.TopMost = checkBoxTop.Checked;
        }

        /************************************************************************/
        /* 完成后选项选择响应                                                   */
        /************************************************************************/
        private void comboBoxAfterDone_SelectedIndexChanged(object sender, EventArgs e) {
            afterDone = (AfterDone)comboBoxAfterDone.SelectedIndex;
        }

        /************************************************************************/
        /* 关闭前保存配置信息                                                   */
        /************************************************************************/
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e) {
            FileService.deleteBat();
            Properties.Settings.Default.MainForm_Args = textBoxOptions.Text;
            Properties.Settings.Default.MainForm_Output = textBoxOutput.Text;
            Properties.Settings.Default.Save();
        }

        /************************************************************************/
        /* 移动主窗口事件，使Bat和Extract窗口跟随移动。                         */
        /************************************************************************/
        private void FormMain_Move(object sender, EventArgs e) {
            if (bat == null && extract == null) {
                return;
            }
            if (bat != null && bat.IsDisposed) {
                bat = null;
            }
            if (extract != null && extract.IsDisposed) {
                extract = null;
            }
            if (bat == null && extract == null) {
                return;
            }

            Form f1, f2 = null;
            if (bat == null || extract == null) {
                f1 = bat != null ? (Form)bat : extract;
            } else if (bat.Visible != extract.Visible) {
                f1 = bat.Visible ? (Form)bat : extract;
                f1.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y);
                //f1.TopMost=true;
            } else {
                f1 = bat.Location.Y > extract.Location.Y ? (Form)extract : bat;
                f2 = bat.Location.Y <= extract.Location.Y ? (Form)extract : bat;
            }

            f1.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y);
            if (f2 != null) {
                f2.SetDesktopLocation(this.Location.X + this.Width, this.Location.Y + f1.Height);
            }
        }
        #endregion

        #region 功能函数

        private void initTitle(String subtitle = "") {
            AssemblyName info = Assembly.GetExecutingAssembly().GetName();
            String title = info.Name + " - " + info.Version.Major
                + "." + info.Version.Minor
                + "." + info.Version.Build
                + (subtitle != "" ? " {" + subtitle + "}" : "");
            if (this.InvokeRequired) {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.Text = title;
                }));
            } else {
                this.Text = title;
            }
        }

        /************************************************************************/
        /* 刷新列表                                                             */
        /************************************************************************/
        public void updateListView() {
            if (this.InvokeRequired) {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    listView.Items.Clear();
                    foreach (VideoItem vi in videos) {
                        ListViewItem it = new ListViewItem();
                        it.Text = vi.getName();
                        it.SubItems.Add(vi.getStatus().ToString());
                        it.SubItems.Add(vi.BitRate.ToString());
                        listView.Items.Add(it);
                    }
                    if (videos.Count != 0) {
                        listView.Columns[0].Width = -1;
                    }
                }));
            } else {
                listView.Items.Clear();
                foreach (VideoItem vi in videos) {
                    ListViewItem it = new ListViewItem();
                    it.Text = vi.getName();
                    it.SubItems.Add(vi.getStatus().ToString());
                    it.SubItems.Add(vi.BitRate.ToString());
                    listView.Items.Add(it);
                }
                if (videos.Count != 0) {
                    listView.Columns[0].Width = -1;
                }
            }

        }

        /************************************************************************/
        /* 更改窗体标题，用于显示压制信息                                       */
        /************************************************************************/
        public void changeTitle(String title) {

            if (this.InvokeRequired) {
                this.BeginInvoke(new MethodInvoker(() =>
                            {
                                this.Text = title;

                            }));
            } else {
                this.Text = title;
            }
        }

        /************************************************************************/
        /* 完成后动作判断                                                       */
        /************************************************************************/
        private void afterEncode() {
            switch (afterDone) {
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
        //[10.9%]123214/535235 frames,
        //size = 
        //Processed 1000 seconds
        private void Output(object sendProcess, DataReceivedEventArgs output) {
            if (String.IsNullOrEmpty(output.Data)) return;
            String info = output.Data;
            if (info.StartsWith("["))
                info = parserX264Info(info);
            else if (info.StartsWith("size"))
                info = parserFfmpegWavInfo(info);
            else if (info.StartsWith("frame="))
                info = parserFfmpegMuxInfo(info);
            else return;
            changeTitle(info);
        }

        private String parserX264Info(String info) {
            String result = info;
            return result;
        }

        private String parserFfmpegWavInfo(String info) {
            String result = info;
            return result;
        }

        private String parserFfmpegMuxInfo(String info) {
            String result = info;
            return result;
        }
        #endregion
    }
}
