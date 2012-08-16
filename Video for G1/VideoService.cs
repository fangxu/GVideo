using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Video_for_G1
{
    class VideoService
    {
        FormMain form;
        HashSet<VideoItem> videoItems;
        Thread thread;
        String options, output;
        //Thread thread ;


        public void encode()
        {
            foreach (VideoItem v in videoItems)
            {
                if (v.getStatus() != Status.waitting) continue;
                v.setStatus(Status.processing);
                freshListView();

                String path = v.getPath();
                string ph = Environment.CommandLine;
                ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
                if (ph[0] == '"')
                    ph = ph.Substring(1);
                FileService.createBat(path, ph, options, output);

                Process p = new Process();
                p.StartInfo.FileName = "\"" + ph + "temp.bat\"";
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
                freshListView();
            }
            changeTitle("Video for G1");
            //afterDone();
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (String.IsNullOrEmpty(output.Data)) return;
            changeTitle(output.Data);
        }

        public void startEncode()
        {
            options = form.textBoxOptions.Text;
            output = form.textBoxOutput.Text;
            thread = new Thread(new ThreadStart(encode));
            thread.Start();
        }


        public VideoService(FormMain form)
        {
            this.form = form;
            videoItems = new HashSet<VideoItem>(new SynonymComparer());
            initListView();
            //thread = new Thread(new ThreadStart(encode));
        }

        public void addVideoItem(String[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                videoItems.Add(new VideoItem(paths[i]));
            }
            freshListView();
        }

        public void removeVideoItem(String[] files)
        {
            foreach (String f in files)
            {
                videoItems.Remove(new VideoItem(f));
            }
            freshListView();
        }

        public void clearVideoItem()
        {
            videoItems.Clear();
            freshListView();
        }

        public void initListView()
        {
            form.listView.Columns.Add("file", -2, HorizontalAlignment.Left);
            form.listView.Columns.Add("statue", -2, HorizontalAlignment.Left);
        }

        public void freshListView()
        {
            form.BeginInvoke(new MethodInvoker(() =>
            {
                form.listView.Clear();
                initListView();
                foreach (VideoItem vi in videoItems)
                {
                    ListViewItem it = new ListViewItem();
                    it.Text = vi.getName();
                    it.SubItems.Add(vi.getStatus().ToString());
                    form.listView.Items.Add(it);
                }
                if (videoItems.Count != 0)
                {
                    form.listView.Columns[0].Width = -1;
                    form.listView.Columns[1].Width = -1;
                }
            }));
        }

        public void changeTitle(String s)
        {
            form.BeginInvoke(new MethodInvoker(() =>
            {
                form.Text = s;
            }));
        }

        /*public void afterDone()
        {
            switch (form.afterDone)
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
        }*/

        public void stopEncode()
        {
            //             if (thread.ThreadState==System.Threading.ThreadState.Running)
            //             {
            //thread.Abort();
            /* } */
        }
    }

    
}
