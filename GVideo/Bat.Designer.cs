namespace GVideo
{
    partial class Bat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bat));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxOne = new System.Windows.Forms.TextBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSubtitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAudio = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAvsArgs = new System.Windows.Forms.TextBox();
            this.textBoxVSFilter = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxResize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxHasAudio = new System.Windows.Forms.CheckBox();
            this.checkBoxHasSubtitle = new System.Windows.Forms.CheckBox();
            this.textBoxQ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "One:";
            // 
            // textBoxOne
            // 
            this.textBoxOne.AllowDrop = true;
            this.textBoxOne.Location = new System.Drawing.Point(64, 4);
            this.textBoxOne.Name = "textBoxOne";
            this.textBoxOne.Size = new System.Drawing.Size(373, 21);
            this.textBoxOne.TabIndex = 1;
            this.textBoxOne.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxOne_DragDrop);
            this.textBoxOne.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxOne_DragEnter);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(365, 176);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 2;
            this.buttonCreate.Text = "Create Bat";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(64, 32);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(43, 21);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Numbers:";
            // 
            // textBoxSubtitle
            // 
            this.textBoxSubtitle.AllowDrop = true;
            this.textBoxSubtitle.Enabled = false;
            this.textBoxSubtitle.Location = new System.Drawing.Point(64, 114);
            this.textBoxSubtitle.Name = "textBoxSubtitle";
            this.textBoxSubtitle.Size = new System.Drawing.Size(338, 21);
            this.textBoxSubtitle.TabIndex = 8;
            this.textBoxSubtitle.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxSubtitle_DragDrop);
            this.textBoxSubtitle.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxSubtitle_DragEnter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Subtitle:";
            // 
            // textBoxAudio
            // 
            this.textBoxAudio.AllowDrop = true;
            this.textBoxAudio.Enabled = false;
            this.textBoxAudio.Location = new System.Drawing.Point(64, 87);
            this.textBoxAudio.Name = "textBoxAudio";
            this.textBoxAudio.Size = new System.Drawing.Size(338, 21);
            this.textBoxAudio.TabIndex = 10;
            this.textBoxAudio.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxAudio_DragDrop);
            this.textBoxAudio.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxAudio_DragEnter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Audio:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "AVS args:";
            // 
            // textBoxAvsArgs
            // 
            this.textBoxAvsArgs.Location = new System.Drawing.Point(64, 60);
            this.textBoxAvsArgs.Name = "textBoxAvsArgs";
            this.textBoxAvsArgs.Size = new System.Drawing.Size(373, 21);
            this.textBoxAvsArgs.TabIndex = 12;
            this.textBoxAvsArgs.Text = "fps=23.976, audio=false, convertfps=true).AssumeFPS(24000,1001)";
            // 
            // textBoxVSFilter
            // 
            this.textBoxVSFilter.Location = new System.Drawing.Point(64, 143);
            this.textBoxVSFilter.Name = "textBoxVSFilter";
            this.textBoxVSFilter.Size = new System.Drawing.Size(373, 21);
            this.textBoxVSFilter.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "VSFilter:";
            // 
            // textBoxResize
            // 
            this.textBoxResize.Location = new System.Drawing.Point(219, 31);
            this.textBoxResize.Name = "textBoxResize";
            this.textBoxResize.Size = new System.Drawing.Size(86, 21);
            this.textBoxResize.TabIndex = 16;
            this.textBoxResize.Text = "480,272";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(130, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "LanczosReize:";
            // 
            // checkBoxHasAudio
            // 
            this.checkBoxHasAudio.AutoSize = true;
            this.checkBoxHasAudio.Location = new System.Drawing.Point(413, 90);
            this.checkBoxHasAudio.Name = "checkBoxHasAudio";
            this.checkBoxHasAudio.Size = new System.Drawing.Size(15, 14);
            this.checkBoxHasAudio.TabIndex = 17;
            this.checkBoxHasAudio.UseVisualStyleBackColor = true;
            this.checkBoxHasAudio.CheckedChanged += new System.EventHandler(this.checkBoxHasAudio_CheckedChanged);
            // 
            // checkBoxHasSubtitle
            // 
            this.checkBoxHasSubtitle.AutoSize = true;
            this.checkBoxHasSubtitle.Location = new System.Drawing.Point(413, 117);
            this.checkBoxHasSubtitle.Name = "checkBoxHasSubtitle";
            this.checkBoxHasSubtitle.Size = new System.Drawing.Size(15, 14);
            this.checkBoxHasSubtitle.TabIndex = 18;
            this.checkBoxHasSubtitle.UseVisualStyleBackColor = true;
            this.checkBoxHasSubtitle.CheckedChanged += new System.EventHandler(this.checkBoxHasSubtitle_CheckedChanged);
            // 
            // textBoxQ
            // 
            this.textBoxQ.Location = new System.Drawing.Point(346, 31);
            this.textBoxQ.Name = "textBoxQ";
            this.textBoxQ.Size = new System.Drawing.Size(64, 21);
            this.textBoxQ.TabIndex = 20;
            this.textBoxQ.Text = "0.28";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(323, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "q:";
            // 
            // Bat
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 211);
            this.Controls.Add(this.textBoxQ);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxHasSubtitle);
            this.Controls.Add(this.checkBoxHasAudio);
            this.Controls.Add(this.textBoxResize);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxVSFilter);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxAvsArgs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAudio);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxSubtitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.textBoxOne);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bat";
            this.ShowInTaskbar = false;
            this.Text = "Bat for AVS and Megui";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Bat_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxOne;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSubtitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAudio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAvsArgs;
        private System.Windows.Forms.TextBox textBoxVSFilter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxResize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxHasAudio;
        private System.Windows.Forms.CheckBox checkBoxHasSubtitle;
        private System.Windows.Forms.TextBox textBoxQ;
        private System.Windows.Forms.Label label3;
    }
}