// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using ReactiveUI;
using System.Reactive;

namespace TemperatureMonitor;

public class TemperatureViewModel : ReactiveObject
{
    private TemperatureSensor _sensor;

    // [<snippet Sensor>]
    public TemperatureSensor Sensor
    {
        get => this._sensor;
        set => this.RaiseAndSetIfChanged( ref this._sensor, value );
    }

    public bool IsSensorEnabled => this.Sensor.IsEnabled;

    // [<endsnippet Sensor>]

    // [<snippet ToggleTemperatureSensorCommandProperty>]
    public ReactiveCommand<Unit, Unit> ToggleTemperatureSensorCommand { get; }

    // [<endsnippet ToggleTemperatureSensorCommandProperty>]

    // [<snippet SetThresholdCommandProperty>]
    public ReactiveCommand<double, Unit> SetThresholdCommand { get; }

    // [<endsnippet SetThresholdCommandProperty>]

    public ReactiveCommand<Unit, Unit> MeasureTemperatureCommand { get; }

    public TemperatureViewModel()
    {
        this.Sensor = new TemperatureSensor();

        this.Threshold = this.Sensor.Threshold;

        // [<snippet ToggleTemperatureSensorCommandCtor>]
        this.ToggleTemperatureSensorCommand = ReactiveCommand.Create(
            () =>
            {
                this.Sensor.IsEnabled = !this.Sensor.IsEnabled;
            } );

        this.Sensor.WhenAnyValue( s => s.IsEnabled )
            .Subscribe( _ => this.RaisePropertyChanged( nameof(this.IsSensorEnabled) ) );

        // [<endsnippet ToggleTemperatureSensorCommandCtor>]

        // [<snippet SetThresholdCommandCtor>]
        this.SetThresholdCommand = ReactiveCommand.Create(
            ( double parameter ) =>
            {
                this.Sensor.Threshold = Convert.ToDouble( parameter );
            } );

        this.Sensor.WhenAnyValue( s => s.Threshold )
            .Subscribe(
                _ =>
                {
                    this.RaisePropertyChanged( nameof(this.TemperatureStatus) );
                    this.RaisePropertyChanged( nameof(this.CurrentTemperature) );
                } );

        // [<endsnippet SetThresholdCommandCtor>]

        this.MeasureTemperatureCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                this.Sensor.Temperature = await this.Sensor.MeasureTemperature();
            } );

        this.Sensor.WhenAnyValue( s => s.Temperature )
            .Subscribe(
                _ =>
                {
                    this.RaisePropertyChanged( nameof(this.TemperatureStatus) );
                    this.RaisePropertyChanged( nameof(this.CurrentTemperature) );
                } );
    }

    private double _threshold;

    public double Threshold
    {
        get => this._threshold;
        set => this.RaiseAndSetIfChanged( ref this._threshold, value );
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
}