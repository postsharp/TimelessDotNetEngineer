// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace Memento.Step3;

public interface IMementoCaretaker : INotifyPropertyChanged
{
    bool CanUndo { get; }

    void CaptureMemento( IMementoable mementoable );

    void Undo();
}