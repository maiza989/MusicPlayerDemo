using NAudio.Wave;
using System.Media;
using Timer = System.Windows.Forms.Timer;
using NAudio.CoreAudioApi;


namespace MusicPlayerDemo
{
    public partial class Form1 : Form
    {
        private IWavePlayer wavePlayer; // audio player
        private AudioFileReader audioFileReader;
        private List<string> playlist;
        private int currentTrackIndex = 0; 
        private WaveChannel32 volumeStream; // audio volume
        private Timer trackBarUpdateTimer; // audio time tracker
        private MMDevice defaultPlaybackDevice; // System volume  

        public Form1()
        {
            InitializeComponent();
            wavePlayer = new WaveOutEvent(); // Initialize WaveOutEvent
            playlist = new List<string>();

            // Initialize the Timer for updating the TrackBar
            trackBarUpdateTimer = new Timer();
            trackBarUpdateTimer.Interval = 1000; // Set the interval in milliseconds (adjust as needed)
            trackBarUpdateTimer.Tick += TrackBarUpdateTimerTick;

            // Subscribe to the Scroll event of the volume trackbar
            VolumeTrackBar.Scroll += volumeTrackBarScroll;
        }
      
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

                    // Stop the timer when audio playback stops
                    trackBarUpdateTimer.Stop();
                }

                // Initialize new resources
                
                audioFileReader = new AudioFileReader(playlist[currentTrackIndex]);
                volumeStream = new WaveChannel32(audioFileReader); // Wrap AudioFileReader in WaveChannel32
                wavePlayer = new WaveOut();
                wavePlayer.Init(audioFileReader);
                wavePlayer.Play();

                // Initialize the default playback device
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                // Start the timer when audio playback starts
                trackBarUpdateTimer.Start();
            }
        }
        private void UpdatePlaylistCountLabel()
        {
            // Update the label to display the number of audio files in the playlist
            playlistCountLabel.Text = "Playlist Count: " + playlist.Count.ToString();
        }
        private void UpdateNextPreviousButtons()
        {
            // Enable/disable next/previous buttons based on the current track index
            NextButton.Enabled = currentTrackIndex < playlist.Count - 1;
            PreviousButton.Enabled = currentTrackIndex > 0;
        }

        private void UpdatePlaylistComboBox()
        {
            // Update the ComboBox with the playlist
            playlistComboBox.Items.Clear();
            // Use LINQ to extract only the file names
            var fileNames = playlist.Select(Path.GetFileNameWithoutExtension).ToArray();
            playlistComboBox.Items.AddRange(fileNames);
            playlistComboBox.SelectedIndex = currentTrackIndex;
        }
        private void UpdateVolume(float volume)
        {
            if (defaultPlaybackDevice != null)
            {
                defaultPlaybackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
            }
        }
        private void volumeTrackBarScroll(object sender, EventArgs e)
        {
            // Calculate the volume from the trackbar value (0 to 100)
            float volume = VolumeTrackBar.Value / 100f;
            UpdateVolume(volume);

        }
        private void playlistComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle selection change in the ComboBox
            int selectedIndex = playlistComboBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < playlist.Count)
            {
                currentTrackIndex = selectedIndex;

                // Set the label text to the selected file name
                selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);

                // Reset the TrackBar position
                trackBar.Value = 0;
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                // Update audio label
                UpdatePlaylistCountLabel();
                // Play audio file
                PlayCurrentTrack();
            }
        }
        private void TrackBarUpdateTimerTick(object sender, EventArgs e)
        {
            if (audioFileReader != null)
            {
                // Update the TrackBar position based on the audio playback position
                trackBar.Value = (int)(audioFileReader.Position / (double)audioFileReader.Length * trackBar.Maximum);
            }
        }


        //---------------------------------------------------------------------------------------------------------------
        //                                      BUTTONS
        //---------------------------------------------------------------------------------------------------------------

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.wav;*.mp3;*.ogg;*.flac;*.aac|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Add the selected files to the playlist history
                playlist.AddRange(openFileDialog.FileNames);

                // Update the ComboBox with the playlist
                UpdatePlaylistComboBox();

                currentTrackIndex = playlist.Count - 1;

                // Set the label text to the selected file name
                if (playlist.Count > 0)
                {
                    selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
                }
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
        }
        private void exitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void PlayButtonClick(object sender, EventArgs e)
        {
            wavePlayer.Play();
        }//
        private void PauseButtonClick(object sender, EventArgs e)
        {
            wavePlayer.Stop();
        }// end of PasueButtonClick
        private void PreviousButtonClick(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;

            // Set the label text to the selected file name
            if (playlist.Count > 0)
            {
                selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }  
        }// end of PreviousButtonClick
        private void NextButtonClick(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;

            // Set the label text to the selected file name
            if (playlist.Count > 0)
            {
                selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
            
        }// end of NextButtonClick

    }
}// end of class
