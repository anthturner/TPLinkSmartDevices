// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using Newtonsoft.Json.Linq;
using System;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartPlug : TPLinkSmartDevice
    {
        private bool _powered;

        /// <summary>
        /// If the outlet relay is powered on (forces a refresh, to make this behavior consistent with Smart Bulbs)
        /// </summary>
        public bool OutletPowered
        {
            get
            {
                Refresh();
                return _powered;
            }
            set
            {
                Execute("system", "set_relay_state", "state", value ? 1 : 0);
                _powered = value;
            }
        }

        /// <summary>
        /// If the LED on the smart plug is on
        /// </summary>
        public bool LedOn { get; private set; }

        /// <summary>
        /// DateTime the relay was powered on
        /// </summary>
        public DateTime PoweredOnSince { get; private set; }

        public string[] Features { get; private set; }

        public TPLinkSmartPlug(string hostname) : base(hostname)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh device information
        /// </summary>
        public void Refresh()
        {
            dynamic sysInfo = Execute("system", "get_sysinfo");
            Features = ((string)sysInfo.feature).Split(':');
            _powered = (bool)sysInfo.relay_state;
            LedOn = !(bool)sysInfo.led_off;
            if ((int)sysInfo.on_time == 0)
                PoweredOnSince = default(DateTime);
            else
                PoweredOnSince = DateTime.Now - TimeSpan.FromSeconds((int)sysInfo.on_time);

            Refresh(sysInfo);
        }
		
        /// <summary>
        /// Configure the device so that it goes in the specified state at the delay expiration
        /// </summary>
        /// <param name="stateAtDelayExpiration"></param>
        /// <param name="delay"></param>
        /// <param name="name"></param>
        /// <returns>The rule ID (could eventually be useful for changing the existing rule...)</returns>
        public string SetCountDown(bool stateAtDelayExpiration, TimeSpan delay, string name = "")
        {
            // Clean-up the previous rule - if not done, the device responds with a "table is full" error
            Execute("count_down", "delete_all_rules", null, null);

            var retValue = Execute("count_down", "add_rule", null, new JObject
                {
                    new JProperty("enable", 1),
                    new JProperty("delay", Convert.ToInt32(delay.TotalSeconds)),
                    new JProperty("act", stateAtDelayExpiration? 1 : 0),
                    new JProperty("name", name)
                });

            return retValue["id"];
        }
    }
}