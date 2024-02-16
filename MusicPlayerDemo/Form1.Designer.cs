namespace MusicPlayerDemo
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
            DarkModeButton = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            playListToolStripMenuItem = new ToolStripMenuItem();
            savePlaylistToolStripMenuItem = new ToolStripMenuItem();
            loadPlaylistToolStripMenuItem = new ToolStripMenuItem();
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
            SoundLevelLabel = new Label();
            DurationLabel = new Label();
            LoopingCheckBox = new CheckBox();
            ShuffleButton = new CheckBox();
            PlaylistLabel = new Label();
            ArtistNameLabel = new Label();
            AlbumPictureBox = new PictureBox();
            BackwardButton = new Button();
            FastFowardButton = new Button();
            saveFileDialog1 = new SaveFileDialog();
            removeTrackToolStripMenuItem = new ToolStripMenuItem();
            menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AlbumPictureBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, playListToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(497, 24);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, DarkModeButton, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(132, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // DarkModeButton
            // 
            DarkModeButton.Name = "DarkModeButton";
            DarkModeButton.Size = new Size(132, 22);
            DarkModeButton.Text = "Dark Mode";
            DarkModeButton.Click += DarkModeButtonClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(132, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItemClick;
            // 
            // playListToolStripMenuItem
            // 
            playListToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { savePlaylistToolStripMenuItem, loadPlaylistToolStripMenuItem, removeTrackToolStripMenuItem });
            playListToolStripMenuItem.Name = "playListToolStripMenuItem";
            playListToolStripMenuItem.Size = new Size(59, 20);
            playListToolStripMenuItem.Text = "PlayList";
            // 
            // savePlaylistToolStripMenuItem
            // 
            savePlaylistToolStripMenuItem.Name = "savePlaylistToolStripMenuItem";
            savePlaylistToolStripMenuItem.Size = new Size(180, 22);
            savePlaylistToolStripMenuItem.Text = "Save Playlist";
            savePlaylistToolStripMenuItem.Click += savePlaylistToolStripMenuItem_Click;
            // 
            // loadPlaylistToolStripMenuItem
            // 
            loadPlaylistToolStripMenuItem.Name = "loadPlaylistToolStripMenuItem";
            loadPlaylistToolStripMenuItem.Size = new Size(180, 22);
            loadPlaylistToolStripMenuItem.Text = "Load Playlist";
            loadPlaylistToolStripMenuItem.Click += loadPlaylistToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // PlayButton
            // 
            PlayButton.Location = new Point(112, 396);
            PlayButton.Name = "PlayButton";
            PlayButton.Size = new Size(99, 23);
            PlayButton.TabIndex = 2;
            PlayButton.Text = "Play";
            PlayButton.UseVisualStyleBackColor = true;
            PlayButton.Click += PlayButtonClick;
            // 
            // PauseButton
            // 
            PauseButton.Location = new Point(285, 396);
            PauseButton.Name = "PauseButton";
            PauseButton.Size = new Size(99, 23);
            PauseButton.TabIndex = 3;
            PauseButton.Text = "Pause";
            PauseButton.UseVisualStyleBackColor = true;
            PauseButton.Click += PauseButtonClick;
            // 
            // PreviousButton
            // 
            PreviousButton.Location = new Point(0, 344);
            PreviousButton.Name = "PreviousButton";
            PreviousButton.Size = new Size(61, 23);
            PreviousButton.TabIndex = 5;
            PreviousButton.Text = "Previous";
            PreviousButton.UseVisualStyleBackColor = true;
            PreviousButton.Click += PreviousButtonClick;
            // 
            // NextButton
            // 
            NextButton.BackColor = SystemColors.Window;
            NextButton.Location = new Point(432, 344);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(62, 23);
            NextButton.TabIndex = 6;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = false;
            NextButton.Click += NextButtonClick;
            // 
            // selectedFileLabel
            // 
            selectedFileLabel.AutoSize = true;
            selectedFileLabel.Location = new Point(112, 315);
            selectedFileLabel.MaximumSize = new Size(400, 30);
            selectedFileLabel.Name = "selectedFileLabel";
            selectedFileLabel.Size = new Size(0, 15);
            selectedFileLabel.TabIndex = 8;
            selectedFileLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playlistCountLabel
            // 
            playlistCountLabel.AutoSize = true;
            playlistCountLabel.Location = new Point(12, 24);
            playlistCountLabel.Name = "playlistCountLabel";
            playlistCountLabel.Size = new Size(0, 15);
            playlistCountLabel.TabIndex = 10;
            playlistCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playlistComboBox
            // 
            playlistComboBox.FormattingEnabled = true;
            playlistComboBox.Location = new Point(52, 47);
            playlistComboBox.Name = "playlistComboBox";
            playlistComboBox.Size = new Size(151, 23);
            playlistComboBox.TabIndex = 11;
            // 
            // trackBar
            // 
            trackBar.Location = new Point(100, 345);
            trackBar.Maximum = 100;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(284, 45);
            trackBar.TabIndex = 12;
            // 
            // VolumeTrackBar
            // 
            VolumeTrackBar.Location = new Point(390, 448);
            VolumeTrackBar.Maximum = 100;
            VolumeTrackBar.Name = "VolumeTrackBar";
            VolumeTrackBar.Size = new Size(104, 45);
            VolumeTrackBar.TabIndex = 13;
            VolumeTrackBar.Value = 10;
            // 
            // SoundLevelLabel
            // 
            SoundLevelLabel.AutoSize = true;
            SoundLevelLabel.Location = new Point(313, 460);
            SoundLevelLabel.Name = "SoundLevelLabel";
            SoundLevelLabel.Size = new Size(71, 15);
            SoundLevelLabel.TabIndex = 14;
            SoundLevelLabel.Text = "Sound Level";
            // 
            // DurationLabel
            // 
            DurationLabel.AutoSize = true;
            DurationLabel.Location = new Point(229, 375);
            DurationLabel.Name = "DurationLabel";
            DurationLabel.Size = new Size(0, 15);
            DurationLabel.TabIndex = 15;
            // 
            // LoopingCheckBox
            // 
            LoopingCheckBox.AutoSize = true;
            LoopingCheckBox.Location = new Point(269, 437);
            LoopingCheckBox.Name = "LoopingCheckBox";
            LoopingCheckBox.Size = new Size(53, 19);
            LoopingCheckBox.TabIndex = 16;
            LoopingCheckBox.Text = "Loop";
            LoopingCheckBox.UseVisualStyleBackColor = true;
            // 
            // ShuffleButton
            // 
            ShuffleButton.AutoSize = true;
            ShuffleButton.Location = new Point(166, 437);
            ShuffleButton.Name = "ShuffleButton";
            ShuffleButton.Size = new Size(63, 19);
            ShuffleButton.TabIndex = 17;
            ShuffleButton.Text = "Shuffle";
            ShuffleButton.UseVisualStyleBackColor = true;
            // 
            // PlaylistLabel
            // 
            PlaylistLabel.AutoSize = true;
            PlaylistLabel.Location = new Point(0, 50);
            PlaylistLabel.Name = "PlaylistLabel";
            PlaylistLabel.Size = new Size(47, 15);
            PlaylistLabel.TabIndex = 18;
            PlaylistLabel.Text = "Playlist:";
            // 
            // ArtistNameLabel
            // 
            ArtistNameLabel.AutoSize = true;
            ArtistNameLabel.Font = new Font("Segoe UI", 7F);
            ArtistNameLabel.Location = new Point(112, 330);
            ArtistNameLabel.MaximumSize = new Size(100, 15);
            ArtistNameLabel.Name = "ArtistNameLabel";
            ArtistNameLabel.Size = new Size(0, 12);
            ArtistNameLabel.TabIndex = 19;
            ArtistNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlbumPictureBox
            // 
            AlbumPictureBox.Location = new Point(130, 77);
            AlbumPictureBox.Name = "AlbumPictureBox";
            AlbumPictureBox.Size = new Size(244, 235);
            AlbumPictureBox.TabIndex = 20;
            AlbumPictureBox.TabStop = false;
            // 
            // BackwardButton
            // 
            BackwardButton.Location = new Point(64, 344);
            BackwardButton.Name = "BackwardButton";
            BackwardButton.Size = new Size(38, 23);
            BackwardButton.TabIndex = 21;
            BackwardButton.Text = "-15";
            BackwardButton.UseVisualStyleBackColor = true;
            BackwardButton.Click += BackwardButtonClick;
            // 
            // FastFowardButton
            // 
            FastFowardButton.Location = new Point(390, 345);
            FastFowardButton.Name = "FastFowardButton";
            FastFowardButton.Size = new Size(36, 23);
            FastFowardButton.TabIndex = 22;
            FastFowardButton.Text = "+15";
            FastFowardButton.UseVisualStyleBackColor = true;
            FastFowardButton.Click += FastFowardButtonClick;
            // 
            // removeTrackToolStripMenuItem
            // 
            removeTrackToolStripMenuItem.Name = "removeTrackToolStripMenuItem";
            removeTrackToolStripMenuItem.Size = new Size(180, 22);
            removeTrackToolStripMenuItem.Text = "Remove Track ";
            removeTrackToolStripMenuItem.Click += removeTrackToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(497, 493);
            Controls.Add(FastFowardButton);
            Controls.Add(BackwardButton);
            Controls.Add(AlbumPictureBox);
            Controls.Add(ArtistNameLabel);
            Controls.Add(PlaylistLabel);
            Controls.Add(ShuffleButton);
            Controls.Add(LoopingCheckBox);
            Controls.Add(DurationLabel);
            Controls.Add(SoundLevelLabel);
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
            ((System.ComponentModel.ISupportInitialize)AlbumPictureBox).EndInit();
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
        private Label SoundLevelLabel;
        private Label DurationLabel;
        private CheckBox LoopingCheckBox;
        private CheckBox ShuffleButton;
        private Label PlaylistLabel;
        private Label ArtistNameLabel;
        private ToolStripMenuItem DarkModeButton;
        private PictureBox AlbumPictureBox;
        private Button BackwardButton;
        private Button FastFowardButton;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem playListToolStripMenuItem;
        private ToolStripMenuItem savePlaylistToolStripMenuItem;
        private ToolStripMenuItem loadPlaylistToolStripMenuItem;
        private ToolStripMenuItem removeTrackToolStripMenuItem;
    }
}
