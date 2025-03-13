// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public interface IWarehouse
{
    void AddStore(IStore store );

    void DistributeItem(Item item);

    void AcceptReturnedItem(Item item, IStore store);
}
