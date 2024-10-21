// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Observability;

namespace RectangleArea;

// [<snippet Polygon>]
[Observable]
internal partial class Polygon
{
    // This attribute represents a multiplier for dimensions
    public double ScaleFactor { get; set; } = 1;
}

// [<endsnippet Polygon>]