using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            else if (!file.Contains("##"))
            {
                return;
            }
            String[] filePart = file.Split(new string[] { "##" }, 2, StringSplitOptions.RemoveEmptyEntries);
            String[] subPart = subtitle.Split(new string[] { "##" }, 2, StringSplitOptions.RemoveEmptyEntries);

            String avsArgs = textBoxAvsArgs.Text;
            String lancResize = textBoxResize.Text;
            String path = file.Substring(0, file.LastIndexOf('\\') + 1);
            String namePart1 = filePart[0].Substring(filePart[0].LastIndexOf('\\') + 1, filePart[0].Length - filePart[0].LastIndexOf('\\') - 1);
            String namePart2 = filePart[1].Substring(0, filePart[1].LastIndexOf('.'));
            int num = (int)numericUpDown1.Value;

            //AVS
            for (int i = 1; i <= num; i++)
            {
                using (FileStream fs = new FileStream(path + namePart1 + i.ToString("D2") + namePart2 + "v.avs", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.WriteLine(@"DirectShowSource(""" + path + namePart1 + i.ToString("D2") + filePart[1]
                            + @""", " + avsArgs);
                        sw.WriteLine(@"LanczosResize(" + lancResize + ")");
                        if (checkBoxHasSubtitle.Checked)
                        {
                            sw.WriteLine(@"LoadPlugin(""" + textBoxVSFilter.Text + @""")");
                            sw.WriteLine(@"TextSub(""" + subPart[0] + i.ToString("D2") + subPart[1] + @""", 1)");
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

            String[] audioPart = audioFile.Split(new string[] { "##" }, 2, StringSplitOptions.RemoveEmptyEntries);

            using (FileStream fs = new FileStream(Global.ph + namePart1 + namePart2 + "_m4a.bat", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 1; i <= num; i++)
                    {
                        sw.WriteLine(@"ffmpeg -i """ + audioPart[0] + i.ToString("D2") + audioPart[1]+
                            @""" -f wav - | neroaacenc -q 0.28 -if - -ignorelength -of """ 
                            + path + namePart1 + i.ToString("D2") + namePart2 + @".m4a""");
                    }
                }
            }

            //mux
            using (FileStream fs = new FileStream(Global.ph + namePart1 + namePart2 + "_mux.bat", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 1; i <= num; i++)
                    {
                        sw.WriteLine(@"ffmpeg -i """ + path + namePart1 + i.ToString("D2") + namePart2 + @"v.mp4"" -i """
                            + path + namePart1 + i.ToString("D2") + namePart2 + @".m4a"" -vcodec copy -acodec copy """
                            + path + namePart1 + i.ToString("D2") + namePart2 + @"enc.mp4""");
                    }
                }
            }
        }
    }
}
