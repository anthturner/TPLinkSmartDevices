// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System;
using TPLinkSmartDevices.Model;

namespace TPLinkSmartDevices.Devices
{
    public class TPLinkSmartLightStrip : TPLinkSmartDevice
    {
        private bool _poweredOn;
        private LightHSV _hsv;
        private int _colorTemp;
        private int _brightness;

        public bool IsColor { get; private set; }
        public bool IsDimmable { get; private set; }
        public bool IsVariableColorTemperature { get; private set; }
        public bool HasEffects { get; private set; }

        /// <summary>
        /// If the light strip is powered on or not
        /// </summary>
        public bool PoweredOn
        {
            get { return _poweredOn; }
            set
            {
                Execute("smartlife.iot.lightStrip", "set_light_state", "on_off", value ? 1 : 0);
                _poweredOn = value;
            }
        }

        /// <summary>
        /// Bulb color
        /// </summary>
        public LightHSV HSV
        {
            get
            {
                if (!IsColor)
                    throw new NotSupportedException("Light strip does not support color changes.");
                return _hsv;
            }
            set
            {
                if (!IsColor)
                    throw new NotSupportedException("Light strip does not support color changes.");

                Execute("smartlife.iot.lightStrip", "set_light_state", "hue", value.Hue);
                Execute("smartlife.iot.lightStrip", "set_light_state", "saturation", value.Saturation);
                Execute("smartlife.iot.lightStrip", "set_light_state", "brightness", value.Value * 100 / 255);
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
                    throw new NotSupportedException("Light strip does not support color temperature changes.");
                return _colorTemp;
            }
            set
            {
                if (!IsVariableColorTemperature)
                    throw new NotSupportedException("Light strip does not support color temperature changes.");

                Execute("smartlife.iot.lightStrip", "set_light_state", "color_temp", value);
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
                    throw new NotSupportedException("Light strip does not support dimming.");
                return _brightness;
            }
            set
            {
                if (!IsDimmable)
                    throw new NotSupportedException("Light strip does not support dimming.");

                Execute("smartlife.iot.lightStrip", "set_light_state", "brightness", value * 100 / 255);
                _brightness = value;
            }
        }

        public TPLinkSmartLightStrip(string hostName) : base(hostName)
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
            HasEffects = (sysInfo.lighting_effect_state != null);

            dynamic lightState = sysInfo.light_state;
            _poweredOn = lightState.on_off;

            if (!_poweredOn)
                lightState = lightState.dft_on_state;
            
            _hsv = new LightHSV() { Hue = lightState.hue, Saturation = lightState.saturation, Value = lightState.brightness };
            _colorTemp = lightState.color_temp;
            _brightness = lightState.brightness;
            
            Refresh(sysInfo);
        }

        public void SetLightingEffect(LightingEffect effect)
        {
            if (!HasEffects)
                throw new NotSupportedException("Light strip does not support effects.");

            Execute("smartlife.iot.lighting_effect", "set_lighting_effect", value: effect);
        }
    }

    public class LightHSV : IEquatable<LightHSV>
    {
        public byte Hue { get; set; }
        public byte Saturation { get; set; }
        public byte Value { get; set; }

        public bool Equals(LightHSV other)
        {
            return this.Hue == other.Hue &&
                   this.Saturation == other.Saturation;
        }
    }
}
