// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

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