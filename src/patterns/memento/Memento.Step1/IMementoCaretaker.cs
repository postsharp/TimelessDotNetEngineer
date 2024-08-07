﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step1;

public interface IMementoCaretaker
{
    bool CanUndo { get; }

    void CaptureSnapshot( IMementoable mementoable );

    void Undo();
}