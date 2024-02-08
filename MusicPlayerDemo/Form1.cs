/*
 * ## TODO
 * Volume Fading:
 * Smoothly fade the volume in and out when starting or stopping a track to prevent abrupt changes in audio levels.
 *
 * ## WAIT LIST
 * 
 * ---
 * 
 */


using NAudio.Wave;
using System.Media;
using Timer = System.Windows.Forms.Timer;
using NAudio.CoreAudioApi;
using NAudio.Gui;
using TagLib;



namespace MusicPlayerDemo
{
    public partial class Form1 : Form
    {

        private Random random = new Random();
        private List<string> playlist;
        private int currentTrackIndex = 0;

        private IWavePlayer wavePlayer; // audio player
        private AudioFileReader audioFileReader;
        private WaveChannel32 volumeStream; // audio volume
        private Timer trackBarUpdateTimer; // audio time tracker
        private MMDevice defaultPlaybackDevice; // System volume  

        private bool isLooping = false; // loop feature
        private bool isShuffle = false; // shuffle feature
        private bool isSeeking = false; // Seeking feature

        
        public Form1()
        {
            InitializeComponent();
            wavePlayer = new WaveOutEvent(); // Initialize WaveOutEvent
            playlist = new List<string>();

            // Initialize the Timer for updating the TrackBar
            trackBarUpdateTimer = new Timer();
            trackBarUpdateTimer.Interval = 100; // Set the interval in milliseconds (adjust as needed)
            trackBarUpdateTimer.Tick += TrackBarUpdateTimerTick;

            FetchSystemVolumeLevel();
            
            VolumeTrackBar.Scroll += VolumeTrackBarScroll;// Subscribe to the Scroll event of the volume trackbar
            playlistComboBox.SelectedIndexChanged += PlaylistComboBoxSelectedIndexChanged;// Subscribe to Combobox event.
            LoopingCheckBox.CheckedChanged += LoopingCheckBoxCheckedChanged;// Subscribe to Checked box changed event for looping. 
            ShuffleButton.CheckedChanged += ShuffleCheckBoxCheckedChanged;// Sbuscribe to check box changed event for shuffle
            trackBar.MouseUp += SeekingTrackBarMouseUp; // subscribe to MouseUp event for audio trackBar seeking feature.
            trackBar.Scroll += SeekingTrackBarScroll; // subscribe to Scroll event for audio trackBar seeking feature.

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
                volumeStream = new WaveChannel32(audioFileReader); // Wrap AudioFileReader in WaveChannel32
                wavePlayer = new WaveOut();
                wavePlayer.Init(audioFileReader);

                /*
                 * An iff statement to check if the loop or shuffle button is checked
                 * If so subscribe to the method of the buttoned check and fetch the track infromation.
                 */
                if(isLooping)
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
                trackBar.Maximum = (int)audioFileReader.TotalTime.TotalSeconds; // Set the maximum value of the TrackBar to the total duration of the audio file
                TimeSpan totalDuration = audioFileReader.TotalTime;  // Set the durationLabel text based on the total duration of the audio file
                DurationLabel.Text = $"{totalDuration.Hours:D2}:{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
       
                trackBarUpdateTimer.Start();
            }
        }// end of PlayCurrentTrack

        private void FetchTrackInfo(string filePath)
        {   
            
            try
            {
                // load audio file
                TagLib.File file = TagLib.File.Create(filePath);   
                //extract the artist name from metadata
                string artist = file.Tag.FirstPerformer;
                string title = file.Tag.Title;
                // conditonal operation to check if the artist or title are null/empty.
                ArtistNameLabel.Text = string.IsNullOrEmpty(artist) ? "Unknown" : artist;
                selectedFileLabel.Text = string.IsNullOrEmpty(title) ? System.IO.Path.GetFileNameWithoutExtension(filePath) : title;    
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error extracting metadata for artist name" + ex.Message);
                ArtistNameLabel.Text = "Unknown";
                selectedFileLabel.Text = System.IO.Path.GetFileNameWithoutExtension(playlist[currentTrackIndex]);
            }

        }// end of FetchTrackInfo

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
                FetchTrackInfo(playlist[currentTrackIndex]);
                trackBar.Value = 0;
                UpdateNextPreviousButtons();
                UpdatePlaylistCountLabel();            
            }
            if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing) // Stop the current track if playing
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

                if(remainingTime.TotalSeconds <= 0)
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
                // Check if the audio file reader is playing or paused before seeking
                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    // Pause playback while seeking to avoid audio glitches
                    if (wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        wavePlayer.Pause();
                    }

                    // Seek to the desired position
                    TimeSpan newPosition = TimeSpan.FromSeconds(trackBar.Value);
                    audioFileReader.CurrentTime = newPosition;
                    
                    isSeeking = true; // set seeking flag to true
                }
            }
        }// end of SeekingTrackBarScroll

        private void SeekingTrackBarMouseUp(object sender, MouseEventArgs e)
        {

            // Resume playback if it was paused due to seeking
            if (isSeeking && wavePlayer.PlaybackState == PlaybackState.Paused)
            {
                wavePlayer.Play();
            }

            // Reset the seeking flag
            isSeeking = false;

        }
        private void ShuffleNextTrack(object sender, StoppedEventArgs e)
        {

            if (isShuffle)
            {
                // Choose a random track index different from the current one
                int nextIndex;
                do
                {
                    nextIndex = random.Next(0, playlist.Count);
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

        }// end of LoopingCheckBoxCheckedChanged

        private void ShuffleCheckBoxCheckedChanged(object sender, EventArgs e)
        {

            isShuffle = ShuffleButton.Checked;
            isLooping = false;

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
                    FetchTrackInfo(playlist[currentTrackIndex]);  
                }

                UpdatePlaylistCountLabel();
                UpdateNextPreviousButtons();
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

            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;

            if (playlist.Count > 0)
            {
                FetchTrackInfo(playlist[currentTrackIndex]);
                UpdatePlaylistCountLabel();
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
        }// end of NextButtonClick
    }// end of partial call Form1
}// end of namespace
