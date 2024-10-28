using Metalama.Patterns.Observability;
using Metalama.Patterns.Wpf;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TemperatureMonitor;

[Observable]
public partial class TemperatureViewModel
{
    public TemperatureSensor Sensor { get; set; }

    // [<snippet ToggleTemperatureSensorCommand>] 
    [Command]
    public void ToggleTemperatureSensor()
    {
        this.Sensor.IsEnabled = !this.Sensor.IsEnabled;
    }
    // [<endsnippet ToggleTemperatureSensorCommand>]

    // [<snippet SetThresholdCommand>]
    [Command]
    public void SetThreshold(double threshold)
    {
        this.Sensor.Threshold = threshold;
    }
    // [<endsnippet SetThresholdCommand>]

    // [<snippet MeasureTemperatureCommand>]
    [Command]
    public void MeasureTemperature()
    {
        this.Sensor.Temperature = this.Sensor.MeasureTemperature();
    }

    public bool CanMeasureTemperature => this.Sensor.IsEnabled && !this.Sensor.IsMeasuring;
    // [<endsnippet MeasureTemperatureCommand>]

    public TemperatureViewModel()
    {
        this.Sensor = new TemperatureSensor();
        
        Threshold = this.Sensor.Threshold;
    }
    public bool IsSensorEnabled => this.Sensor.IsEnabled;

    public double Threshold { get; set; }

    public double CurrentTemperature => this.Sensor.Temperature;

    public string TemperatureStatus
    {
        get => this.Sensor.Temperature > this.Sensor.Threshold ? 
            "Temperature is above threshold!" : 
            "Temperature is below threshold.";
    }
}
