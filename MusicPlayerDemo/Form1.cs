using System.Media;

namespace MusicPlayerDemo
{
    public partial class Form1 : Form
    {
        private SoundPlayer soundPlayer = new SoundPlayer();
        private string[] playlist; // Array to store paths of music files
        private int currentTrackIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.wav;*.mp3|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                playlist = openFileDialog.FileNames;
                currentTrackIndex = 0;
                PlayCurrentTrack();
            }
        }
        private void PlayCurrentTrack()
        {
            if (playlist != null && playlist.Length > 0 && currentTrackIndex < playlist.Length)
            {
                soundPlayer.SoundLocation = playlist[currentTrackIndex];
                soundPlayer.Play();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Play();
        }
        private void PauseButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
        }
        private void PreviousButton_Click(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Length) % playlist.Length;
            PlayCurrentTrack();
        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
            PlayCurrentTrack();
        }
           
    }
}
