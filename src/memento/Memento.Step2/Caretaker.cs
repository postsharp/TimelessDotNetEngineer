using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memento.Step2;

public sealed class Caretaker : ISnapshotCaretaker
{
    private readonly Stack<IMemento> _mementos = new();

    public void CaptureSnapshot( IMementoable mementoable )
    {
        _mementos.Push( mementoable.SaveToMemento() );

        OnPropertyChanged( nameof(CanUndo) );
    }

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