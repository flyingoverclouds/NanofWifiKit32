# NanofWifiKit32
Different samples for Wifikit32 (ESP32 board) written using nanoframework.
Futur : integration Lorakit32 (ESP32 board with LORA network).

# Prerequisite & introduction

[Nanoframework](https://www.nanoframework.net/) is an open-source light .NET Framework (CLR + BCL) for embeddedMCU (such as ESP32, ...) and a very powerfull integration with Visual Studio ecosystem (extension for free [VS219 Community Edition] with full codebuild/debug/deploy features, nuget package integration, ...). 
The project joined the [.Net Foundation](https://dotnetfoundation.org/projects/nanoframework) in october 2020.

[WifiKit32](https://heltec.org/project/wifi-kit-32/) is a cheap ESP32 based developper board support Wifi & Bluetooth Low Emission (BLE). It integration a USB programmer port, LiPo battery management and onboard an OLED screen  (0.96' 128x64 pixel). 

----
# Projects

## Wifikit32Common (library)
A set common class, helper, framework for Wifikit32. 
This project is used in other samples.

## [BlinkyLed](src/BlinkyLed/BlinkyLed/) (exe)

// *The iot diy hello world !* //

This program blink the wifikit32 onboard led (hard wired to GPIO 25).

It the simplest test to check if the whole dev/compile/deploy chain works with a visual confirmation (the blinking led + debug counter increasing printed on debug output)


