using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TemperatureMonitor
{
    public class TemperatureViewModel : INotifyPropertyChanged
    {
        private TemperatureSensor _sensor;

        public TemperatureSensor Sensor
        {
            get => this._sensor;

            set
            {
                if (!ReferenceEquals(value, this._sensor))
                {
                    this.UnsubscribeFromSensor();
                    this._sensor = value;
                    this.OnPropertyChanged(nameof(this.Sensor));
                    this.SubscribeToSensor(this.Sensor);
                }
            }
        }

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

        private double _threshold;
        public double Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                OnPropertyChanged(nameof(Threshold));
            }
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SubscribeToSensor(TemperatureSensor value)
        {
            if (value != null)
            {
                value.PropertyChanged += this.HandleSensorPropertyChanged;
            }
        }

        private void HandleSensorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            {
                var propertyName = e.PropertyName;

                if (propertyName is nameof(this.Sensor.IsEnabled))
                {
                    this.OnPropertyChanged(nameof(this.IsSensorEnabled));
                }

                if (propertyName is nameof(this.Sensor.Temperature) or nameof(this.Sensor.Threshold))
                {
                    this.OnPropertyChanged(nameof(this.CurrentTemperature));
                    this.OnPropertyChanged(nameof(this.TemperatureStatus));
                }
            }
        }

        private void UnsubscribeFromSensor()
        {
            if (this._sensor != null!)
            {
                this._sensor.PropertyChanged -= this.HandleSensorPropertyChanged;
            }
        }
    }
}
