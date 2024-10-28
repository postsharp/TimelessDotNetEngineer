using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TemperatureMonitor;

public class TemperatureViewModel : ReactiveObject
{
    private TemperatureSensor _sensor;

    // [<snippet Sensor>]
    public TemperatureSensor Sensor
    {
        get => this._sensor;
        set => this.RaiseAndSetIfChanged(ref this._sensor, value);
    }

    public bool IsSensorEnabled => this.Sensor.IsEnabled;
    // [<endsnippet Sensor>]

    // [<snippet ToggleTemperatureSensorCommandProperty>]
    public ReactiveCommand<Unit, Unit> ToggleTemperatureSensorCommand { get; }
    // [<endsnippet ToggleTemperatureSensorCommandProperty>]

    // [<snippet SetThresholdCommandProperty>]
    public ReactiveCommand<Double, Unit> SetThresholdCommand { get; }
    // [<endsnippet SetThresholdCommandProperty>]

    public ReactiveCommand<Unit, Unit> MeasureTemperatureCommand { get; }

    public TemperatureViewModel()
    {
        this.Sensor = new TemperatureSensor();

        Threshold = this.Sensor.Threshold;

        // [<snippet ToggleTemperatureSensorCommandCtor>]
        ToggleTemperatureSensorCommand = ReactiveCommand.Create(() => {
            Sensor.IsEnabled = !Sensor.IsEnabled;
        });
        this.Sensor.WhenAnyValue(s => s.IsEnabled)
               .Subscribe(_ => this.RaisePropertyChanged(nameof(IsSensorEnabled)));
        // [<endsnippet ToggleTemperatureSensorCommandCtor>]

        // [<snippet SetThresholdCommandCtor>]
        SetThresholdCommand = ReactiveCommand.Create((Double parameter) => {
            Sensor.Threshold = Convert.ToDouble(parameter);
        });
        this.Sensor.WhenAnyValue(s => s.Threshold)
        .Subscribe(_ => 
        {
            this.RaisePropertyChanged(nameof(TemperatureStatus));
            this.RaisePropertyChanged(nameof(CurrentTemperature));
        });
        // [<endsnippet SetThresholdCommandCtor>]

        MeasureTemperatureCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Sensor.Temperature = await Sensor.MeasureTemperature();
        });

        this.Sensor.WhenAnyValue(s => s.Temperature)
       .Subscribe(_ =>
       {
           this.RaisePropertyChanged(nameof(TemperatureStatus));
           this.RaisePropertyChanged(nameof(CurrentTemperature));
       });
    }

    private double _threshold;
    public double Threshold
    {
        get => _threshold;
        set => this.RaiseAndSetIfChanged(ref _threshold, value);
    }
    public double CurrentTemperature => this.Sensor.Temperature;

    public string TemperatureStatus
    {
        get
        {
            if (this.Sensor.Temperature > this.Sensor.Threshold)
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
