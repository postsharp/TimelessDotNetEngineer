using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor
{
    public class TemperatureSensor : INotifyPropertyChanged
    {
        public TemperatureSensor() {
            IsEnabled = false;
            Threshold = 25; // Default threshold
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private bool _isMeasuring;
        public bool IsMeasuring
        {
            get { return _isMeasuring; }
            set
            {
                _isMeasuring = value;
                OnPropertyChanged(nameof(IsMeasuring));
            }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        public double _threshold;
        public double Threshold
        {
            get { return _threshold; }
            set
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
