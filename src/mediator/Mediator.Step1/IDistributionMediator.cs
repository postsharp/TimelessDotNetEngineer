﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public interface IDistributionMediator
{
    void Distribute( Item item );
    void AddStore( IStore store );
    void Redistribute( Item item, IStore store );
}
