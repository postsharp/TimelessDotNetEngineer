using System.ComponentModel;

namespace RectangleArea
{
    partial class Polygon : INotifyPropertyChanged
    {
        private double _scaleFactor;

        // This attribute represents a multiplier for dimensions
        public double ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }

            set
            {
                if (_scaleFactor != value)
                {
                    _scaleFactor = value;
                    OnPropertyChanged(nameof(ScaleFactor));
                }
            }
        }

        public Polygon()
        {
            this.ScaleFactor = 1;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
