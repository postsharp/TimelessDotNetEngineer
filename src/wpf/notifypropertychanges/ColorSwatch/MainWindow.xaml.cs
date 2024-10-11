// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Windows;

namespace ColorSwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Darker( object sender, RoutedEventArgs e )
        {
            ((ColorViewModel) this.DataContext).RgbColor.IncreaseBrightness( -20 );
        }

        private void Brighter( object sender, RoutedEventArgs e )
        {
            ((ColorViewModel) this.DataContext).RgbColor.IncreaseBrightness( 20 );
        }
    }
}