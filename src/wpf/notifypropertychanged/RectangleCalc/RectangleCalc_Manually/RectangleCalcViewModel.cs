// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace RectangleArea
{
    internal partial class RectangleCalcViewModel : INotifyPropertyChanged
    {
        // [<snippet AreaChildProperty>]
        private Rectangle _rectangle = new( 10, 5 );

        public Rectangle Rectangle
        {
            get => this._rectangle;

            set
            {
                if ( !ReferenceEquals( value, this._rectangle ) )
                {
                    this.UnsubscribeFromRectangle();
                    this._rectangle = value;
                    this.OnPropertyChanged( nameof(this.Rectangle) );
                    this.SubscribeToRectangle( this.Rectangle );
                }
            }
        }

        // [<snippet AreaProp>]
        public double Area => this.Rectangle.Area;

        // [<endsnippet AreaProp>]

        private void SubscribeToRectangle( Rectangle value )
        {
            if ( value != null )
            {
                value.PropertyChanged += this.HandleRectanglePropertyChanged;
            }
        }

        private void HandleRectanglePropertyChanged( object? sender, PropertyChangedEventArgs e )
        {
            {
                var propertyName = e.PropertyName;

                if ( propertyName is null or nameof(this.Rectangle.Width)
                    or nameof(this.Rectangle.Height) )
                {
                    this.OnPropertyChanged( nameof(this.Area) );
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

        protected virtual void OnPropertyChanged( string propertyName )
        {
            this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}