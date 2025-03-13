// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public class ReturnMediator : IReturnMediator
{
    private readonly IWarehouse _warehouse;
    private readonly IMetricService _metricService;
    private readonly ILoggingService _loggingService;

    public ReturnMediator( IWarehouse warehouse, IMetricService metricService, ILoggingService loggingService )
    {
        this._warehouse = warehouse;
        this._metricService = metricService;
        this._loggingService = loggingService;
    }

    public void ReturnItem( Item item, IStore store )
    {
        this._loggingService.Log( $"{store.Name} returned {item.Kind}." );
        this._metricService.ItemReturned(store, item, this._warehouse);
        this._warehouse.AcceptReturnedItem( item, store );
    }
}