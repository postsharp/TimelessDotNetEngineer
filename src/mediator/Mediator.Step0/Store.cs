// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

internal class Store : IStore
{
    private readonly IWarehouse _warehouse;
    private readonly IMetricService _metricService;
    private readonly ILoggingService _loggingService;
    private readonly List<Item> _items;

    public string Name { get; }
    public IReadOnlyList<Item> Items => this._items;

    public Store(string name, IWarehouse warehouse, IMetricService metricService, ILoggingService loggingService )
    {
        this.Name = name;
        this._warehouse = warehouse;
        this._metricService = metricService;
        this._loggingService = loggingService;
        this._items = new List<Item>();
    }

    public void ReceiveItem(Item item)
    {
        this._items.Add( item );
        this._metricService.ItemReceived( this, item );
        this._loggingService.Log( $"{this.Name} received {item.Kind}." );
    }

    public void ReturnItem(Item item)
    {
        this._items.Remove( item );
        this._loggingService.Log( $"{this.Name} returned {item.Kind}." );
        this._warehouse.AcceptReturnedItem( item, this );
    }
}