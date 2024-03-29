# TP-Link Smart Devices SDK
This library allows a developer to discover and operate TP-Link Smart Devices with C# applications.

This includes support for TP-Link Smart Switches HS100/105/110 as well as TP-Link Smart Bulbs LB100/110/120/130.

## NuGet Packages
A NuGet package for this version is available at https://www.nuget.org/packages/TPLinkSmartDevice.NETCore/

**NOTICE:** Unfortunately, a couple of other users have forked this work and published their own NuGet packages without attribution. However, instead of publishing yet another .NET package, I have opted to just re-point the project to to `netstandard` and release a package for that target instead. If there is a need to create another .NET package for it in the future, I will be happy to do so. If users have contributions, I encourage them to open pull requests here.

## Usage
### Discovery
	// Runs in a Task<List<TPLinkSmartDevice>>
	var discoveredDevices = new TPLinkSmartDevices.TPLinkDiscovery().Discover().Result;


### Basic Usage
    var smartPlug = new TPLinkSmartDevices.Devices.TPLinkSmartPlug("HS100.myhome.net");
    smartPlug.OutletPowered = true; // Turn on relay
    smartPlug.OutletPowered = false; // Turn off relay

    var smartBulb = new TPLinkSmartDevices.Devices.TPLinkSmartBulb("LB100.myhome.net");
    smartBulb.PoweredOn = true; // Turn on bulb
    smartBulb.PoweredOn = false; // Turn off bulb


### Message Caching
Both the TPLinkSmartPlug and TPLinkSmartBulb classes have a MessageCache object, which allows the developer
to adjust the caching behavior of the library based on their needs. The options as packaged are NoMessageCache,
ManualMessageCache, and TimeGatedMessageCache.

* NoMessageCache passes all requests through to the device in real-time.

* ManualMessageCache caches everything and only updates the cache after the Flush() function is called.

* TimeGatedMessageCache (default) expires messages in cache after a given number of seconds from their original cache time.


## Disclaimer
This is an unofficial SDK that has no affiliation with TP-Link.

TP-Link and all respective product names are copyright TP-Link Technologies Co, Ltd. and/or its subsidiaries and affiliates.
