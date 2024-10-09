using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorSwatch
{
    public class ColorModel
    {
        public string HexColor { get; set; }

        // Convert the HexColor string to a SolidColorBrush
        public static SolidColorBrush ConvertToBrush(string hexColor)
        {
            try
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString(hexColor);
            }
            catch (FormatException)
            {
                // Return a default brush (White) in case of invalid input
                return Brushes.White;
            }
        }
    }
}
