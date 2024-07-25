namespace Memento.Step2;

public abstract partial class EditableObject
{
    protected abstract class EditableObjectMemento : IMemento
    {
        protected EditableObjectMemento( EditableObject originator )
        {
            this.Originator = originator;
        }
        
        public IMementoable Originator { get; }
    }
}