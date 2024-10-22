using Metalama.Patterns.Observability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor
{
    [Observable]
    public partial class TemperatureSensor
    {
        public bool IsEnabled { get; set; } = false;

        public bool IsMeasuring { get; set; }

        public double Temperature { get; set; }

        public double Threshold { get; set; } = 25; // Default threshold

        public async Task<double> MeasureTemperature()
        {
            IsMeasuring = true;
            // Simulate measuring the temperature
            await Task.Delay(2000);
            IsMeasuring = false;

            Random random = new Random();
            return random.Next(10, 36);
        }
    }

}
