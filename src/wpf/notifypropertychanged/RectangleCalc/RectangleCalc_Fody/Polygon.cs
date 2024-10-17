using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RectangleArea
{
    // [<snippet Polygon>]
    partial class Polygon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This attribute represents a multiplier for dimensions
        public double ScaleFactor { get; set; }

        public Polygon() {
            ScaleFactor = 1;
        }
    }
    // [<endsnippet Polygon>]
}
