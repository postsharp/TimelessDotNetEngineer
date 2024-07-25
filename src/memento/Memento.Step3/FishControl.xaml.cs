using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.Step3;

/// <summary>
/// Interaction logic for ItemControl.xaml
/// </summary>
public sealed partial class FishControl
{
    private readonly IMementoCaretaker? _caretaker = App.Host?.Services.GetService<IMementoCaretaker>();

    public FishControl()
    {
        InitializeComponent();
    }

    // [<snippet OnPropertyChanged>]
    protected override void OnPropertyChanged( DependencyPropertyChangedEventArgs e )
    {
        base.OnPropertyChanged( e );

        if (e.Property.Name == nameof(DataContext))
        {
            if (e.OldValue is Fish oldFish)
            {
                oldFish.PropertyChanged -= OnFishPropertyChanged;
            }

            if (e.NewValue is Fish newFish)
            {
                newFish.PropertyChanged += OnFishPropertyChanged;

                // Capture before any change.
                _caretaker?.CaptureMemento( newFish );
            }
        }
    }

    private void OnFishPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        var fish = (Fish?)DataContext;

        if (fish != null)
        {
            _caretaker?.CaptureMemento( fish );
        }
    }
    // [<endsnippet OnPropertyChanged>]

    // [<snippet OnTextBoxUpdated>]
    private void OnTextBoxUpdated( object sender, KeyEventArgs e )
    {
        if (sender is TextBox textBox)
        {
            var binding = BindingOperations.GetBindingExpression( textBox, TextBox.TextProperty );

            if (binding != null)
            {
                binding.UpdateSource();
            }
        }
    }
    // [<endsnippet OnTextBoxUpdated>]

}