using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memento.Step3;

public sealed class Caretaker : IMementoCaretaker
{
    private readonly Stack<IMemento> _mementos = new();

    // [<snippet CaptureMemento>]
    public void CaptureMemento( IMementoable mementoable )
    {
        if (_mementos.Count > 0)
        {
            var lastMemento = _mementos.Peek();

            if (lastMemento.MementoTime > DateTime.Now.AddSeconds( -5 ) && lastMemento.Originator == mementoable)
            {
                // Ignore.
                return;
            }
        }

        _mementos.Push( mementoable.SaveToMemento() );
        OnPropertyChanged( nameof(CanUndo) );
    }

    // [<endsnippet CaptureMemento>]

    public void Undo()
    {
        if (_mementos.Count > 0)
        {
            var memento = _mementos.Pop();
            memento.Originator.RestoreMemento( memento );
        }

        OnPropertyChanged( nameof(CanUndo) );
    }

    public bool CanUndo => _mementos.Count > 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged( [CallerMemberName] string? propertyName = null )
    {
        PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}