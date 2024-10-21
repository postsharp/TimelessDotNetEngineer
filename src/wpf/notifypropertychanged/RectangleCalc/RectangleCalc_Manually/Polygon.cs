﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace RectangleArea
{
    internal partial class Polygon : INotifyPropertyChanged
    {
        private double _scaleFactor;

        // This attribute represents a multiplier for dimensions
        public double ScaleFactor
        {
            get
            {
                return this._scaleFactor;
            }

            set
            {
                if ( this._scaleFactor != value )
                {
                    this._scaleFactor = value;
                    this.OnPropertyChanged( nameof(this.ScaleFactor) );
                }
            }
        }

        public Polygon()
        {
            this.ScaleFactor = 1;
        }

        protected virtual void OnPropertyChanged( string propertyName )
        {
            this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}