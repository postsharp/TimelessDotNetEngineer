// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace TemperatureMonitor;

public sealed class TemperatureSensor : INotifyPropertyChanged
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
        set
        {
            this._isEnabled = value;
            this.OnPropertyChanged( nameof(this.IsEnabled) );
        }
    }

    private bool _isMeasuring;

    public bool IsMeasuring
    {
        get => this._isMeasuring;
        set
        {
            this._isMeasuring = value;
            this.OnPropertyChanged( nameof(this.IsMeasuring) );
        }
    }

    private double _temperature;

    public double Temperature
    {
        get => this._temperature;
        set
        {
            if ( this._temperature != value )
            {
                this._temperature = value;
                this.OnPropertyChanged( nameof(this.Temperature) );
            }
        }
    }

    public double _threshold;

    public double Threshold
    {
        get => this._threshold;
        set
        {
            if ( this._threshold != value )
            {
                this._threshold = value;
                this.OnPropertyChanged( nameof(this.Threshold) );
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged( string propertyName ) => this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

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