/*
 * TODO#
 * Display Track Information:
 * Show metadata information such as artist, album, and track name. You can fetch this information from the audio file tags.
 * ---
 * Seeking in Track:
 * Allow users to seek within a track by clicking on a specific position on the track progress bar.
 * ---
 * 
 */


using NAudio.Wave;
using System.Media;
using Timer = System.Windows.Forms.Timer;
using NAudio.CoreAudioApi;
using NAudio.Gui;


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
        private bool isLooping = false; // loop feature
        private bool isShuffle = false; // shuffle feature
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            wavePlayer = new WaveOutEvent(); // Initialize WaveOutEvent
            playlist = new List<string>();

            // Initialize the Timer for updating the TrackBar
            trackBarUpdateTimer = new Timer();
            trackBarUpdateTimer.Interval = 100; // Set the interval in milliseconds (adjust as needed)
            trackBarUpdateTimer.Tick += TrackBarUpdateTimerTick;

            // Subscribe to the Scroll event of the volume trackbar
            VolumeTrackBar.Scroll += VolumeTrackBarScroll;
            FetchSystemVolumeLevel();
            // Subscribe to Combobox event.
            playlistComboBox.SelectedIndexChanged += PlaylistComboBoxSelectedIndexChanged;
            // Subscribe to Checked box changed event for looping. 
            LoopingCheckBox.CheckedChanged += LoopingCheckBoxCheckedChanged;
            // Sbuscribe to check box changed event for shuffle
            ShuffleButton.CheckedChanged += ShuffleCheckBoxCheckedChanged;


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
                    // Stop the timer when audio playback stops
                    trackBarUpdateTimer.Stop();
                }
                // Initialize new resources  
                audioFileReader = new AudioFileReader(playlist[currentTrackIndex]);
                volumeStream = new WaveChannel32(audioFileReader); // Wrap AudioFileReader in WaveChannel32
                wavePlayer = new WaveOut();
                wavePlayer.Init(audioFileReader);
                // Subscribe to the playback stopped event
                //wavePlayer.PlaybackStopped += WavePlayerPlaybackStopped;
                if(isLooping)
                {
                    wavePlayer.PlaybackStopped += LoopCurrentTrack;
                    selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
                }
                else if (isShuffle && playlist.Count > 1)
                {
                    wavePlayer.PlaybackStopped += ShuffleNextTrack;
                    selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
                }
                wavePlayer.Play();

                // Initialize the default playback device
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                // Set the maximum value of the TrackBar to the total duration of the audio file
                trackBar.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;
                // Set the durationLabel text based on the total duration of the audio file
                TimeSpan totalDuration = audioFileReader.TotalTime;
                DurationLabel.Text = $"{totalDuration.Hours:D2}:{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
                // Start the timer when audio playback starts
                trackBarUpdateTimer.Start();
                
            }
        }// end of PlayCurrentTrack
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
            // Update the label to display the number of audio files in the playlist
            playlistCountLabel.Text = "Playlist Count: " + playlist.Count.ToString();
        }// end of UpdatePlaylistCountLabel
        private void UpdateNextPreviousButtons()
        {
            // Enable/disable next/previous buttons based on the current track index
            NextButton.Enabled = currentTrackIndex < playlist.Count - 1;
            PreviousButton.Enabled = currentTrackIndex > 0;
        }// end of UpdateNextPreviousButtons
        private void UpdatePlaylistComboBox(int currentIndex)
        {
            // Update the ComboBox with the playlist
            playlistComboBox.Items.Clear();
            // Extract only the file names
            var fileNames = playlist.Select(Path.GetFileNameWithoutExtension).ToArray();
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
        private void VolumeTrackBarScroll(object sender, EventArgs e)
        {
            // Calculate the volume from the trackbar value (0 to 100)
            float volume = VolumeTrackBar.Value / 100f;
            UpdateVolume(volume);

        }// end of VolumeTrackBarScroll
        private void PlaylistComboBoxSelectedIndexChanged(object sender, EventArgs e)
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
                // Update audio count label
                UpdatePlaylistCountLabel();
                
            }
            // Stop the current track if playing
            if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing)
            {
                wavePlayer.Stop();
            }
            // Play audio file
            PlayCurrentTrack();
        }// end of PlaylistComboBoxSelectedIndexChanged
        private void TrackBarUpdateTimerTick(object sender, EventArgs e)
        {
            if (audioFileReader != null)
            {
                // Update the TrackBar position based on the audio playback position
                int currentPosition = (int)(audioFileReader.CurrentTime.TotalSeconds);
                trackBar.Value = currentPosition;
                // Update the DurationLabel to count down
                TimeSpan remainingTime = audioFileReader.TotalTime - audioFileReader.CurrentTime;
                DurationLabel.Text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";

                if(remainingTime.TotalSeconds <= 0)
                {     
                    trackBarUpdateTimer.Stop();
                    wavePlayer.Stop();
                    
                }
            }
        }// end of TrackBarUpdateTimerTick
        private void ShuffleNextTrack(object sender, StoppedEventArgs e)
        {
            if (isShuffle)
            {
                // Choose a random track index different from the current one
                int nextIndex;
                do
                {
                    nextIndex = random.Next(0, playlist.Count);
                    //UpdatePlaylistComboBox(currentTrackIndex);
                } while (nextIndex == currentTrackIndex);

                currentTrackIndex = nextIndex;
            }
            else
            {
                // Move to the next track in a sequential order
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            }

            PlayCurrentTrack();
        }// end of ShuffleNextTrack
        private void LoopCurrentTrack(object sender, StoppedEventArgs e)
        {
            if (isLooping)
            {
                if (e.Exception != null) // If the playback stopped due to an exception, handle it here
                {
                    Console.WriteLine("Playback stopped with exception: " + e.Exception.Message);
                }
                PlayCurrentTrack();
            }
        }// end of LoopCUrrentTrack

        private void LoopingCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            
            isLooping = LoopingCheckBox.Checked;
            isShuffle = false;
            //LoopCurrentTrack();

        }// end of LoopingCheckBoxCheckedChanged
        private void ShuffleCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            isShuffle = ShuffleButton.Checked;
            isLooping = false;
           // ShuffleNextTrack();

        }// end of ShuffleCheckBoxCheckedChanged

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
                // playlist.AddRange(openFileDialog.FileNames);

                foreach (var filePath in openFileDialog.FileNames)
                {
                    // Check if the file is not already in the playlist before adding
                    if (!playlist.Contains(filePath))
                    {
                        // Add the selected file to the playlist
                        playlist.Add(filePath);
                    }
                }
                // Update the ComboBox with the playlist
                UpdatePlaylistComboBox(currentTrackIndex);
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
                // play audio file
                PlayCurrentTrack();
                
            }
        }// end of openToolStripMenuItemClick
        private void exitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }// end of exitToolStripMenuItemClick
        private void PlayButtonClick(object sender, EventArgs e)
        {
            wavePlayer.Play();
            if(wavePlayer.PlaybackState  == PlaybackState.Stopped)
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
                // Play audio file
                PlayCurrentTrack();
            }
        }// end of NextButtonClick
    }// end of partial call Form1
}// end of namespace
