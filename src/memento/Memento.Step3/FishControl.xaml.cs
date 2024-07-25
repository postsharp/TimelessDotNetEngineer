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
            if (e.OldValue is IMementoable and INotifyPropertyChanged newObservable )
            {
                newObservable.PropertyChanged -= OnMementoablePropertyChanged;
            }

            if (e.NewValue is IMementoable newMementoable and INotifyPropertyChanged newObervable )
            {
                newObervable.PropertyChanged += OnMementoablePropertyChanged;

                // Capture _before_ any change.
                _caretaker?.CaptureMemento( newMementoable );
            }
        }
    }

    private void OnMementoablePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        var mementoable = (IMementoable?) DataContext;

        if (mementoable != null)
        {
            _caretaker?.CaptureMemento( mementoable );
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