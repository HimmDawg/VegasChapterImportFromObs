# What does it do?
This script takes a hybrid-mp4 file produced by Open Broadcaster Software (OBS) that has chapters added to it and imports those chapters into VEGAS Pro as markers.

# What problem does this solve?
VEGAS Pro needs support from third-party apps like _MediaInfo_ in order to import chapter markers from OBS and even then, the process was a little too involved just for importing chapters.
With this, you can import chapters directly from within VEGAS Pro.

# Setup
- install `ffprobe` and add it to your PATH (e.g. use winget like: https://winstall.app/apps/Gyan.FFmpeg , it'll automatically add the executable to PATH)
- download the `[Custom] Import-MP4Chapters.cs` script and the `[Custom] Import-MP4Chapters.cs.config`
    - you can rename the script, but make sure that the config file has the same name, followed by `.config`
- place both files in `C:\Program Files\VEGAS\VEGAS Pro x.0\Script Menu`
    - if not available, choose either of the locations listed here https://help.magix-hub.com/video/vegas/21/en/content/topics/external/vegasscriptfaq.html#1.10


# How to use
- select exactly one video file with chapters in the media explorer
- in VEGAS Pro, navigate to `Tools > Scripting`
    - if the script is not visible here, hit `Rescan Script Menu Folder`
    - else you can use it from here now

**The markers wont take the track position into consideration.**

# Remarks
This was tested with VEGAS Pro Post 21.0, no guarantee that this will work with other versions.
I am not sure if this would work with any hybrid mp4.
