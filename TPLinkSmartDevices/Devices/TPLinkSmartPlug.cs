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
    }
}