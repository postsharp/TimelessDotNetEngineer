// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Observability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

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