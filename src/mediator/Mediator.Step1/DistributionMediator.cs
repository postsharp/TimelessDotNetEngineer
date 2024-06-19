// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public class DistributionMediator : IDistributionMediator
{
    private int _nextStore;
    private readonly Lazy<IWarehouse> _warehouse;
    private readonly List<IStore> _stores = new();
    private readonly IMetricService _metricService;
    private readonly ILoggingService _loggingService;

    public DistributionMediator( Lazy<IWarehouse> warehouse, IMetricService metricService, ILoggingService loggingService )
    {
        this._warehouse = warehouse;
        this._metricService = metricService;
        this._loggingService = loggingService;
    }

    public void Distribute( Item item )
    {
        var store = this._stores[this._nextStore];
        this._metricService.ItemDistributed( this._warehouse.Value, item, store );
        this._loggingService.Log( $"Distributed {item.Kind} to {store.Name}." );
        store.ReceiveItem( item );
        this._loggingService.Log( $"{store.Name} received {item.Kind}." );
        this._metricService.ItemReceived( store, item );
        this.ToNextStore();
    }

    public void AddStore( IStore store )
    {
        this._stores.Add( store );
    }

    public void Redistribute( Item item, IStore store )
    {
        if ( this._stores[this._nextStore] == store )
        {
            this.ToNextStore();
        }

        this.Distribute( item );
    }

    private void ToNextStore()
    {
        this._nextStore = (this._nextStore + 1) % this._stores.Count;
    }
}