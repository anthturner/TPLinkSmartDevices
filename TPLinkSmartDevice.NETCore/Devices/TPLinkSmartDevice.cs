// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System;
using System.Collections.Generic;
using TPLinkSmartDevices.Messaging;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartDevice
    {
        const byte INITIALIZATION_VECTOR = 171;

        private string _alias;

        // TODO: EMeter-related commands

        public string Hostname { get; private set; }
        public int Port { get; private set; }

        public IMessageCache MessageCache { get; set; } = new TimeGatedMessageCache(2);

        public string SoftwareVersion { get; private set; }
        public string HardwareVersion { get; private set; }
        public string Type { get; private set; }
        public string Model { get; private set; }
        public string MacAddress { get; private set; }
        public string DevName { get; private set; }

        public string HardwareId { get; private set; }
        public string FirmwareId { get; private set; }
        public string DeviceId { get; private set; }
        public string OemId { get; private set; }

        public int RSSI { get; private set; }

        public int[] LocationLatLong { get; private set; }

        public string Alias
        {
            get { return _alias; }
            set
            {
                Execute("system", "set_dev_alias", "alias", value);
                _alias = value;
            }
        }

        public DateTime CurrentTime
        {
            get
            {
                dynamic rawTime = Execute("time", "get_time");
                return new DateTime((int)rawTime.year, (int)rawTime.month, (int)rawTime.mday, (int)rawTime.hour, (int)rawTime.min, (int)rawTime.sec);
            }
            set
            {
                // TODO: Implement this
            }
        }

        protected TPLinkSmartDevice(string hostname, int port=9999)
        {
            Hostname = hostname;
            Port = port;
        }

        public void Refresh(dynamic sysInfo = null)
        {
            if (sysInfo == null)
                sysInfo = Execute("system", "get_sysinfo");

            SoftwareVersion = sysInfo.sw_ver;
            HardwareVersion = sysInfo.hw_ver;
            Type = sysInfo.type;
            Model = sysInfo.model;
            MacAddress = sysInfo.mac;
            DevName = sysInfo.dev_name;
            _alias = sysInfo.alias;
            HardwareId = sysInfo.hwId;
            FirmwareId = sysInfo.fwId;
            DeviceId = sysInfo.deviceId;
            OemId = sysInfo.oemId;
            RSSI = sysInfo.rssi;

            if (sysInfo.latitude != null)
                LocationLatLong = new int[2] { sysInfo.latitude, sysInfo.longitude };
            else if (sysInfo.latitude_i != null)
                LocationLatLong = new int[2] { sysInfo.latitude_i, sysInfo.longitude_i };
        }

        protected dynamic Execute(string system, string command, string argument = null, object value = null, List<string> childIdS = null)
        {
            var message = new SmartHomeProtocolMessage(system, command, argument, value, childIdS);
            return MessageCache.Request(message, Hostname, Port);
        }
    }
}
