using Metalama.Patterns.Observability;
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

        // [<snippet RgbToGrayscale>]
        public static RgbColor RgbToGrayscale(RgbColor color)
        {
            // Calculate the grayscale value using the luminance formula
            var grayValue = (int)(color.Red * 0.299 + color.Green * 0.587 + color.Blue * 0.114);

            // Ensure the grayscale value is clamped between 0 and 255
            grayValue = Math.Clamp(grayValue, 0, 255);

            return new RgbColor(grayValue, grayValue, grayValue); // Return the grayscale RGB
        }
        // [<endsnippet RgbToGrayscale>]
    }
}
