using System.Windows;

namespace Memento.Step1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow( MainViewModel mainViewModel )
    {
        InitializeComponent();

        DataContext = mainViewModel;
    }
}