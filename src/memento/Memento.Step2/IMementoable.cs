// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step2;

public interface IMementoable
{
    IMemento SaveToMemento();

    void RestoreMemento( IMemento memento );
}