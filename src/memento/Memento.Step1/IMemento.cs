// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step1;

public interface IMemento
{
    IMementoable Originator { get; }
}