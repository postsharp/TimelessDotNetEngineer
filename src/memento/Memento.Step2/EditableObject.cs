using System.ComponentModel;

namespace Memento.Step2;

public abstract partial class EditableObject : IEditableObject, IMementoable
{
    private IMemento? _beforeEditMemento;
    
    public void BeginEdit()
    {
        this._beforeEditMemento = this.SaveToMemento();
    }

    public void CancelEdit()
    {
        if (this._beforeEditMemento == null)
        {
            throw new InvalidOperationException();
        }
        
        this.RestoreMemento( this._beforeEditMemento );
    }

    public void EndEdit()
    {
        this._beforeEditMemento = null;
    }

    public abstract IMemento SaveToMemento();

    public abstract void RestoreMemento( IMemento memento );
}