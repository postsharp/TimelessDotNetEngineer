// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Observability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleArea
{
    // [<snippet Polygon>]
    [Observable]
    internal partial class Polygon
    {
        // This attribute represents a multiplier for dimensions
        public double ScaleFactor { get; set; } = 1;
    }
    // [<endsnippet Polygon>]
}