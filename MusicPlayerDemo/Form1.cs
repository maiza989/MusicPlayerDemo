/*
 * ## TODO
 *  
 * 
 * ---
 * ## DONE
 * Theme Customization: Allow users to choose different themes or customize the appearance of the application
 * ---
 * Show Album cover.
 * ---
 * Track Fast foward and backward: Allow users to fast foward the audio by 15 seconds or backward by 15 seconds. 
 * ---
 * ## WAIT LIST
 * Volume Fading:
 * Smoothly fade the volume in and out when starting or stopping a track to prevent abrupt changes in audio levels. 
 * ---
 * Audio Visualization:
 * Include a visualizer to provide a dynamic representation of the audio being played.
 * 
 */
using NAudio.Wave;
using System.Media;
using Timer = System.Windows.Forms.Timer;
using NAudio.CoreAudioApi;
using NAudio.Gui;
using TagLib;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using File = System.IO.File;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;




namespace MusicPlayerDemo
{
    public partial class Form1 : Form
    {

        private Random random = new Random();
        private List<string> playlist;
        private int currentTrackIndex = 0;
        private const int fadingDurationMilliseconds = 1000;        // Adjust fading duration as needed
        private const int fadingIntervalMilliseconds = 100;         // Adjust fading interval as needed
        private const int fadingThresholdMilliseconds = 1000;       // Threshold to start fading (1 second before the end)


        private IWavePlayer wavePlayer;                             // audio player
        private AudioFileReader audioFileReader;
        private WaveChannel32 volumeStream;                         // audio volume
        private Timer trackBarUpdateTimer;                          // audio time tracker
        private MMDevice defaultPlaybackDevice;                     // System volume  

        private bool isLooping = false;                             // loop feature
        private bool isShuffle = false;                             // shuffle feature
        private bool isSeeking = false;                             // Seeking feature
        private bool isDarkMode = false;                            // Dark Theme feature


        public Form1()
        {
            InitializeComponent();
            SetDarkMode(this);
            FetchSystemVolumeLevel();
            wavePlayer = new WaveOutEvent();                           // Initialize WaveOutEvent
            playlist = new List<string>();

            this.FormBorderStyle = FormBorderStyle.Fixed3D;            // Set the form's border style to fixed order
            this.MinimumSize = new Size(450, 350);                     // Adjust the dimensions as needed (

            // Initialize the Timer for updating the TrackBar
            trackBarUpdateTimer = new Timer();
            trackBarUpdateTimer.Interval = 100;                        // Set the interval in milliseconds (adjust as needed)
            trackBarUpdateTimer.Tick += TrackBarUpdateTimerTick;

            VolumeTrackBar.Scroll += VolumeTrackBarScroll;                                         // Subscribe to the Scroll event of the volume trackbar
            playlistComboBox.SelectedIndexChanged += PlaylistComboBoxSelectedIndexChanged;         // Subscribe to Combobox event.
            LoopingCheckBox.CheckedChanged += LoopingCheckBoxCheckedChanged;                       // Subscribe to Checked box changed event for looping. 
            ShuffleButton.CheckedChanged += ShuffleCheckBoxCheckedChanged;                         // Sbuscribe to check box changed event for shuffle
            trackBar.MouseUp += SeekingTrackBarMouseUp;                                            // subscribe to MouseUp event for audio trackBar seeking feature.
            trackBar.Scroll += SeekingTrackBarScroll;                                              // subscribe to Scroll event for audio trackBar seeking feature.

            // Set anchor properties for UI controls
            trackBar.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            DurationLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            PlayButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            PauseButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            PreviousButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            NextButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            LoopingCheckBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            ShuffleButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            VolumeTrackBar.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            SoundLevelLabel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

            playlistComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            playlistCountLabel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            selectedFileLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

        }// end of Form1

        //------------------------------------------------------------------------------------------------------
        //                                             METHODS
        //------------------------------------------------------------------------------------------------------

        private void PlayCurrentTrack()
        {

            if (playlist != null && playlist.Count > 0 && currentTrackIndex < playlist.Count)
            {
                if (audioFileReader != null)
                {
                    // Stop and dispose of the current resources
                    wavePlayer.Stop();
                    wavePlayer.Dispose();
                    audioFileReader.Dispose();
                    trackBarUpdateTimer.Stop();
                }
                // Initialize new resources  
                audioFileReader = new AudioFileReader(playlist[currentTrackIndex]);
                volumeStream = new WaveChannel32(audioFileReader);                              // Wrap AudioFileReader in WaveChannel32
                wavePlayer = new WaveOut();
                wavePlayer.Init(audioFileReader);

                /*
                 * An iff statement to check if the loop or shuffle button is checked
                 * If so subscribe to the method of the buttoned check and fetch the track infromation.
                 */
                if (isLooping)
                {
                    wavePlayer.PlaybackStopped += LoopCurrentTrack;
                    FetchTrackInfo(playlist[currentTrackIndex]);
                }
                else if (isShuffle && playlist.Count > 1)
                {
                    wavePlayer.PlaybackStopped += ShuffleNextTrack;
                    FetchTrackInfo(playlist[currentTrackIndex]);
                }
                wavePlayer.Play();

                // Initialize the default playback device
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                trackBar.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;                                                  // Set the maximum value of the TrackBar to the total duration of the audio file
                TimeSpan totalDuration = audioFileReader.TotalTime;                                                              // Set the durationLabel text based on the total duration of the audio file
                DurationLabel.Text = $"{totalDuration.Hours:D2}:{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";

                trackBarUpdateTimer.Start();

            }
        }// end of PlayCurrentTrack

        private void FetchTrackInfo(string filePath)
        {

            try
            {

                TagLib.File file = TagLib.File.Create(filePath);                             // load audio file

                //extract the artist name from metadata
                string artist = file.Tag.FirstPerformer;
                string title = file.Tag.Title;

                // conditonal operation to check if the artist or title are null/empty.
                ArtistNameLabel.TextAlign = ContentAlignment.MiddleCenter;
                ArtistNameLabel.Text = string.IsNullOrEmpty(artist) ? "Unknown" : artist;
                selectedFileLabel.TextAlign = ContentAlignment.MiddleCenter;
                selectedFileLabel.Text = string.IsNullOrEmpty(title) ? System.IO.Path.GetFileNameWithoutExtension(filePath) : title;


                if (file.Tag.Pictures.Length > 0)                                                       // Extract album art if available
                {
                    var picture = file.Tag.Pictures[0];                                                 // Get the first picture
                    using (MemoryStream ms = new MemoryStream(picture.Data.Data))                       // Convert album art to Image
                    {
                        Image albumArt = Image.FromStream(ms);

                        AlbumPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        AlbumPictureBox.Image = ResizeImage(albumArt, AlbumPictureBox.Size);            // Resize Image and display it. 
                    }
                }
                else
                {
                    Console.WriteLine("No Image was found");
                    AlbumPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;                         // No album cover is found , set it to defualt image
                    AlbumPictureBox.Image = Properties.Resources.ResourceManager.GetObject("Hmmm") as Image;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting metadata for artist name" + ex.Message);
                ArtistNameLabel.Text = "Unknown";
                selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
            }

        }// end of FetchTrackInfo

        /**
         * This method resizes an image to fit within a specified size while maintaining its aspect ratio.
         */
        private Image ResizeImage(Image image, Size size)
        {
            int targetWidth, targetHeight;
            double aspectRatio = (double)image.Width / image.Height;

            if (image.Width > image.Height)
            {
                targetWidth = size.Width;
                targetHeight = (int)(size.Width / aspectRatio);
            }
            else
            {
                targetHeight = size.Height;
                targetWidth = (int)(size.Height * aspectRatio);
            }

            Bitmap resizedImage = new Bitmap(targetWidth, targetHeight);                   // Create a new bitmap with the calculated target width and height       

            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, targetWidth, targetHeight);                // Use Graphics object to draw the original image onto the resized bitmap
            }

            return resizedImage;
        }// end of ResizeImage

        private void FetchSystemVolumeLevel()
        {

            // Fetch and set the default system audio volume
            defaultPlaybackDevice = (new MMDeviceEnumerator()).GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            float defaultVolume = defaultPlaybackDevice.AudioEndpointVolume.MasterVolumeLevelScalar;

            // Set the initial value of the VolumeTrackBar based on the default system volume
            int trackBarValue = (int)(defaultVolume * 100);
            VolumeTrackBar.Value = trackBarValue;

        }// end of FetchSystemVolumeLevel

        private void UpdatePlaylistCountLabel()
        {

            playlistCountLabel.Text = "Playlist Count: " + playlist.Count.ToString();               // Update the label to display the number of audio files in the playlist

        }// end of UpdatePlaylistCountLabel

        private void UpdateNextPreviousButtons()
        {

            // Enable/disable next/previous buttons based on the current track index
            NextButton.Enabled = currentTrackIndex < playlist.Count - 1;
            PreviousButton.Enabled = currentTrackIndex > 0;

        }// end of UpdateNextPreviousButtons

        private void UpdatePlaylistComboBox(int currentIndex)
        {

            playlistComboBox.Items.Clear();
            var fileNames = playlist.Select(Path.GetFileNameWithoutExtension).ToArray();            // Extract only the file names
            playlistComboBox.Items.AddRange(fileNames);
            playlistComboBox.SelectedIndex = currentIndex;

        }// end of UpdatePlaylistComboBox

        private void UpdateVolume(float volume)
        {

            if (defaultPlaybackDevice != null)
            {
                defaultPlaybackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
            }

        }// end of UpdateVolume

        private void UpdateUI()
        {
            // Update the ComboBox with the playlist
            UpdatePlaylistComboBox(currentTrackIndex);
            currentTrackIndex = playlist.Count - 1;

            if (playlist.Count > 0)
            {
                FetchTrackInfo(playlist[currentTrackIndex]);            // Set the label text to the selected file name
            }

            UpdatePlaylistCountLabel();
            UpdateNextPreviousButtons();
            PlayCurrentTrack();
        }
        private void VolumeTrackBarScroll(object sender, EventArgs e)
        {


            float volume = VolumeTrackBar.Value / 100f;                                             // Calculate the volume from the trackbar value (0 to 100)
            UpdateVolume(volume);

        }// end of VolumeTrackBarScroll

        private void PlaylistComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {

            // Handle selection change in the ComboBox
            int selectedIndex = playlistComboBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < playlist.Count)
            {
                currentTrackIndex = selectedIndex;
                FetchTrackInfo(playlist[currentTrackIndex]);
                trackBar.Value = 0;
                UpdateNextPreviousButtons();
                UpdatePlaylistCountLabel();
            }
            if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing)            // Stop the current track if playing
            {
                wavePlayer.Stop();
            }

            PlayCurrentTrack();
        }// end of PlaylistComboBoxSelectedIndexChanged

        private void TrackBarUpdateTimerTick(object sender, EventArgs e)
        {

            if (audioFileReader != null && wavePlayer.PlaybackState == PlaybackState.Playing)
            {
                // Update the TrackBar position based on the audio playback position
                int currentPosition = (int)(audioFileReader.CurrentTime.TotalSeconds);
                trackBar.Value = currentPosition;

                // Update the DurationLabel to count down
                TimeSpan remainingTime = audioFileReader.TotalTime - audioFileReader.CurrentTime;
                DurationLabel.Text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";

                if (remainingTime.TotalSeconds <= 0)
                {
                    trackBarUpdateTimer.Stop();
                    wavePlayer.Stop();
                }
            }

        }// end of TrackBarUpdateTimerTick

        private void SeekingTrackBarScroll(object sender, EventArgs e)
        {
            // Calculate the position to seek to based on the TrackBar value
            if (audioFileReader != null)
            {

                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Paused)          // Check if the audio file reader is playing or paused before seeking
                {

                    if (wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        wavePlayer.Pause();                                                                                         // Pause playback while seeking to avoid audio glitches
                    }

                    // Seek to the desired position
                    TimeSpan newPosition = TimeSpan.FromSeconds(trackBar.Value);
                    audioFileReader.CurrentTime = newPosition;

                    isSeeking = true;                                                   // set seeking flag to true
                }
            }
        }// end of SeekingTrackBarScroll

        private void SeekingTrackBarMouseUp(object sender, MouseEventArgs e)
        {

            if (isSeeking && wavePlayer.PlaybackState == PlaybackState.Paused)
            {
                wavePlayer.Play();                                                      // Resume playback if it was paused due to seeking
            }

            isSeeking = false;                                                          // Reset the seeking flag

        }
        private void ShuffleNextTrack(object sender, StoppedEventArgs e)
        {

            if (isShuffle)
            {
                int nextIndex;
                do
                {
                    nextIndex = random.Next(0, playlist.Count);                         // Choose a random track index different from the current one
                } while (nextIndex == currentTrackIndex);

                currentTrackIndex = nextIndex;
            }
            else
            {
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;           // Move to the next track in a sequential order
            }

            PlayCurrentTrack();

        }// end of ShuffleNextTrack

        private void LoopCurrentTrack(object sender, StoppedEventArgs e)
        {

            if (isLooping)
            {

                if (e.Exception != null)
                {
                    Console.WriteLine("Playback stopped with exception: " + e.Exception.Message);           // If the playback stopped due to an exception, handle it here
                }
                PlayCurrentTrack();

            }

        }// end of LoopCUrrentTrack

        private void LoopingCheckBoxCheckedChanged(object sender, EventArgs e)
        {

            isLooping = LoopingCheckBox.Checked;
            isShuffle = false;

        }// end of LoopingCheckBoxCheckedChanged

        private void ShuffleCheckBoxCheckedChanged(object sender, EventArgs e)
        {

            isShuffle = ShuffleButton.Checked;
            isLooping = false;

        }// end of ShuffleCheckBoxCheckedChanged

        private void SetDarkMode(Control control)
        {

            //this.BackColor = Color.FromArgb(40, 40, 40);                            // Set dark mode colors for UI elements


            control.BackColor = Color.FromArgb(40, 40, 40);
            control.ForeColor = Color.White;
            foreach (Control c in control.Controls)
            {
                SetDarkMode(c);
            }


            /* PlayButton.BackColor = Color.FromArgb(64, 64, 64);
             PlayButton.ForeColor = Color.White;
             PauseButton.BackColor = Color.FromArgb(64, 64, 64);
             PauseButton.ForeColor = Color.White;
             PreviousButton.BackColor = Color.FromArgb(64, 64,64);
             PreviousButton.ForeColor = Color.White;
             NextButton.BackColor = Color.FromArgb(64, 64, 64);
             NextButton.ForeColor = Color.White;
             playlistComboBox.BackColor = Color.FromArgb(64, 64, 64);
             playlistComboBox.ForeColor = Color.White;


             ShuffleButton.ForeColor = Color.White;
             LoopingCheckBox.ForeColor = Color.White;
             SoundLevelLabel.ForeColor = Color.White;
             playlistCountLabel.ForeColor = Color.White; 
             PlaylistLabel.ForeColor = Color.White;
             selectedFileLabel.ForeColor = Color.White;
             ArtistNameLabel.ForeColor = Color.White;
             DurationLabel.ForeColor = Color.White;*/


        }// end of SetDarkMode

        private void SetLightMode(Control control)
        {

            // this.BackColor = SystemColors.Control;                                  // Set light mode colors for UI elements

            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;

            foreach (Control c in control.Controls)
            {
                SetLightMode(c);
            }

            /* PlayButton.BackColor = SystemColors.Control;
             PlayButton.ForeColor = SystemColors.ControlText;
             PauseButton.BackColor = SystemColors.Control;
             PauseButton.ForeColor = SystemColors.ControlText;
             PreviousButton.BackColor = SystemColors.Control;
             PreviousButton.ForeColor = SystemColors.ControlText;
             NextButton.BackColor = SystemColors.Control;
             NextButton.ForeColor = SystemColors.ControlText;
             playlistComboBox.BackColor = SystemColors.Control;
             playlistComboBox.ForeColor = SystemColors.ControlText;

             ShuffleButton.ForeColor = SystemColors.ControlText;
             LoopingCheckBox.ForeColor = SystemColors.ControlText;
             SoundLevelLabel.ForeColor = SystemColors.ControlText;
             playlistCountLabel.ForeColor = SystemColors.ControlText;
             PlaylistLabel.ForeColor = SystemColors.ControlText;
             selectedFileLabel.ForeColor = SystemColors.ControlText;
             ArtistNameLabel.ForeColor = SystemColors.ControlText;
             DurationLabel.ForeColor = SystemColors.ControlText;
            */
        }// end of SetLightMode

        private void CreatePlaylist()                                                   // Method to create a new playlist
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Playlist Files (*.m3u)|*.m3u";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string playlistName = saveFileDialog.FileName;
                // You can save the playlist name or do further processing here
            }

        }// end of CreatePlaylist


        private void SavePlaylist()                                                     // Method to save the current playlist
        {

            if (playlist.Count == 0)
            {
                MessageBox.Show("Playlist is empty. Add tracks to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Playlist Files (*.m3u)|*.m3u";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string playlistFilePath = saveFileDialog.FileName;
                File.WriteAllLines(playlistFilePath, playlist);
            }

        }// end of SavePlaylist


        private void LoadPlaylist()                                                     // Method to load a playlist
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Playlist Files (*.m3u)|*.m3u";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string playlistFilePath = openFileDialog.FileName;
                playlist.Clear();
                playlist.AddRange(File.ReadAllLines(playlistFilePath));
                // You can do further processing with the loaded playlist here
            }

        }// end of LoadPlaylist

        /*private void RemoveTracksFromPlaylist()
        {
            // Example: Remove selected tracks from the playlist
            foreach (int selectedIndex in PlayListListBox.SelectedIndices)
            {
                playlist.RemoveAt(selectedIndex);
            }
            // Update UI or perform other actions after removing tracks
        }*/

        /*private async Task FadeOutVolume()
        {
            float initialVolume = VolumeTrackBar.Value / 100f;
            float targetVolume = 0.0f; // Mute
            float volumeDecrement = (initialVolume - targetVolume) / (fadingDurationMilliseconds / fadingIntervalMilliseconds);

            for (float volume = initialVolume; volume > targetVolume; volume -= volumeDecrement)
            {
                UpdateVolume(volume);
                await Task.Delay(fadingIntervalMilliseconds);
            }
        }*/

        //---------------------------------------------------------------------------------------------------------------
        //                                      BUTTONS
        //---------------------------------------------------------------------------------------------------------------

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.wav;*.mp3;*.ogg;*.flac;*.aac|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                foreach (var filePath in openFileDialog.FileNames)          // Add the selected files to the playlist history
                {
                    if (!playlist.Contains(filePath))                       // Check if the file is not already in the playlist before adding
                    {
                        playlist.Add(filePath);                             // Add the selected file to the playlist
                    }
                }
                UpdateUI();                                                 // Call to update UI 
            }
        }// end of openToolStripMenuItemClick
        private void exitToolStripMenuItemClick(object sender, EventArgs e)
        {

            Application.Exit();

        }// end of exitToolStripMenuItemClick

        private void PlayButtonClick(object sender, EventArgs e)
        {
            wavePlayer.Play();
            if (wavePlayer.PlaybackState == PlaybackState.Stopped)
            {
                PlayCurrentTrack();
            }

        }//end of PlayButtonClick

        private void PauseButtonClick(object sender, EventArgs e)
        {

            wavePlayer.Pause();

        }// end of PasueButtonClick

        private void PreviousButtonClick(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;          // Move to the preivous track

            if (playlist.Count > 0)
            {
                FetchTrackInfo(playlist[currentTrackIndex]);
                UpdatePlaylistCountLabel();
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
        }// end of PreviousButtonClick

        private void NextButtonClick(object sender, EventArgs e)
        {

            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;                           // Move to the next track
            if (playlist.Count > 0)
            {
                FetchTrackInfo(playlist[currentTrackIndex]);
                UpdatePlaylistCountLabel();
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }

        }// end of NextButtonClick

        private void DarkModeButtonClick(object sender, EventArgs e)
        {

            if (isDarkMode)
            {
                SetDarkMode(this);
            }
            else
            {
                SetLightMode(this);
            }
            isDarkMode = !isDarkMode;

        }// end of DarkModeButtonClick

        private void BackwardButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (audioFileReader != null)
                {

                    TimeSpan newTime = audioFileReader.CurrentTime - TimeSpan.FromSeconds(15);            // Rewind Track by 15 seconds
                    if (newTime >= TimeSpan.Zero)
                    {
                        audioFileReader.CurrentTime = newTime;
                    }
                    else
                    {
                        audioFileReader.CurrentTime = TimeSpan.Zero;
                        PlayCurrentTrack();                                                               // Play current track in when reach start of the track
                    }
                }
                else
                {
                    wavePlayer.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Issue has occured while using the backward button: {ex.Message}");      // Stop Audio when error happen
                audioFileReader.CurrentTime = TimeSpan.Zero;
                wavePlayer.Stop();
                //PreviousButtonClick(sender, e);
            }
        }// end of BackwardButtonClick

        private void FastFowardButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (audioFileReader != null)
                {
                    TimeSpan newTime = audioFileReader.CurrentTime + TimeSpan.FromSeconds(15);            // Fast-forward Track by 15 seconds
                    if (newTime <= audioFileReader.TotalTime)
                    {
                        audioFileReader.CurrentTime = newTime;
                    }
                    else
                    {
                        audioFileReader.CurrentTime = audioFileReader.TotalTime;
                        NextButtonClick(sender, e);                                                       // Go to the next track when Fast-forwarding
                    }
                }
                else
                {
                    wavePlayer.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Issue has occured while using the fast-forward button: {ex.Message}");   // Stop Audio when error happen
                audioFileReader.CurrentTime = audioFileReader.TotalTime;
                wavePlayer.Stop();
                //NextButtonClick(sender, e);
            }
        }// end of FastFowardButtonClick

        private void savePlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePlaylist();
        }

        private void loadPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPlaylist();
            UpdateUI();

        }
    }// end of partial call Form1
}// end of namespace
