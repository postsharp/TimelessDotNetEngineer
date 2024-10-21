// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace RectangleArea
{
    internal partial class Rectangle : Polygon
    {
        // [<snippet WidthProp>]
        private double _width;

        public double Width
        {
            get
            {
                return this._width;
            }

            set
            {
                if ( this._width != value )
                {
                    this._width = value;
                    this.OnPropertyChanged( nameof(this.Width) );
                    this.OnPropertyChanged( nameof(this.Area) );
                    this.OnPropertyChanged( nameof(this.ScaledArea) );
                }
            }
        }

        // [<endsnippet WidthProp>]

        private double _height;

        public double Height
        {
            get
            {
                return this._height;
            }

            set
            {
                if ( this._height != value )
                {
                    this._height = value;
                    this.OnPropertyChanged( nameof(this.Height) );
                    this.OnPropertyChanged( nameof(this.Area) );
                    this.OnPropertyChanged( nameof(this.ScaledArea) );
                }
            }
        }

        // [<snippet AreaProp>]
        public double Area => this.Height * this.Width;

        // [<snippet ScaledAreaProp>]
        public double ScaledArea => this.Area * this.ScaleFactor;

        // [<endsnippet AreaProp>]

        protected override void OnPropertyChanged( string propertyName )
        {
            switch ( propertyName )
            {
                case nameof(this.ScaleFactor):
                    this.OnPropertyChanged( nameof(this.ScaledArea) );

                    break;
            }

            base.OnPropertyChanged( propertyName );
        }

        // [<endsnippet ScaledAreaProp>]

        public Rectangle( double width, double height )
        {
            this.Width = width;
            this.Height = height;
        }
    }
}