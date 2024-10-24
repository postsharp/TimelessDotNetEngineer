using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TemperatureMonitor
{
    class SetThresholdCommand : ICommand
    {
        private readonly TemperatureSensor _sensor;

        public SetThresholdCommand(TemperatureSensor sensor)
        {
            _sensor = sensor;
            _sensor.PropertyChanged += OnSensorPropertyChanged; // Subscribe to sensor property changes
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _sensor.IsEnabled;
        }

        public void Execute(object? parameter)
        {
            _sensor.Threshold = Convert.ToDouble(parameter);
        }

        private void OnSensorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is null or nameof(TemperatureSensor.IsEnabled))
            {
                // Notify WPF that CanExecute has changed
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
