﻿namespace MusicPlayerDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip2 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            PlayButton = new Button();
            PauseButton = new Button();
            PreviousButton = new Button();
            NextButton = new Button();
            selectedFileLabel = new Label();
            playlistCountLabel = new Label();
            playlistComboBox = new ComboBox();
            trackBar = new TrackBar();
            VolumeTrackBar = new TrackBar();
            label1 = new Label();
            DurationLabel = new Label();
            menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeTrackBar).BeginInit();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(466, 24);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(103, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(103, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItemClick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // PlayButton
            // 
            PlayButton.Location = new Point(100, 194);
            PlayButton.Name = "PlayButton";
            PlayButton.Size = new Size(99, 23);
            PlayButton.TabIndex = 2;
            PlayButton.Text = "Play";
            PlayButton.UseVisualStyleBackColor = true;
            PlayButton.Click += PlayButtonClick;
            // 
            // PauseButton
            // 
            PauseButton.Location = new Point(285, 194);
            PauseButton.Name = "PauseButton";
            PauseButton.Size = new Size(99, 23);
            PauseButton.TabIndex = 3;
            PauseButton.Text = "Pause";
            PauseButton.UseVisualStyleBackColor = true;
            PauseButton.Click += PauseButtonClick;
            // 
            // PreviousButton
            // 
            PreviousButton.Location = new Point(52, 113);
            PreviousButton.Name = "PreviousButton";
            PreviousButton.Size = new Size(99, 23);
            PreviousButton.TabIndex = 5;
            PreviousButton.Text = "Previous";
            PreviousButton.UseVisualStyleBackColor = true;
            PreviousButton.Click += PreviousButtonClick;
            // 
            // NextButton
            // 
            NextButton.Location = new Point(326, 113);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(99, 23);
            NextButton.TabIndex = 6;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += NextButtonClick;
            // 
            // selectedFileLabel
            // 
            selectedFileLabel.AutoSize = true;
            selectedFileLabel.Location = new Point(176, 113);
            selectedFileLabel.Name = "selectedFileLabel";
            selectedFileLabel.Size = new Size(0, 15);
            selectedFileLabel.TabIndex = 8;
            // 
            // playlistCountLabel
            // 
            playlistCountLabel.AutoSize = true;
            playlistCountLabel.Location = new Point(12, 24);
            playlistCountLabel.Name = "playlistCountLabel";
            playlistCountLabel.Size = new Size(0, 15);
            playlistCountLabel.TabIndex = 10;
            // 
            // playlistComboBox
            // 
            playlistComboBox.FormattingEnabled = true;
            playlistComboBox.Location = new Point(0, 42);
            playlistComboBox.Name = "playlistComboBox";
            playlistComboBox.Size = new Size(151, 23);
            playlistComboBox.TabIndex = 11;
            // 
            // trackBar
            // 
            trackBar.Location = new Point(100, 143);
            trackBar.Maximum = 100;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(284, 45);
            trackBar.TabIndex = 12;
            // 
            // VolumeTrackBar
            // 
            VolumeTrackBar.Location = new Point(350, 262);
            VolumeTrackBar.Maximum = 100;
            VolumeTrackBar.Name = "VolumeTrackBar";
            VolumeTrackBar.Size = new Size(104, 45);
            VolumeTrackBar.TabIndex = 13;
            VolumeTrackBar.Value = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(273, 274);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 14;
            label1.Text = "Sound Level";
            // 
            // DurationLabel
            // 
            DurationLabel.AutoSize = true;
            DurationLabel.Location = new Point(217, 173);
            DurationLabel.Name = "DurationLabel";
            DurationLabel.Size = new Size(0, 15);
            DurationLabel.TabIndex = 15;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(466, 308);
            Controls.Add(DurationLabel);
            Controls.Add(label1);
            Controls.Add(VolumeTrackBar);
            Controls.Add(trackBar);
            Controls.Add(playlistComboBox);
            Controls.Add(playlistCountLabel);
            Controls.Add(selectedFileLabel);
            Controls.Add(NextButton);
            Controls.Add(PreviousButton);
            Controls.Add(PauseButton);
            Controls.Add(PlayButton);
            Controls.Add(menuStrip2);
            ForeColor = SystemColors.InfoText;
            Name = "Form1";
            Text = "Media Player";
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip2;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private Button PlayButton;
        private Button PauseButton;
        private Button PreviousButton;
        private Button NextButton;
       // private TrackBar TrackBarUpdateTimerTick;
        private Label selectedFileLabel;
        private Label playlistCountLabel;
        private ComboBox playlistComboBox;
        private TrackBar trackBar;
        private TrackBar VolumeTrackBar;
        private Label label1;
        private Label DurationLabel;
    }
}
