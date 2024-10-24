using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TemperatureMonitor;

class MeasureTemperatureCommand : ICommand
{
    private readonly TemperatureSensor _sensor;

    public MeasureTemperatureCommand(TemperatureSensor sensor)
    {
        _sensor = sensor;
        _sensor.PropertyChanged += OnSensorPropertyChanged; // Subscribe to sensor property changes
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _sensor.IsEnabled && !_sensor.IsMeasuring;
    }

    public async void Execute(object? parameter)
    {
        _sensor.Temperature = await _sensor.MeasureTemperature();
    }

    private void OnSensorPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is null or nameof(TemperatureSensor.IsEnabled) or nameof(TemperatureSensor.IsMeasuring))
        {
            // Notify WPF that CanExecute has changed
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
