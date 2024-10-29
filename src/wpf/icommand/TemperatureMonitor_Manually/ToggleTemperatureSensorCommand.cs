// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Windows.Input;

namespace TemperatureMonitor;

internal sealed class ToggleTemperatureSensorCommand : ICommand
{
    private readonly TemperatureSensor _sensor;

    public ToggleTemperatureSensorCommand( TemperatureSensor sensor )
    {
        this._sensor = sensor;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute( object? parameter )
    {
        return true;
    }

    public void Execute( object? parameter )
    {
        this._sensor.IsEnabled = !this._sensor.IsEnabled;
    }
}