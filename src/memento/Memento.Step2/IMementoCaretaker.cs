using System.ComponentModel;

namespace Memento.Step2;

public interface IMementoCaretaker : INotifyPropertyChanged
{
    bool CanUndo { get; }

    void CaptureMemento( IMementoable mementoable );

    void Undo();
}