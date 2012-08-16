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
    public partial class FormMain : Form, iVideoView
    {
        private static iVideoModel Model = new VideoModel();
        private iVideoControl Control = new VideoControl();
        public String statu;

        
        //VideoService vs;
        public FormMain()
        {
            InitializeComponent();
            FileService.checkExe();
            checkBoxTop.Checked = true;
            this.TopMost = true;
            comboBoxAfterDone.Items.AddRange(new String[] {
                AfterDone.nothing.ToString(),AfterDone.close.ToString(),AfterDone.shutdown.ToString() });
            comboBoxAfterDone.SelectedIndex = 0;
            //vs = new VideoService(this);//add Columns  "file" and "status"                      



            if (Model != null)
            {
                Model.removeObserver(this);
            }
            Control.setView(this);
            Control.setModel(Model);
            Model.addObserver(this);
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] stringTemp = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (textBoxOutput.Text == "")
            {
                textBoxOutput.Text = stringTemp[0].Substring(0, stringTemp[0].LastIndexOf('\\'));
            }
            Control.addVideos(stringTemp);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Control.clearVideo();
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
            String[] selectItems = new String[n];
            for (int i = 0; i < n; i++)
            {
                selectItems[i] = listView.SelectedItems[i].Text;
            }
            Control.removeVideos(selectItems);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Control.encode(this.textBoxOptions.Text, this.textBoxOutput.Text);
        }

        private void checkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxTop.Checked;
        }

        private void comboBoxAfterDone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Model.setAfterDone((AfterDone)comboBoxAfterDone.SelectedIndex);
        }

        public void updateView()
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                listView.Clear();
                listView.Columns.Add("file", -2, HorizontalAlignment.Left);
                listView.Columns.Add("statue", -2, HorizontalAlignment.Left);
                HashSet<VideoItem> videos = Model.getVideos();
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
                    listView.Columns[1].Width = -1;
                }
            }));
        }

        public void changeTitle(String title)
        {


            this.BeginInvoke(new MethodInvoker(() =>
            {
                this.Text = title;
                //this.Text = statu;

            }));

        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            new Help().Show();
        }
    }
}
