using System.ComponentModel;

namespace Memento.Step3;

public interface IMementoCaretaker : INotifyPropertyChanged
{
    bool CanUndo { get; }

    void CaptureMemento( IMementoable mementoable );

    void Undo();
}