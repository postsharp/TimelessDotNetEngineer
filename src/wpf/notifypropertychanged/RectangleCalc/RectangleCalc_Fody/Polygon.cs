// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace RectangleArea;

// [<snippet Polygon>]
internal partial class Polygon : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // This attribute represents a multiplier for dimensions
    public double ScaleFactor { get; set; } = 1;
}

// [<endsnippet Polygon>]