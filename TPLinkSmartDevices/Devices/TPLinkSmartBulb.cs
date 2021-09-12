// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using Newtonsoft.Json.Linq;
using System;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartBulb : TPLinkSmartDevice
    {
        private bool _poweredOn;
        private BulbHSV _hsv;
        private int _colorTemp;
        private int _brightness;

        public bool IsColor { get; private set; }
        public bool IsDimmable { get; private set; }
        public bool IsVariableColorTemperature { get; private set; }

        /// <summary>
        /// If the bulb is powered on or not
        /// </summary>
        public bool PoweredOn
        {
            get { return _poweredOn; }
            set
            {
                Execute("smartlife.iot.smartbulb.lightingservice", "transition_light_state", "on_off", value ? 1 : 0);
                _poweredOn = value;
            }
        }

        /// <summary>
        /// Bulb color
        /// </summary>
        public BulbHSV HSV
        {
            get
            {
                if (!IsColor)
                    throw new NotSupportedException("Bulb does not support color changes.");
                return _hsv;
            }
            set
            {
                if (!IsColor)
                    throw new NotSupportedException("Bulb does not support color changes.");

                Execute("smartlife.iot.smartbulb.lightingservice", "transition_light_state", "light_state", new JObject
                {
                    new JProperty("hue", value.Hue),
                    new JProperty("saturation", value.Saturation),
                    new JProperty("brightness", (int)(value.Value * 100 / 255)),
                    new JProperty("color_temp", 0)
                });
                _hsv = value;
            }
        }

        /// <summary>
        /// Color temperature in Kelvin
        /// </summary>
        public int ColorTemperature
        {
            get
            {
                if (!IsVariableColorTemperature)
                    throw new NotSupportedException("Bulb does not support color temperature changes.");
                return _colorTemp;
            }
            set
            {
                if (!IsVariableColorTemperature)
                    throw new NotSupportedException("Bulb does not support color temperature changes.");

                Execute("smartlife.iot.smartbulb.lightingservice", "transition_light_state", "color_temp", value);
                _brightness = value;
            }
        }

        /// <summary>
        /// Bulb brightness in percent
        /// </summary>
        public int Brightness
        {
            get
            {
                if (!IsDimmable)
                    throw new NotSupportedException("Bulb does not support dimming.");
                return _brightness;
            }
            set
            {
                if (!IsDimmable)
                    throw new NotSupportedException("Bulb does not support dimming.");

                Execute("smartlife.iot.smartbulb.lightingservice", "transition_light_state", "brightness", value);
                _brightness = value;
            }
        }

        public TPLinkSmartBulb(string hostName) : base(hostName)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh device information
        /// </summary>
        public void Refresh()
        {
            dynamic sysInfo = Execute("system", "get_sysinfo");
            IsColor = (bool)sysInfo.is_color;
            IsDimmable = (bool)sysInfo.is_dimmable;
            IsVariableColorTemperature = (bool)sysInfo.is_variable_color_temp;

            dynamic lightState = Execute("smartlife.iot.smartbulb.lightingservice", "get_light_state");
            _poweredOn = lightState.on_off;

            if (!_poweredOn)
                lightState = lightState.dft_on_state;
            
            _hsv = new BulbHSV() { Hue = lightState.hue, Saturation = lightState.saturation, Value = lightState.brightness * 255 / 100 };
            _colorTemp = lightState.color_temp;
            _brightness = lightState.brightness;
            
            Refresh(sysInfo);
        }

        public class BulbHSV
        {
            public byte Hue { get; set; }
            public byte Saturation { get; set; }
            public byte Value { get; set; }
        }
    }
}
