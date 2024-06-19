// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public interface IMetricService
{
    void ItemDistributed( IWarehouse warehouse, Item item, IStore store );
    void ItemReceived( IStore store, Item item );
    void ItemReturned( IStore store, Item item, IWarehouse warehouse );

    void Print();
}
