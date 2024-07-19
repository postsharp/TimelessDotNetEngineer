namespace Memento.Step2;

public sealed class Caretaker : ISnapshotCaretaker
{
    private readonly Stack<IMemento> _mementos = new();

    public void CaptureSnapshot( IMementoable mementoable )
    {
        _mementos.Push( mementoable.SaveToMemento() );
    }

    public void Undo()
    {
        if (_mementos.Count > 0)
        {
            var memento = _mementos.Pop();
            memento.Originator.RestoreMemento( memento );
        }
    }

    public bool CanUndo => _mementos.Count > 0;
}