using System.Windows.Media;
using Metalama.Patterns.Observability;

namespace ColorSwatch
{
    [Observable]
    // [<snippet ColorViewModel>]
    public partial class ColorViewModel
    {
        public RgbColor RgbColor { get; set; } = new RgbColor( 255, 255, 255 );

        public SolidColorBrush BackgroundBrush => ColorHelper.ConvertToBrush( this.RgbColor.Hex );
    }
    // [<endsnippet ColorViewModel>]
}
