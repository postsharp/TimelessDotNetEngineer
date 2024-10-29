// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Observability;

namespace TemperatureMonitor;

[Observable]
public partial class TemperatureSensor
{
    public bool IsEnabled { get; set; }

    public bool IsMeasuring { get; set; }

    public double Temperature { get; set; }

    public double Threshold { get; set; } = 25; // Default threshold

    public double MeasureTemperature()
    {
        this.IsMeasuring = true;

        // Simulate measuring the temperature
        Thread.Sleep( 2000 );

        var random = new Random();
        this.IsMeasuring = false;

        return random.Next( 10, 36 );
    }
}