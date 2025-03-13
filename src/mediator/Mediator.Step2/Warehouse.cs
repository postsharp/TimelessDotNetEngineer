// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Mediator;

public class Warehouse : IWarehouse
{
    private readonly IDistributionMediator _distributionMediator;

    public Warehouse( IDistributionMediator distributionMediator )
    {
        this._distributionMediator = distributionMediator;
    }

    public void DistributeItem( Item item )
    {
        this._distributionMediator.Distribute( item );
    }

    public void AcceptReturnedItem( Item item, IStore store )
    {
        this._distributionMediator.Redistribute( item, store );
    }
}
