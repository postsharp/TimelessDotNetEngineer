// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel;

namespace Memento.Step2;

public abstract partial class EditableObject : IEditableObject, IMementoable
{
    private IMemento? _beforeEditMemento;

    public void BeginEdit()
    {
        if ( this._beforeEditMemento != null )
        {
            throw new InvalidOperationException();
        }

        this._beforeEditMemento = this.SaveToMemento();
    }

    public void CancelEdit()
    {
        if ( this._beforeEditMemento == null )
        {
            throw new InvalidOperationException();
        }

        this.RestoreMemento( this._beforeEditMemento );
    }

    public void EndEdit()
    {
        if ( this._beforeEditMemento == null )
        {
            throw new InvalidOperationException();
        }

        this._beforeEditMemento = null;
    }

    public abstract IMemento SaveToMemento();

    public abstract void RestoreMemento( IMemento memento );
}