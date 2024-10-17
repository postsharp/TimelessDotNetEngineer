using System.ComponentModel;

namespace RectangleArea
{
    partial class RectangleCalcViewModel : INotifyPropertyChanged
    {
        // [<snippet RectangleProp>]
        private Rectangle _rectangle;
        public Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
            set
            {
                if (!object.ReferenceEquals(value, _rectangle))
                {
                    _rectangle = value;
                    this.OnPropertyChanged(nameof(Rectangle));
                }

            }
        }
        // [<endsnippet RectangleProp>]

        // [<snippet AreaChildProperty>]
        // [<snippet AreaProp>]
        public double Area => this.Rectangle.Area;
        // [<endsnippet AreaProp>]

        public RectangleCalcViewModel()
        {
            _rectangle = new Rectangle(10, 5);
            SubscribeToRectangle(Rectangle);
        }

        private PropertyChangedEventHandler? _handleRectanglePropertyChanged;

        private void SubscribeToRectangle(Rectangle value)
        {
            if (value != null)
            {
                _handleRectanglePropertyChanged ??= HandlePropertyChanged;
                value.PropertyChanged += _handleRectanglePropertyChanged;
            }

            void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                {
                    var propertyName = e.PropertyName;
                    if (propertyName is null or "Area" or "Width" or "Height")
                    {
                        OnPropertyChanged("Area");
                    }
                }
            }
        }
        // [<endsnippet AreaChildProperty>]

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
