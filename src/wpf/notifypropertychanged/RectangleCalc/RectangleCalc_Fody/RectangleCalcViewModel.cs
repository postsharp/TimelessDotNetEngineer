// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace RectangleArea;

internal partial class RectangleCalcViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Rectangle Rectangle { get; set; } = new( 10, 5 );

    // WARNING! This property that depends on the child object Rectangle does not raise PropertyChanged events.
    public double Area => this.Rectangle.Area;
}