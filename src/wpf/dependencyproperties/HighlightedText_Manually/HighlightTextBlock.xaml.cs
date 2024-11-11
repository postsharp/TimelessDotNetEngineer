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
    // Dependency Property with validation and PropertyChanged callback
    public static readonly DependencyProperty HighlightColorProperty =
        DependencyProperty.Register(
            nameof(HighlightColor),
            typeof(Brush),
            typeof(HighlightTextBlock),
            new PropertyMetadata(Brushes.LightGreen, OnHighlightColorChanged),
            ValidateHighlightColor);

    public Brush HighlightColor
    {
        get => (Brush)GetValue(HighlightColorProperty);
        set => SetValue(HighlightColorProperty, value);
    }
    // [<endsnippet HighlightColor_Property>]

    // [<snippet OnHighlightColorChanged>]
    // PropertyChanged callback
    private static void OnHighlightColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        var control = (HighlightTextBlock)d;

        // add a border to the control
        control.BorderBrush = Brushes.Red;
        control.BorderThickness = new Thickness(1);
    }
    // [<endsnippet OnHighlightColorChanged>]

    // [<snippet ValidateHighlightColor>]
    // Validation callback
    private static bool ValidateHighlightColor(object value)
    {
        // Example validation: only accept solid colors (no gradients)
        if (value is SolidColorBrush)
        {
            return true;
        }

        // If validation fails, return false
        Console.WriteLine("Invalid HighlightColor value. Only solid colors are allowed.");
        return false;
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
