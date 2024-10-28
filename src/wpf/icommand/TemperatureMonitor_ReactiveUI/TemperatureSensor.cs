using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor;

public class TemperatureSensor : ReactiveObject
{
    public TemperatureSensor() {
        IsEnabled = false;
        Threshold = 25; // Default threshold
    }

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    private bool _isMeasuring;
    public bool IsMeasuring
    {
        get => _isMeasuring;
        set => this.RaiseAndSetIfChanged(ref _isMeasuring, value);
    }

    private double _temperature;
    public double Temperature
    {
        get => _temperature;
        set => this.RaiseAndSetIfChanged(ref _temperature, value);
    }

    public double _threshold;
    public double Threshold
    {
        get => _threshold;
        set => this.RaiseAndSetIfChanged(ref _threshold, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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
