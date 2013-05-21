using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace GVideo
{
    public partial class HelpX264 : Form
    {
        private StringBuilder sb = null;
        public HelpX264()
        {
            sb = new StringBuilder();
            InitializeComponent();
            //Init();
            textBox1.Text = "hello";
            Init();
        }

        public void Init()
        {
            //String path = v.getPath();
            string ph = Environment.CommandLine;
            ph = ph.Substring(0, ph.LastIndexOf('\\') + 1);
            if (ph[0] == '"')
                ph = ph.Substring(1);
            //FileService.createBat(path, ph, options, output);

            Process p = new Process();
            p.StartInfo.FileName = "\"" + ph + "x264.exe\"";
            p.StartInfo.Arguments = "--fullhelp";
            //p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            //p.ErrorDataReceived += new DataReceivedEventHandler(Output);
            p.OutputDataReceived += new DataReceivedEventHandler(Output);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            //p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();
            p.Dispose();
            textBox1.Text = sb.ToString();
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (String.IsNullOrEmpty(output.Data)) return;
            //view.changeTitle(output.Data);
            sb.AppendLine(output.Data);
        }


    }
}
