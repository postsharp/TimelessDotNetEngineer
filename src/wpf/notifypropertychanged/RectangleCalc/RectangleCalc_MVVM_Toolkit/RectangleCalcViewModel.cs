using CommunityToolkit.Mvvm.ComponentModel;
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
    partial class RectangleCalcViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Area))]
        public Rectangle rectangle = new Rectangle(10, 5);

        public double Area => this.Rectangle.Area;
    }
}
