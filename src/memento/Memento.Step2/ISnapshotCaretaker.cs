namespace Memento.Step2;

public interface ISnapshotCaretaker
{
    bool CanUndo { get; }

    void CaptureSnapshot( IMementoable mementoable );

    void Undo();
}