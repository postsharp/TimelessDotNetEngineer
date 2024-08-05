// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memento.Step2;

public sealed class Caretaker : IMementoCaretaker
{
    private readonly Stack<IMemento> _mementos = new();

    public void CaptureMemento( IMementoable mementoable )
    {
        this._mementos.Push( mementoable.SaveToMemento() );

        this.OnPropertyChanged( nameof(this.CanUndo) );
    }

    public void Undo()
    {
        if ( this._mementos.Count > 0 )
        {
            var memento = this._mementos.Pop();
            memento.Originator.RestoreMemento( memento );
        }

        this.OnPropertyChanged( nameof(this.CanUndo) );
    }

    public bool CanUndo => this._mementos.Count > 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged( [CallerMemberName] string? propertyName = null )
    {
        this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}