# TP-Link Smart Devices SDK
This library allows a developer to discovery and operate TP-Link Smart Devices in C#.

This includes TP-Link Smart Switches HS100/105/110 as well as TP-Link Smart Bulbs LB100/110/120/130.


## Usage
### Discovery
	// Runs in a Task<List<TPLinkSmartDevice>>
	var discoveredDevices = new TPLinkSmartDevices.TPLinkDiscovery().Discover().Result;


### Basic Usage
    var smartPlug = TPLinkSmartDevices.Devices.TPLinkSmartPlug("HS100.myhome.net");
    smartPlug.OutletPowered = true; // Turn on relay
    smartPlug.OutletPowered = false; // Turn off relay

    var smartBulb = TPLinkSmartDevices.Devices.TPLinkSmartBulb("LB100.myhome.net");
    smartBulb.PoweredOn = true; // Turn on bulb
    smartBulb.PoweredOff = false; // Turn off bulb


### Message Caching
Both the TPLinkSmartPlug and TPLinkSmartBulb classes have a MessageCache object, which allows the developer
to adjust the caching behavior of the library based on their needs. The options as packaged are NoMessageCache,
ManualMessageCache, and TimeGatedMessageCache.

* NoMessageCache passes all requests through to the device in real-time.

* ManualMessageCache caches everything and only flushes when the Flush() function is called.

* TimeGatedMessageCache (default) expires messages in cache after a given number of seconds.


## Disclaimer
This is an unofficial SDK that has no affiliation with TP-Link.

TP-Link and all respective product names are copyright TP-Link Technologies Co, Ltd. and/or its subsidiaries and affiliates.