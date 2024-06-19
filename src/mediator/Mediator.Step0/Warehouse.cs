// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

internal class Warehouse : IWarehouse
{
    private int _nextStore;
    private readonly List<IStore> _stores;
    private readonly IMetricService _metricService;
    private readonly ILoggingService _loggingService;

    public Warehouse( IMetricService metricService, ILoggingService loggingService )
    {
        this._stores = new List<IStore>();
        this._metricService = metricService;
        this._loggingService = loggingService;
    }

    public void DistributeItem( Item item )
    {
        var store = this._stores[this._nextStore];
        this._loggingService.Log( $"Distributed {item.Kind} to {store.Name}." );
        store.ReceiveItem(item);
        this._metricService.ItemDistributed( this, item, store );
        this.ToNextStore();
    }

    public void AddStore( IStore store )
    {
        this._stores.Add( store );
    }

    public void AcceptReturnedItem( Item item, IStore store )
    {
        this._metricService.ItemReturned( store, item, this );

        if ( this._stores[this._nextStore] == store)
        {
            this.ToNextStore();
        }

        this.DistributeItem( item );
    }

    private void ToNextStore()
    {
        this._nextStore = (this._nextStore + 1) % this._stores.Count;
    }
}
