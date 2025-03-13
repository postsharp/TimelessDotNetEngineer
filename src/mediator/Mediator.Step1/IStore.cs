// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public interface IStore
{
    string Name { get; }

    IReadOnlyList<Item> Items { get; }

    void ReceiveItem(Item item);

    void ReturnItem(Item item);
}
