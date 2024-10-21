﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleArea
{
    // [<snippet Rectangle>]
    internal partial class Rectangle : Polygon
    {
        public Rectangle( double width, double height )
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Area => this.Height * this.Width;

        public double ScaledArea => this.Area * this.ScaleFactor;
    }

    // [<endsnippet Rectangle>]
}