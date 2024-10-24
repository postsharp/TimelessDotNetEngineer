using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TemperatureMonitor
{
    public partial class TemperatureViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MeasureTemperatureCommand))]
        private TemperatureSensor _sensor;

        partial void OnSensorChanging(TemperatureSensor? sensor)
        {
            this.UnsubscribeFromSensor();
        }

        partial void OnSensorChanged(TemperatureSensor sensor)
        {
            this.SubscribeToSensor(sensor);
        }

        // [<snippet ToggleTemperatureSensorCommand>] 
        [RelayCommand]
        public void ToggleTemperatureSensor()
        {
            this.Sensor.IsEnabled = !this.Sensor.IsEnabled;
        }
        // [<endsnippet ToggleTemperatureSensorCommand>]

        [RelayCommand]
        public void SetThreshold(double threshold)
        {
            this.Sensor.Threshold = threshold;
        }

        // [<snippet MeasureTemperatureCommand>]
        [RelayCommand(CanExecute = nameof(CanMeasureTemperature))]
        public async Task MeasureTemperature()
        {
            this.Sensor.Temperature = await this.Sensor.MeasureTemperature();
        }

        private bool CanMeasureTemperature => this.Sensor.IsEnabled && !this.Sensor.IsMeasuring;
        // [<endsnippet MeasureTemperatureCommand>]

        public TemperatureViewModel()
        {
            this.Sensor = new TemperatureSensor();
            
            Threshold = this.Sensor.Threshold;
        }
        public bool IsSensorEnabled => this.Sensor.IsEnabled;

        [ObservableProperty]
        private double _threshold;

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
                    this.MeasureTemperatureCommand.NotifyCanExecuteChanged();
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
