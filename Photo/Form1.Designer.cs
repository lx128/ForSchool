namespace WinFormCharpWebCam
{
    //Design by Pongsakorn Poosankam
    partial class mainWinForm
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
        	this.imgVideo = new System.Windows.Forms.PictureBox();
        	this.imgCapture = new System.Windows.Forms.PictureBox();
        	this.bntStart = new System.Windows.Forms.Button();
        	this.bntStop = new System.Windows.Forms.Button();
        	this.bntContinue = new System.Windows.Forms.Button();
        	this.bntCapture = new System.Windows.Forms.Button();
        	this.bntSave = new System.Windows.Forms.Button();
        	this.bntVideoFormat = new System.Windows.Forms.Button();
        	this.bntVideoSource = new System.Windows.Forms.Button();
        	this.comboBox1 = new System.Windows.Forms.ComboBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.button1 = new System.Windows.Forms.Button();
        	((System.ComponentModel.ISupportInitialize)(this.imgVideo)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.imgCapture)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// imgVideo
        	// 
        	this.imgVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.imgVideo.Location = new System.Drawing.Point(12, 71);
        	this.imgVideo.Name = "imgVideo";
        	this.imgVideo.Size = new System.Drawing.Size(320, 200);
        	this.imgVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        	this.imgVideo.TabIndex = 0;
        	this.imgVideo.TabStop = false;
        	// 
        	// imgCapture
        	// 
        	this.imgCapture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.imgCapture.Location = new System.Drawing.Point(410, 71);
        	this.imgCapture.Name = "imgCapture";
        	this.imgCapture.Size = new System.Drawing.Size(200, 320);
        	this.imgCapture.TabIndex = 1;
        	this.imgCapture.TabStop = false;
        	// 
        	// bntStart
        	// 
        	this.bntStart.Location = new System.Drawing.Point(12, 12);
        	this.bntStart.Name = "bntStart";
        	this.bntStart.Size = new System.Drawing.Size(41, 23);
        	this.bntStart.TabIndex = 2;
        	this.bntStart.Text = "Start";
        	this.bntStart.UseVisualStyleBackColor = true;
        	this.bntStart.Click += new System.EventHandler(this.bntStart_Click);
        	// 
        	// bntStop
        	// 
        	this.bntStop.Location = new System.Drawing.Point(59, 12);
        	this.bntStop.Name = "bntStop";
        	this.bntStop.Size = new System.Drawing.Size(49, 23);
        	this.bntStop.TabIndex = 3;
        	this.bntStop.Text = "Stop";
        	this.bntStop.UseVisualStyleBackColor = true;
        	this.bntStop.Click += new System.EventHandler(this.bntStop_Click);
        	// 
        	// bntContinue
        	// 
        	this.bntContinue.Location = new System.Drawing.Point(114, 12);
        	this.bntContinue.Name = "bntContinue";
        	this.bntContinue.Size = new System.Drawing.Size(61, 23);
        	this.bntContinue.TabIndex = 4;
        	this.bntContinue.Text = "Continue";
        	this.bntContinue.UseVisualStyleBackColor = true;
        	this.bntContinue.Click += new System.EventHandler(this.bntContinue_Click);
        	// 
        	// bntCapture
        	// 
        	this.bntCapture.Location = new System.Drawing.Point(660, 71);
        	this.bntCapture.Name = "bntCapture";
        	this.bntCapture.Size = new System.Drawing.Size(147, 23);
        	this.bntCapture.TabIndex = 5;
        	this.bntCapture.Text = "Capture Image";
        	this.bntCapture.UseVisualStyleBackColor = true;
        	this.bntCapture.Click += new System.EventHandler(this.bntCapture_Click);
        	// 
        	// bntSave
        	// 
        	this.bntSave.Location = new System.Drawing.Point(660, 129);
        	this.bntSave.Name = "bntSave";
        	this.bntSave.Size = new System.Drawing.Size(147, 23);
        	this.bntSave.TabIndex = 6;
        	this.bntSave.Text = "Enregistrer sous...";
        	this.bntSave.UseVisualStyleBackColor = true;
        	this.bntSave.Click += new System.EventHandler(this.bntSave_Click);
        	// 
        	// bntVideoFormat
        	// 
        	this.bntVideoFormat.Location = new System.Drawing.Point(181, 12);
        	this.bntVideoFormat.Name = "bntVideoFormat";
        	this.bntVideoFormat.Size = new System.Drawing.Size(147, 23);
        	this.bntVideoFormat.TabIndex = 7;
        	this.bntVideoFormat.Text = "Video Format";
        	this.bntVideoFormat.UseVisualStyleBackColor = true;
        	this.bntVideoFormat.Click += new System.EventHandler(this.bntVideoFormat_Click);
        	// 
        	// bntVideoSource
        	// 
        	this.bntVideoSource.Location = new System.Drawing.Point(334, 12);
        	this.bntVideoSource.Name = "bntVideoSource";
        	this.bntVideoSource.Size = new System.Drawing.Size(147, 23);
        	this.bntVideoSource.TabIndex = 8;
        	this.bntVideoSource.Text = "Video Source";
        	this.bntVideoSource.UseVisualStyleBackColor = true;
        	this.bntVideoSource.Click += new System.EventHandler(this.bntVideoSource_Click);
        	// 
        	// comboBox1
        	// 
        	this.comboBox1.FormattingEnabled = true;
        	this.comboBox1.Location = new System.Drawing.Point(660, 44);
        	this.comboBox1.Name = "comboBox1";
        	this.comboBox1.Size = new System.Drawing.Size(147, 21);
        	this.comboBox1.TabIndex = 9;
        	this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(660, 13);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(147, 23);
        	this.label1.TabIndex = 10;
        	this.label1.Text = "Classe";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        	// 
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(230, 45);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(251, 23);
        	this.label2.TabIndex = 11;
        	this.label2.Text = "Nom Prénom";
        	this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        	// 
        	// button1
        	// 
        	this.button1.Location = new System.Drawing.Point(660, 100);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(147, 23);
        	this.button1.TabIndex = 12;
        	this.button1.Text = "Enregistrer image";
        	this.button1.UseVisualStyleBackColor = true;
        	this.button1.Click += new System.EventHandler(this.Button1Click);
        	// 
        	// mainWinForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(814, 399);
        	this.Controls.Add(this.button1);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.comboBox1);
        	this.Controls.Add(this.bntVideoSource);
        	this.Controls.Add(this.bntVideoFormat);
        	this.Controls.Add(this.bntSave);
        	this.Controls.Add(this.bntCapture);
        	this.Controls.Add(this.bntContinue);
        	this.Controls.Add(this.bntStop);
        	this.Controls.Add(this.bntStart);
        	this.Controls.Add(this.imgCapture);
        	this.Controls.Add(this.imgVideo);
        	this.Name = "mainWinForm";
        	this.Text = "Photos pour Pronote";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainWinForm_Closing);
        	this.Load += new System.EventHandler(this.mainWinForm_Load);
        	((System.ComponentModel.ISupportInitialize)(this.imgVideo)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.imgCapture)).EndInit();
        	this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;

        #endregion

        private System.Windows.Forms.PictureBox imgVideo;
        private System.Windows.Forms.PictureBox imgCapture;
        private System.Windows.Forms.Button bntStart;
        private System.Windows.Forms.Button bntStop;
        private System.Windows.Forms.Button bntContinue;
        private System.Windows.Forms.Button bntCapture;
        private System.Windows.Forms.Button bntSave;
        private System.Windows.Forms.Button bntVideoFormat;
        private System.Windows.Forms.Button bntVideoSource;
    }
}

