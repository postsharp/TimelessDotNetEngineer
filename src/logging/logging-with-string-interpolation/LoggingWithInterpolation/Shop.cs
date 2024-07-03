// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;

namespace LoggingWithInterpolation;

internal class Shop( ILogger<Shop> logger )
{
    private readonly ILogger<Shop> _logger = logger;

    public void Sell( Product product, decimal price )
    {
        this._logger.LogInformation( $"Product {product} sold for {price:productPrice}." );
    }
}

internal record Product( string Name )
{
    public override string ToString() => this.Name;
}