using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor
{
    public partial class TemperatureSensor : ObservableObject
    {
        public TemperatureSensor() {
            IsEnabled = false;
            Threshold = 25; // Default threshold
        }

        [ObservableProperty]
        private bool _isEnabled;

        [ObservableProperty]
        private bool _isMeasuring;

        [ObservableProperty]
        private double _temperature;

        [ObservableProperty]
        public double _threshold;

        public async Task<double> MeasureTemperature()
        {
            IsMeasuring = true;
            // Simulate measuring the temperature
            await Task.Delay(2000);
            IsMeasuring = false;

            Random random = new Random();
            return random.Next(10, 36);
        }
    }

}
