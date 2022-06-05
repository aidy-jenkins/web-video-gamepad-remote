# web-video-gamepad-remote
Runs a container for video streaming websites on PC using a gamepad as a remote control

This allows you to run web apps like YouTube, BBC iPlayer etc. using an Xbox (or any other XInput) controller

Use it with your gaming PC or HTPC to avoid the need for a mouse and keyboard.
Also by loading the Smart TV version of these sites, this enables casting from your phone as a remote control.


Works by running a browser instance (Gecko) and mapping controller inputs to key presses

# How to use

Download a release and run the file `WebVideoGamepad.NET.exe`

On an Xbox controller (other controllers should work but the mapping may vary), use the 
control stick or d-pad to navigate, A to select and B to go back
|Control|Action|
|--- | --- |
|Control stick|Navigate
|D-pad|Navigate
|A|Select
|B|Return
|Y|Exit (this is likely to change in future)

Alternatively, you may still use a mouse/keyboard or your phone to cast (if the website supports it)

# Configuration

You can set configuration such as the URL to load, control stick dead zone etc. in the config.json
Alternatively, you can provide the following parameters as command line args (which takes precedence over the config file)

|Name|Value|Description|
|--- | --- |---|
|-url|`<string>`|The URL to load in the player
|-useragent|`<string>`|The user agent to use when loading the page (e.g. that of a smart tv/stick)
|--fullscreen|`<none>`|If present, loads the app in full screen mode
