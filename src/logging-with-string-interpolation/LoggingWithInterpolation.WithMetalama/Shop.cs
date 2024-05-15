// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace LoggingWithInterpolation.WithMetalama;

public partial class Shop
{
    public void Sell( Product product, decimal price ) { }
}

public record Product( string Name )
{
    public override string ToString() => this.Name;
}