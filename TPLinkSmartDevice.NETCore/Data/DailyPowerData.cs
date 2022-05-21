using System;
using System.Collections.Generic;
using System.Text;

namespace TPLinkSmartDevices.Data
{
    /// <summary>
    /// Daily power statistics
    /// </summary>
    public class DailyPowerData
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Power (Wh)
        /// </summary>
        public int WattHours { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rawData">JSON raw object</param>
        public DailyPowerData(dynamic rawData)
        {
            Date = new DateTime((int)rawData.year, (int)rawData.month, (int)rawData.day);
            WattHours = rawData.energy_wh;
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} {WattHours} Wh";
        }
    }
}
