using NAudio.Wave;
using System.Media;

namespace MusicPlayerDemo
{
    public partial class Form1 : Form
    {
        private IWavePlayer wavePlayer;
        private AudioFileReader audioFileReader;
        private List<string> playlist;
        private int currentTrackIndex = 0;


        public Form1()
        {
            InitializeComponent();
            wavePlayer = new WaveOutEvent(); // Initialize WaveOutEvent
            playlist = new List<string>();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.wav;*.mp3;*.ogg;*.flac;*.aac|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Add the selected files to the playlist history
                playlist.AddRange(openFileDialog.FileNames.Select(Path.GetFileName));

                // Update the ComboBox with the playlist
                UpdatePlaylistComboBox();

                currentTrackIndex = playlist.Count - 1;

                // Set the label text to the selected file name
                if (playlist.Count > 0)
                {
                    selectedFileLabel.Text = System.IO.Path.GetFileName(playlist[currentTrackIndex]);
                }
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
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
                }

                // Initialize new resources
                audioFileReader = new AudioFileReader(playlist[currentTrackIndex]);
                wavePlayer = new WaveOutEvent();
                wavePlayer.Init(audioFileReader);
                wavePlayer.Play();
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
            playlistComboBox.Items.AddRange(playlist.ToArray());
            playlistComboBox.SelectedIndex = currentTrackIndex;
        }
        private void playlistComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle selection change in the ComboBox
            int selectedIndex = playlistComboBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < playlist.Count)
            {
                currentTrackIndex = selectedIndex;

                // Set the label text to the selected file name
                selectedFileLabel.Text = System.IO.Path.GetFileName(playlist[currentTrackIndex]);

                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                // Update audio label
                UpdatePlaylistCountLabel();
                // Play audio file
                PlayCurrentTrack();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            wavePlayer.Play();
        }
        private void PauseButton_Click(object sender, EventArgs e)
        {
            wavePlayer.Stop();
        }
        private void PreviousButton_Click(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;

            // Set the label text to the selected file name
            if (playlist.Count > 0)
            {
                selectedFileLabel.Text = System.IO.Path.GetFileName(playlist[currentTrackIndex]);
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
            
        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;

            // Set the label text to the selected file name
            if (playlist.Count > 0)
            {
                selectedFileLabel.Text = System.IO.Path.GetFileName(playlist[currentTrackIndex]);
                // Update the playlist count label
                UpdatePlaylistCountLabel();
                // Update the next/previous buttons
                UpdateNextPreviousButtons();
                PlayCurrentTrack();
            }
            
        }

    }
}
