// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step2;

public interface IMemento
{
    IMementoable Originator { get; }
}