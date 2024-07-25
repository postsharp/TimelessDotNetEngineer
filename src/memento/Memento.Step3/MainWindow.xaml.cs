// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow
{
    public MainWindow( MainViewModel mainViewModel )
    {
        this.InitializeComponent();

        this.DataContext = mainViewModel;
    }
}