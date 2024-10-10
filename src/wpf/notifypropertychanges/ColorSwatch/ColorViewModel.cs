using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Metalama.Patterns.Observability;

namespace ColorSwatch
{
    [Observable]
    public partial class ColorViewModel
    {
        private ColorModel _colorModel;

        public ColorViewModel()
        {
            _colorModel = new ColorModel();
            HexColor = "#FFFFFF";
        }

        public string HexColor
        {
            get { return _colorModel.HexColor; }
            set
            {
                if (_colorModel.HexColor != value)
                {
                    _colorModel.HexColor = value;
                }
            }
        }

        public SolidColorBrush BackgroundBrush
        {
            get {
                return ColorModel.ConvertToBrush(HexColor);
            }
        }
    }
}
