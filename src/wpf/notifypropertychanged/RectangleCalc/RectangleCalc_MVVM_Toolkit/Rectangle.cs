using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleArea
{
    // [<snippet Rectangle>]
    partial class Rectangle : Polygon // Polygon inherits from ObservableObject
    {
        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Area))]
        [NotifyPropertyChangedFor(nameof(ScaledArea))]
        public double width;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Area))]
        [NotifyPropertyChangedFor(nameof(ScaledArea))]
        public double height;
        public double Area => this.Height * this.Width;
        new public double ScaledArea => this.Area * this.ScaleFactor;
    }
    // [<endsnippet Rectangle>]
}
