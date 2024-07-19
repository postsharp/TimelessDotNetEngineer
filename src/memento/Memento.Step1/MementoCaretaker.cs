namespace Memento.Step1;

public sealed class MementoCaretaker : IMementoCaretaker
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