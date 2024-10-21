// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangleArea
{
    // [<snippet Rectangle>]
    internal partial class Rectangle : Polygon // Polygon inherits from ObservableObject
    {
        public Rectangle( double width, double height )
        {
            this.width = width;
            this.height = height;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor( nameof(Area) )]
        [NotifyPropertyChangedFor( nameof(ScaledArea) )]
        public double width;

        [ObservableProperty]
        [NotifyPropertyChangedFor( nameof(Area) )]
        [NotifyPropertyChangedFor( nameof(ScaledArea) )]
        public double height;

        public double Area => this.Height * this.Width;

        public double ScaledArea => this.Area * this.ScaleFactor;
    }

    // [<endsnippet Rectangle>]
}