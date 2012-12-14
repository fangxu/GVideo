using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Video_for_G1
{
    public partial class Bat : Form
    {
        public Bat()
        {
            InitializeComponent();
        }

        private void textBoxOne_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void textBoxOne_DragDrop(object sender, DragEventArgs e)
        {
            string[] stringTemp = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBoxOne.Text = stringTemp[0];
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            String file = textBoxOne.Text;
            String subtitle = textBoxSubtitle.Text;
            if (file == "")
            {
                return;
            }
            String[] vParts = SplitFilePathName(file);
            String[] sParts = SplitFilePathName(subtitle);

            String avsArgs = textBoxAvsArgs.Text;
            String lancResize = textBoxResize.Text;

            int num = (int)numericUpDown1.Value;

            //AVS
            for (int i = 1; i <= num; i++)
            {
                using (FileStream fs = new FileStream(vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + "v.avs", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.WriteLine(@"DirectShowSource(""" + vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + vParts[3]
                            + @""", " + avsArgs);
                        sw.WriteLine(@"LanczosResize(" + lancResize + ")");
                        if (checkBoxHasSubtitle.Checked)
                        {
                            sw.WriteLine(@"LoadPlugin(""" + textBoxVSFilter.Text + @""")");
                            sw.WriteLine(@"TextSub(""" + sParts[0] + sParts[1] + i.ToString("D2") + sParts[2] + sParts[3] + @""", 1)");
                        }
                    }
                }
            }

            //m4a
            String audioFile = file;
            if (checkBoxHasAudio.Checked)
            {
                audioFile = textBoxAudio.Text;
            }

            String[] aParts = SplitFilePathName(audioFile);
            String q = textBoxQ.Text;

            using (FileStream fs = new FileStream(Global.ph + vParts[1] + vParts[2] + "_m4a.bat", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 1; i <= num; i++)
                    {
                        sw.WriteLine(@"ffmpeg -i """ + aParts[0] + aParts[1] + i.ToString("D2") + aParts[2] + aParts[3] +
                            @""" -f wav - | neroaacenc -q " + q + @" -if - -ignorelength -of """
                            + vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + @".m4a""");
                    }
                }
            }

            //mux
            using (FileStream fs = new FileStream(Global.ph + vParts[1] + vParts[2] + "_mux.bat", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 1; i <= num; i++)
                    {
                        sw.WriteLine(@"ffmpeg -i """ + vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + @"v.mp4"" -i """
                            + vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + @".m4a"" -vcodec copy -acodec copy """
                            + vParts[0] + vParts[1] + i.ToString("D2") + vParts[2] + @"enc.mp4""");
                    }
                }
            }
        }

        private String[] SplitFilePathName(String fileString)
        {
            String[] result = new String[4];
            Match m = Regex.Match(fileString, @"([\s\S]+\\)([\s\S]*?\[)\d{2}(][\s\S]*?)(\.[\S]*)$");
            if (!m.Success)
            {
                return null;
            }
            result[0] = m.Groups[1].Value;
            result[1] = m.Groups[2].Value;
            result[2] = m.Groups[3].Value;
            result[3] = m.Groups[4].Value;
            return result;
        }


    }
}
