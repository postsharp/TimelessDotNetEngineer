// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangleArea;

//[<snippet Polygon>]
internal partial class Polygon : ObservableObject
{
    // This attribute represents a multiplier for dimensions
    [ObservableProperty]
    public double scaleFactor = 1;
}

//[<endsnippet Polygon>]