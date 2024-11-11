using Metalama.Patterns.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HighlightedText;

/// <summary>
/// Interaction logic for HighlightTextBlock.xaml
/// </summary>
public partial class HighlightTextBlock : UserControl
{
    public HighlightTextBlock()
    {
        InitializeComponent();
    }

    // [<snippet HighlightColor_Property>]
    [DependencyProperty]
    public Brush HighlightColor { get; set; } = Brushes.LightGreen;
    // [<endsnippet HighlightColor_Property>]

    // [<snippet OnHighlightColorChanged>]
    // PropertyChanged callback
    private void OnHighlightColorChanged<Brush>(Brush? oldValue, Brush? newValue)
    {
        if (oldValue?.Equals(newValue) == true)
        {
            return;
        }

        var control = (HighlightTextBlock)this;

        // add a border to the control
        control.BorderBrush = Brushes.Red;
        control.BorderThickness = new Thickness(1);
    }
    // [<endsnippet OnHighlightColorChanged>]

    // [<snippet ValidateHighlightColor>]
    // Validation callback
    private void ValidateHighlightColor(object value)
    {
        // Example validation: only accept solid colors (no gradients)
        if (value is not SolidColorBrush)
        {
            throw new ArgumentException("Invalid HighlightColor value. Only solid colors are allowed.");
        }
    }
    // [<endsnippet ValidateHighlightColor>]

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(HighlightTextBlock),
            new PropertyMetadata(string.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
