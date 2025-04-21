# What does it do?
In a sentence: This script takes a video file produced by Open Broadcaster Software (OBS) that has chapters added to it and imports those chapters in VEGAS Pro.

# What problem does this solve?
VEGAS Pro always needed support from third party apps like MediaInfo in order to import chapters and even then, the process was a little too involved just for importing chapters. 
With this, you can import chapters directly from within VEGAS Pro.

# Setup
- install `ffprobe` and add it to your PATH (use winget like this: https://winstall.app/apps/Gyan.FFmpeg , it'll automatically add the executable to PATH)
- download the `[Custom] Import Chapters from OBS Hybrid MP4.cs` script and the `[Custom] Import Chapters from OBS Hybrid MP4.cs.config`
    - you can rename the script, but make sure that the config file has the same name, followed by `.config`
- place both files in either of the locations listed here https://help.magix-hub.com/video/vegas/21/en/content/topics/external/vegasscriptfaq.html#1.10
- in VEGAS Pro, navigate to `Tools > Scripting`
    - if the script is not visible here, hit `Rescan Script Menu Folder`
    - else you can use it from here now

# Remarks
This was tested with VEGAS Pro Post 21.0.
I am not sure if this would work with any hybrid mp4.