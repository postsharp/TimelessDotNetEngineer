// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using ReactiveUI;
using System.ComponentModel;

namespace TemperatureMonitor;

public class TemperatureSensor : ReactiveObject
{
    public TemperatureSensor()
    {
        this.IsEnabled = false;
        this.Threshold = 25; // Default threshold
    }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get => this._isEnabled;
        set => this.RaiseAndSetIfChanged( ref this._isEnabled, value );
    }

    private bool _isMeasuring;

    public bool IsMeasuring
    {
        get => this._isMeasuring;
        set => this.RaiseAndSetIfChanged( ref this._isMeasuring, value );
    }

    private double _temperature;

    public double Temperature
    {
        get => this._temperature;
        set => this.RaiseAndSetIfChanged( ref this._temperature, value );
    }

    private double _threshold;

    public double Threshold
    {
        get => this._threshold;
        set => this.RaiseAndSetIfChanged( ref this._threshold, value );
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged( string propertyName )
    {
        this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

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