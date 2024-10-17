namespace RectangleArea
{
    partial class Rectangle : Polygon
    {
        // [<snippet WidthProp>]
        private double _width;
        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(this.Width));
                    OnPropertyChanged(nameof(this.Area));
                    OnPropertyChanged(nameof(this.ScaledArea));
                }
            }
        }
        // [<endsnippet WidthProp>]

        private double _height;
        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(this.Height));
                    OnPropertyChanged(nameof(this.Area));
                    OnPropertyChanged(nameof(this.ScaledArea));
                }
            }
        }
        // [<snippet AreaProp>]
        public double Area => this.Height * this.Width;
        // [<snippet ScaledAreaProp>]
        public double ScaledArea => this.Area * this.ScaleFactor;
        // [<endsnippet AreaProp>]

        protected override void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.ScaleFactor):
                    OnPropertyChanged(nameof(this.ScaledArea));
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }
        // [<endsnippet ScaledAreaProp>]

        public Rectangle(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

    }
}
