// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public class Store : IStore
{
    private readonly IReturnMediator _returnMediator;
    private readonly List<Item> _items;

    public string Name { get; }

    public IReadOnlyList<Item> Items => this._items;

    public Store(string name, IReturnMediator returnMediator)
    {
        this.Name = name;
        this._returnMediator = returnMediator;
        this._items = new List<Item>();
    }

    public void ReceiveItem(Item item)
    {
        this._items.Add( item );
    }

    public void ReturnItem(Item item)
    {
        this._items.Remove( item );
        this._returnMediator.ReturnItem( item, this );
    }
}