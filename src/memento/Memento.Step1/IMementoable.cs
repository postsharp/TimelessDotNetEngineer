// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step1;

public interface IMementoable
{
    IMemento SaveToMemento();

    void RestoreMemento( IMemento memento );
}