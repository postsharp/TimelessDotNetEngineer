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
    [NotifyPropertyChangedAttribute]
    public partial class ColorViewModel : INotifyPropertyChanged
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
                    // Notify that BackgroundBrush changed due to the HexColor change
                    OnPropertyChanged(nameof(BackgroundBrush));
                }
            }
        }

        public SolidColorBrush BackgroundBrush
        {
            get {
                return ColorModel.ConvertToBrush(HexColor);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
