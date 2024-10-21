// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

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
    internal partial class Polygon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This attribute represents a multiplier for dimensions
        public double ScaleFactor { get; set; }

        public Polygon()
        {
            this.ScaleFactor = 1;
        }
    }

    // [<endsnippet Polygon>]
}