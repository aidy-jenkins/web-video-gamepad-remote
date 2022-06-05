# web-video-gamepad-remote
Runs a container for video streaming websites on PC using a gamepad as a remote control

This allows you to run web apps like YouTube, BBC iPlayer etc. using an Xbox (or any other XInput) controller

Use it with your gaming PC or HTPC to avoid the need for a mouse and keyboard.
Also by loading the Smart TV version of these sites, this enables casting from your phone as a remote control.


Works by running a browser instance (Gecko) and mapping controller inputs to key presses


You can set configuration such as the URL to load, control stick dead zone etc. in the config.json
Alternatively, you can provide the following paramaters as command line args (which takes precedence over the config file)

|Name|Value|Description|
|--- | --- |---|
|-url|`<string>`|The URL to load in the player
|-useragent|`<string>`|The user agent to use when loading the page (e.g. that of a smart tv/stick)
|--fullscreen|`<none>`|If present, loads the app in full screen mode
