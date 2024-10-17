using System.ComponentModel;

namespace RectangleArea
{
    partial class RectangleCalcViewModel : INotifyPropertyChanged
    {
        // [<snippet RectangleProp>]
        private Rectangle _rectangle;
        public Rectangle Rectangle
        {
            get => _rectangle;

            set
            {
                if (!object.ReferenceEquals(value, _rectangle))
                {
                    UnsubscribeFromRectangle();
                    _rectangle = value;
                    this.OnPropertyChanged(nameof(Rectangle));
                    SubscribeToRectangle(Rectangle);
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
            Rectangle = new Rectangle(10, 5);
        }

        private void SubscribeToRectangle(Rectangle value)
        {
            if (value != null)
            {
                value.PropertyChanged += HandleRectanglePropertyChanged;
            }
            
        }
        
        private void HandleRectanglePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            {
                var propertyName = e.PropertyName;
                if (propertyName is null or "Area" or "Width" or "Height")
                {
                    OnPropertyChanged("Area");
                }
            }
        }

        private void UnsubscribeFromRectangle()
        {
            if ( this._rectangle != null! )
            {
                this._rectangle.PropertyChanged -= this.HandleRectanglePropertyChanged;
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
