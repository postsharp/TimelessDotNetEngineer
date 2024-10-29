// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;

namespace TemperatureMonitor;

public partial class TemperatureSensor : ObservableObject
{
    public TemperatureSensor()
    {
        this.IsEnabled = false;
        this.Threshold = 25; // Default threshold
    }

    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private bool _isMeasuring;

    [ObservableProperty]
    private double _temperature;

    [ObservableProperty]
    public double _threshold;

    public async Task<double> MeasureTemperature()
    {
        this.IsMeasuring = true;

        // Simulate measuring the temperature
        await Task.Delay( 2000 );
        this.IsMeasuring = false;

        var random = new Random();

        return random.Next( 10, 36 );
    }
}