// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memento.Step3;

public sealed class Caretaker : IMementoCaretaker
{
    private readonly Stack<IMemento> _mementos = new();

    // [<snippet CaptureMemento>]
    public void CaptureMemento( IMementoable mementoable )
    {
        if ( this._mementos.Count > 0 )
        {
            var lastMemento = this._mementos.Peek();

            if ( lastMemento.MementoTime > DateTime.Now.AddSeconds( -5 )
                 && lastMemento.Originator == mementoable )
            {
                // Ignore.
                return;
            }
        }

        this._mementos.Push( mementoable.SaveToMemento() );
        this.OnPropertyChanged( nameof(this.CanUndo) );
    }

    // [<endsnippet CaptureMemento>]

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