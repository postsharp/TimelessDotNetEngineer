// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace TemperatureMonitor;

internal sealed class SetThresholdCommand : ICommand
{
    private readonly TemperatureSensor _sensor;

    public SetThresholdCommand( TemperatureSensor sensor )
    {
        this._sensor = sensor;

        // Subscribe to sensor property changes
        this._sensor.PropertyChanged += this.OnSensorPropertyChanged; 
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute( object? parameter )
    {
        return this._sensor.IsEnabled;
    }

    // [<snippet SetThresholdCommandExecute>] 
    public void Execute( object? parameter )
    {
        this._sensor.Threshold = Convert.ToDouble( parameter!, CultureInfo.CurrentCulture );
    }
    // [<endsnippet SetThresholdCommandExecute>]

    private void OnSensorPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName is null or nameof(TemperatureSensor.IsEnabled) )
        {
            // Notify WPF that CanExecute has changed
            this.CanExecuteChanged?.Invoke( this, EventArgs.Empty );
        }
    }
}