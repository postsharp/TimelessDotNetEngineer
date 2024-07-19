namespace Memento.Step0;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow
{
    public MainWindow( MainViewModel mainViewModel )
    {
        InitializeComponent();

        DataContext = mainViewModel;
    }
}