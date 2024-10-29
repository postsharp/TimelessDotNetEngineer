// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;
using System.Windows.Input;

namespace TemperatureMonitor;

internal sealed class MeasureTemperatureCommand : ICommand
{
    private readonly TemperatureSensor _sensor;

    public MeasureTemperatureCommand( TemperatureSensor sensor )
    {
        this._sensor = sensor;

        this._sensor.PropertyChanged +=
            this.OnSensorPropertyChanged; // Subscribe to sensor property changes
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute( object? parameter )
    {
        return this._sensor is { IsEnabled: true, IsMeasuring: false };
    }

    public async void Execute( object? parameter )
    {
        this._sensor.Temperature = await this._sensor.MeasureTemperature();
    }

    private void OnSensorPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName is null or nameof(TemperatureSensor.IsEnabled)
            or nameof(TemperatureSensor.IsMeasuring) )
        {
            // Notify WPF that CanExecute has changed
            this.CanExecuteChanged?.Invoke( this, EventArgs.Empty );
        }
    }
}