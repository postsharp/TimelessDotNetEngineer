using System.ComponentModel;

namespace Memento.Step2;

public interface ISnapshotCaretaker : INotifyPropertyChanged
{
    bool CanUndo { get; }

    void CaptureSnapshot( IMementoable mementoable );

    void Undo();
}