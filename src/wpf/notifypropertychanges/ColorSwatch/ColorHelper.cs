using System.Windows.Media;

namespace ColorSwatch
{
    internal static class ColorHelper
    {
        public static SolidColorBrush ConvertToBrush( string hexColor )
        {
            try
            {
                return (SolidColorBrush) new BrushConverter().ConvertFromString( hexColor )!;
            }
            catch ( FormatException )
            {
                // Return a default brush (White) in case of invalid input
                return Brushes.White;
            }
        }
    }
}
