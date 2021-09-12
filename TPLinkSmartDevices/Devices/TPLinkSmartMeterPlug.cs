// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPLinkSmartDevices.Data;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartMeterPlug : TPLinkSmartPlug
    {
        public TPLinkSmartMeterPlug(string hostname) : base(hostname)
        {
        }

        public PowerData CurrentPowerUsage => new PowerData(Execute("emeter", "get_realtime"));

    }
}