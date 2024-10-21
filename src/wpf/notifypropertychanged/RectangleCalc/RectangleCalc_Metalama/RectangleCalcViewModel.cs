// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Observability;

namespace RectangleArea
{
    // [<snippet RectangleCalcViewModel>]
    [Observable]
    internal partial class RectangleCalcViewModel
    {
        public Rectangle Rectangle { get; set; } = new( 10, 5 );

        public double Area => this.Rectangle.Area;
    }

    // [<endsnippet RectangleCalcViewModel>]
}