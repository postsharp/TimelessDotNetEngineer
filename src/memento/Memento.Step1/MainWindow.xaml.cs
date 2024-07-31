// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Windows;

namespace Memento.Step1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow( MainViewModel mainViewModel )
    {
        this.InitializeComponent();

        this.DataContext = mainViewModel;
    }
}