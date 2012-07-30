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
        public AfterDone afterDone;
        VideoService vs;
        public FormMain()
        {
            InitializeComponent();
            FileService.checkExe();
            checkBoxTop.Checked = true;
            this.TopMost = true;
            comboBoxAfterDone.Items.AddRange(new String[] {
                AfterDone.nothing.ToString(),AfterDone.close.ToString(),AfterDone.shutdown.ToString() });
            comboBoxAfterDone.SelectedIndex = 0;
            vs = new VideoService(this);//add Columns  "file" and "status"                      
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
            vs.addVideoItem(stringTemp);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            vs.clearVideoItem();
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
            vs.removeVideoItem(selectItems);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            vs.startEncode();
        }

        private void checkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxTop.Checked;
        }        

        /*private void changeStatue(int i, string s)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                listView.Items[i].SubItems[1].Text = s;
            }));
        }*/       
    }
}
