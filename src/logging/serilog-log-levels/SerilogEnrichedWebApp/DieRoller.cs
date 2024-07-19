// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace SerilogEnrichedWebApp;

public class DieRoller
{
    private readonly Random _random = new();

    internal int Roll( int sides ) => this._random.Next( sides ) + 1;
}
