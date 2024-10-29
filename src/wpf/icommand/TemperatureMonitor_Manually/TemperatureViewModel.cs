// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;
using System.Windows.Input;

namespace TemperatureMonitor;

public sealed class TemperatureViewModel : INotifyPropertyChanged
{
    private TemperatureSensor _sensor;

    // [<snippet Sensor>]
    public TemperatureSensor Sensor
    {
        get => this._sensor;

        set
        {
            if ( !ReferenceEquals( value, this._sensor ) )
            {
                this.UnsubscribeFromSensor();
                this._sensor = value;
                this.OnPropertyChanged( nameof(this.Sensor) );
                this.SubscribeToSensor();
            }
        }
    }

    public bool IsSensorEnabled => this.Sensor.IsEnabled;

    // [<endsnippet Sensor>]

    // [<snippet ToggleTemperatureSensorCommandProperty>]
    public ICommand ToggleTemperatureSensorCommand { get; }

    // [<endsnippet ToggleTemperatureSensorCommandProperty>]
    public ICommand SetThresholdCommand { get; }

    public ICommand MeasureTemperatureCommand { get; }

    public TemperatureViewModel()
    {
        this.Sensor = new TemperatureSensor();

        this.Threshold = this.Sensor.Threshold;

        // [<snippet ToggleTemperatureSensorCommandCtor>]
        this.ToggleTemperatureSensorCommand = new ToggleTemperatureSensorCommand( this.Sensor );

        // [<endsnippet ToggleTemperatureSensorCommandCtor>]
        this.SetThresholdCommand = new SetThresholdCommand( this.Sensor );
        this.MeasureTemperatureCommand = new MeasureTemperatureCommand( this.Sensor );
    }

    private double _threshold;

    public double Threshold
    {
        get => this._threshold;
        set
        {
            this._threshold = value;
            this.OnPropertyChanged( nameof(this.Threshold) );
        }
    }

    public double CurrentTemperature => this.Sensor.Temperature;

    public string TemperatureStatus
    {
        get
        {
            if ( this.Sensor.Temperature > this.Sensor.Threshold )
            {
                return "Temperature is above threshold!";
            }
            else
            {
                return "Temperature is below threshold.";
            }
        }
    }

    private void OnPropertyChanged( string propertyName )
    {
        this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // [<snippet SubscribeToSensorChanges>]
    private void SubscribeToSensor()
    {
        if ( this._sensor != null )
        {
            this._sensor.PropertyChanged += this.HandleSensorPropertyChanged;
        }
    }

    private void HandleSensorPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        {
            var propertyName = e.PropertyName;

            if ( propertyName is nameof(this.Sensor.IsEnabled) )
            {
                this.OnPropertyChanged( nameof(this.IsSensorEnabled) );
            }

            if ( propertyName is nameof(this.Sensor.Temperature) or nameof(this.Sensor.Threshold) )
            {
                this.OnPropertyChanged( nameof(this.CurrentTemperature) );
                this.OnPropertyChanged( nameof(this.TemperatureStatus) );
            }
        }
    }

    // [<endsnippet SubscribeToSensorChanges>]

    private void UnsubscribeFromSensor()
    {
        if ( this._sensor != null! )
        {
            this._sensor.PropertyChanged -= this.HandleSensorPropertyChanged;
        }
    }
}