using Metalama.Patterns.Observability;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TemperatureMonitor
{
    [Observable]
    public partial class TemperatureViewModel
    {
        public TemperatureSensor Sensor { get; set; }

        // [<snippet ToggleTemperatureSensorCommandProperty>]
        public ICommand ToggleTemperatureSensorCommand { get; }
        // [<endsnippet ToggleTemperatureSensorCommandProperty>]
        public ICommand SetThresholdCommand { get; }
        public ICommand MeasureTemperatureCommand { get; }

        public TemperatureViewModel()
        {
            this.Sensor = new TemperatureSensor();
            
            Threshold = this.Sensor.Threshold;

            // [<snippet ToggleTemperatureSensorCommandCtor>]
            ToggleTemperatureSensorCommand = new ToggleTemperatureSensorCommand(this.Sensor);
            // [<endsnippet ToggleTemperatureSensorCommandCtor>]
            SetThresholdCommand = new SetThresholdCommand(this.Sensor);
            MeasureTemperatureCommand = new MeasureTemperatureCommand(this.Sensor);
        }
        public bool IsSensorEnabled => this.Sensor.IsEnabled;

        public double Threshold { get; set; }

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
}
