using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
            dynamic sysInfo = Execute("system", "get_sysinfo");

            var Children = (JArray) sysInfo.children;
            dynamic Plug = Children[RelayID -1];

            Execute("system", "set_relay_state", "state", State ? 1 : 0, new List<string>()
            {
                Plug.id.ToString()
            });
        }

        /// <summary>
        /// Refresh device information
        /// </summary>
        public void Refresh()
        {
            dynamic sysInfo = Execute("system", "get_sysinfo");
            Features = ((string)sysInfo.feature).Split(':');
            LedOn = !(bool)sysInfo.led_off;

            Refresh(sysInfo);
        }
    }
}
