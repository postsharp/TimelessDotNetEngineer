// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step3;

public interface IMemento
{
    IMementoable Originator { get; }

    DateTime MementoTime { get; }
}