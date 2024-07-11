using System.Windows;

namespace Memento;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow( MainViewModel mainViewModel )
    {
        InitializeComponent();

        this.DataContext = mainViewModel;
    }
}