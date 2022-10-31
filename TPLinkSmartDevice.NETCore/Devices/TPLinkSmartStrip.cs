// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TPLinkSmartDevices.Model;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartStrip : TPLinkSmartDevice
    {
        public bool LedOn { get; private set; }

        /// <summary>
        /// DateTime the relay was powered on
        /// </summary>
        public DateTime PoweredOnSince { get; private set; }

        public string[] Features { get; private set; }

        public  TPLinkSmartStrip(string hostname, int port = 9999) : base(hostname, port)
        {
            Refresh();
        }

        public void ChangeRelayState(bool State, int RelayID)
        {
            Execute("system", "set_relay_state", "state", State ? 1 : 0, new List<string>()
            {
                Plugs[RelayID - 1].Id
            });
        }

        class PlugInfo
        {
            public string Id { get; set; }
            public string Alias { get; set; }
            public bool On { get; set; }
            public PlugInfo(dynamic rawJsonObject)
            {
                Id = rawJsonObject.id;
                Alias = rawJsonObject.alias;
                On = rawJsonObject.state == 1;
            }
        }

        PlugInfo[] Plugs = null;

        /// <summary>
        /// Read realtime power data of specified plug index 
        /// </summary>
        /// <param name="plugId">Plug index start from one</param>
        /// <returns>Power data</returns>
        public PowerData ReadRealtimePowerData(int plugId)
        {
            var pd = new PowerData(Execute("emeter", "get_realtime", childIdS: new List<string> { Plugs[plugId - 1].Id }));
            return pd;
        }

        public DailyPowerData[] ReadDailyStatistic(int plugId, int? year = null, int? month = null)
        {
            var res = Execute("emeter", "get_daystat", null, new JObject(
                new JProperty("month", month ?? DateTime.Today.Month),
                new JProperty("year", year ?? DateTime.Today.Year)
            ), childIdS: new List<string> { Plugs[plugId - 1].Id });
            return (res.day_list as JArray).Select(o => new DailyPowerData(o)).ToArray();
        }

        /// <summary>
        /// Refresh device information
        /// </summary>
        public void Refresh()
        {
            dynamic sysInfo = Execute("system", "get_sysinfo");
            Features = ((string)sysInfo.feature).Split(':');
            LedOn = !(bool)sysInfo.led_off;
            if (Plugs == null)
                Plugs = ((JArray)sysInfo.children).Select(o => new PlugInfo(o)).ToArray();
            Refresh(sysInfo);
        }
    }
}
