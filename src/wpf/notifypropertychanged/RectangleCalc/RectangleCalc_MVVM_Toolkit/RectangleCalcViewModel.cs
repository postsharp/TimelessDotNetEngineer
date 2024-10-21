// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

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
    internal partial class RectangleCalcViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor( nameof(Area) )]
        public Rectangle rectangle = new( 10, 5 );

        // WARNING! This property that depends on the child object Rectangle does not raise PropertyChanged events.
        public double Area => this.Rectangle.Area;
    }
}