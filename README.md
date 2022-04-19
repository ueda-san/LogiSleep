# LogiSleep

(This file was machine translated)

If for some reason the keyboard backlight does not turn off when the display is turned off, this software will force the keyboard backlight to turn off.
As an added bonus, there is also a display turn-off prevention function/immediate turn-off function.

- Logitech Gaming Software or GHUB is required.
- Cannot be used at the same time as other LED control applications such as games.


## How to use

To use the x64 version, download it from the Release page.
If you want to run it at Windows startup, register the shortcut to Startup. (If you want to put it in the taskbar from the beginning, set the "Run" in the properties of the shortcut to "minimized")

![LogiSleep3_en](https://user-images.githubusercontent.com/70194698/163715194-a8f9a1b2-fb3a-4945-a947-6f12595d0d14.png)


- Check "Turns off keyboard LED when screen is off" to turn off the keyboard backlight when the screen is turned off.
- Check "Prevent the screen from turning off" to prevent the display from turning off by periodically generating a mouse movement event.
- Pressing the "Turn off now" button immediately turns off the screen.

<img width="267" alt="LogiSleep1_en" src="https://user-images.githubusercontent.com/70194698/163715203-4deccb0e-d2e3-468d-8698-66870c5c8c31.png">


Press the minimize button to enter the task tray.
Double-click the task tray icon to display the window.

<img width="142" alt="LogiSleep2_en" src="https://user-images.githubusercontent.com/70194698/163715212-6568cbd0-bf9b-40a2-b84f-29f7208256c6.png">


## How to build

If you want to use the x86 version or build it yourself, build the source code and copy LogitechLedEnginesWrapper.dll from the SDK to the same directory as the executable.


## Check Status

Windows10 (21H2) / LED_SDK 9.00

|        | LGS 9.04.28 | GHUB 2022.4.250563    |
|--------|-------------|-----------------------|
| G610   | ok          | ok                    |
| G710+  | ok(*1)      | Keyboard not detected |
| G15 v2 | NG          | Keyboard not detected |

*1 WASD zone lighting not restored.

## License

MIT license

Licensing of Logitech's LED Illumination SDK is in accordance with the documentation that comes with the SDK.

https://www.logitechg.com/en-us/innovation/developer-lab.html
