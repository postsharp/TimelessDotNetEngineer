// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSwatch
{
    // [<snippet TransparentRgbColor>]
    public class TransparentRgbColor : RgbColor
    {
        public TransparentRgbColor(int red, int green, int blue, double alpha)
            : base(red, green, blue) // Call the base class constructor
        {
            this.Alpha = alpha;
        }

        public double Alpha { get; set; }

        public string HexWithAlpha => $"{Hex}{((byte)(Alpha * 255)):x2}";
    }
    // [<endsnippet TransparentRgbColor>]
}
