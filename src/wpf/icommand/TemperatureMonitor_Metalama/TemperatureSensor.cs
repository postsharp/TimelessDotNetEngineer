using Metalama.Patterns.Observability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor;

[Observable]
public partial class TemperatureSensor
{
    public bool IsEnabled { get; set; } = false;

    public bool IsMeasuring { get; set; }

    public double Temperature { get; set; }

    public double Threshold { get; set; } = 25; // Default threshold

    public double MeasureTemperature()
    {
        IsMeasuring = true;
        // Simulate measuring the temperature
        Thread.Sleep(2000);

        Random random = new Random();
        IsMeasuring = false;
        return random.Next(10, 36);
    }
}
