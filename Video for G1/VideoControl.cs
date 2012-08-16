using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Video_for_G1
{
    class VideoControl : iVideoControl
    {
        private iVideoModel model;
        private iVideoView view;
        //FormMain form;

        public void addVideo(String name)
        {
            model.addVideo(name);
        }

        public void addVideos(String[] names)
        {
            model.addVideos(names);
        }

        public void removeVideo(String name)
        {
            model.removeVideo(name);
        }

        public void removeVideos(String[] names)
        {
            model.removeVideos(names);
        }

        public void clearVideo()
        {
            model.clearVideo();
        }

        public void setModel(iVideoModel vm)
        {
            this.model = vm;
        }

        public void setView(iVideoView vv)
        {
            this.view = vv;
            //this.form = (FormMain)view;
        }

        public void encode(String options, String output)
        {
            new Thread(() =>
            {
                HashSet<VideoItem> videos = model.getVideos();
                foreach (VideoItem v in videos)
                {
                    if (v.getStatus() != Status.waitting) continue;
                    v.setStatus(Status.processing);
                    view.updateView();

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
                    view.updateView();
                }
                view.changeTitle("Video for G1");
                afterDone();
            }).Start();

            
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (String.IsNullOrEmpty(output.Data)) return;
            view.changeTitle(output.Data);
        }

        public void afterDone()
        {
            switch (model.getAfterDone())
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
    }
}
