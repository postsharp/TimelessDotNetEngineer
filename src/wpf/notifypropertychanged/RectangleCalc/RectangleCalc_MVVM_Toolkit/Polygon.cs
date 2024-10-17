using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleArea
{
    //[<snippet Polygon>]
    partial class Polygon : ObservableObject
    {
        // This attribute represents a multiplier for dimensions
        [ObservableProperty]
        public double scaleFactor;

        public Polygon() {
            scaleFactor = 1;
        }
    }
    //[<endsnippet Polygon>]
}
