# Msuic Player

>>>>CURRENT DEMO IS NOT UP TO DATE
## Desicription
 This is a simple music player application built using C# and .NET, leveraging the NAudio and TagLib libraries for audio playback and metadata retrieval. It allows users to play audio files, manage playlists, adjust volume, seek tracks, and toggle looping and shuffling functionalities.


## Tools
- NAudio API
   - Used as the main resource to play and manipulate audio files.
- TagLib 2.0 API
   - Used as a tool to extract audio file metadata such as, song name, artist name, and album cover. 
- C#
   - Main Language used to build the application

## Features
- Play Audio Files (wav, mp3, ogg, oga, or r16).
- Loop Audio or Shuffle.
- Fast-forward and rewind track time.
- Hop to next or previous track.
- Seek through the track audio.
- Display Album infromation such as, song name, artist name, album cover.
- Adjust Audio level as desired.
- Dark mode/Light mode.

## Instruction
- Download the project. 
    - Navigate to the saved location
       - 
       - Open Builds > Run MusicPlayer.application
          - If that does not work
              - Open the Solution file('MusicPlayerDemo.slm') in Visual Studio
              - Build the Solution to resotre NuGet packagrs and complie the application
    - Play Audio file
       - 
       - File > Open > Select desired file to play.
    - Save Playlist
       - 
       - Add as many audio files from step above.
       - Playlist > Save Playlist > Save in desire location.
    - Load Playlist
       - 
       - Playlist > Load Playlist > Pick the playlist you saved.
       - To navigate through the playlist > Expand the drop down mark with "Playlist:" > Click on desired song to play.
 - UI
      -
    - Menu Bar
         -
       - File Menu: Contain drop down to Select a track and Enabel dark and light mode.
       - Playlist Menu: Contain drop down to save playlist and load saved playlist.
    - Top\Middle Section
         -
       - Playlist Count: Display how many tracks are currently in the playlist.
       - Playlist with dropdown: Display current audio files in the playlist.
       - Empty Spot in the middle: Display album cover.
       - First Text Label: Song name.
       - Second Text Label: Artist name with smaller font.
    - Bottom Section
         -
       - Track Bar: Display the progress of the track time.
       - Duration timer: Display Track timer in HH:MM:SS format.
       - Preivous Button: Goes to the previous track on the playlist.
       - Next Button: Goes to the next track on the playlist.
       - -15 and +15 Buttons: Fast-forward and rewind track by -15 or +15 seconds.
       - Pause Button: Paused current track.
       - Play Button: Continue current track if paused.
       - Shuffle Button: If check play a random track from the playlist
       - Loop Button: If checked loop current track.
       - Sound Level Track bar: Adjust system audio level.


## Pending Features
- Integration with Online Services.
- Audio Visualization.
- Crossfade/Volume Fading.
- Mini Player Mode.
