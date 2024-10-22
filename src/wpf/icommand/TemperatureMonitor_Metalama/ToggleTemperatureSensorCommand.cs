using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TemperatureMonitor
{
    class ToggleTemperatureSensorCommand : ICommand
    {
        private readonly TemperatureSensor _sensor;

        public ToggleTemperatureSensorCommand(TemperatureSensor sensor)
        {
            _sensor = sensor;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
           _sensor.IsEnabled = !_sensor.IsEnabled;
        }
    }
}
