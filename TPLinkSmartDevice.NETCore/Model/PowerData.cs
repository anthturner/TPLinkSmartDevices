// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkSmartDevices.Model
{
    /// <summary>
    /// Encapsulates JSON data structure for current energy use as metered by the HS110 Smart Energy Meter.
    /// </summary>
    public class PowerData
    {
        private readonly dynamic _powerData;

        public PowerData(dynamic powerData)
        {
            _powerData = powerData;
        }

        /// <summary>
        /// Current measured voltage in volts.
        /// </summary>
        public double Voltage => _powerData.voltage == null ? _powerData.voltage_mv / 1000.0d : _powerData.voltage;

        /// <summary>
        /// Current measured amperage in amps.
        /// </summary>
        public double Amperage => _powerData.current == null ? _powerData.current_ma / 1000.0d : _powerData.current;

        /// <summary>
        /// Current measured power usage in watts.
        /// </summary>
        public double Power => _powerData.power == null ? _powerData.power_mw / 1000.0d : _powerData.power;

        /// <summary>
        /// Current total power consumption in kilowatthours.
        /// </summary>
        public double Total => _powerData.total == null ? _powerData.total_wh / 1000.0d : _powerData.total;

        public int ErrorCode => _powerData.err_code;

        public override string ToString()
        {
            return $"{Voltage:0.0} Volt, {Amperage:0.00} Ampere, {Power:0.00} Watt";
        }
    }
}
