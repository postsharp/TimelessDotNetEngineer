// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

internal class MetricService : IMetricService
{
    private int _distributedItems;
    private int _returnedItems;
    private int _receivedItems;

    public void ItemDistributed( IWarehouse warehouse, Item item, IStore store )
    {
        this._distributedItems++;
    }

    public void ItemReceived( IStore store, Item item )
    {
        this._receivedItems++;
    }

    public void ItemReturned( IStore store, Item item, IWarehouse warehouse )
    {
        this._returnedItems++;
    }

    public void Print()
    {
        Console.WriteLine( $"Distributed: {this._distributedItems}, Received: {this._receivedItems}, Returned: {this._returnedItems}" );
    }
}
