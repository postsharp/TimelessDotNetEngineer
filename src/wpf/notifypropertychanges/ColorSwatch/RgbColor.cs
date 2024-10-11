using Metalama.Patterns.Observability;

namespace ColorSwatch
{
    [Observable]
    // [<snippet RgbColor>]
    public partial class RgbColor
    {
        public RgbColor( int red, int green, int blue )
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public int Red { get; set; }

        public int Green { get; set; }

        public int Blue { get; set; }

        public string Hex => $"#{this.Red:x2}{this.Green:x2}{this.Blue:x2}";

    }
    // [<endsnippet RgbColor>]
}
