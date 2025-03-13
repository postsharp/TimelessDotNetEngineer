// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Extensions.Architecture.Aspects;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Extensions.Architecture.Validators;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Metalama.Framework.Validation;

namespace Mediator;

internal class Program
{
    static void Main( string[] args )
    {
        Warehouse a1 = new Warehouse();
        Store a2 = new Store();

        a1.DistributeItem();
        a2.ReturnItem();
    }
}

[ShouldOnlyBeUsedFromMediator( typeof( IWarehouseMediator ) )]
public class Warehouse
{
    private readonly IWarehouseMediator _mediator;

    public void DistributeItem() 
    {
        Console.WriteLine( "Foo" );
        this._mediator.Foo( this );
    }

#pragma warning disable CA1822 // Mark members as static
    public void OnReturnItem()
#pragma warning restore CA1822 // Mark members as static
    {
        Console.WriteLine( "OnBar" );
    }
}

[ShouldOnlyBeUsedFromMediator( typeof(IWarehouseMediator) )]
public class Store
{
    private readonly IWarehouseMediator _mediator;

    public void ReturnItem()
    {
        Console.WriteLine( "Bar" );
        this._mediator.Bar( this );
    }

#pragma warning disable CA1822 // Mark members as static
    public void OnDistributeItem()
#pragma warning restore CA1822 // Mark members as static
    {
        Console.WriteLine( "OnFoo" );
    }
}

public class Supplier
{
    private Warehouse a;

    public void ReceiveItem( Item item )
    {
        a.DistributeItem();
    }
}

public class Item
{
}

public interface IMediator
{
}

public interface IWarehouseMediator : IMediator
{
    void Foo( Warehouse actor1 );
    void Bar( Store actor2);
}

public class ActorMediator : IWarehouseMediator
{
    private readonly Warehouse _actor1;
    private readonly Store _actor2;

    public ActorMediator( Warehouse actor1, Store actor2 )
    {
        this._actor1 = actor1;
        this._actor2 = actor2;
    }   

    public void Foo(Warehouse actor1)
    {
        this._actor2.OnDistributeItem();
    }

    public void Bar( Store actor2 )
    {
        this._actor1.OnReturnItem();
    }
}

public class ShouldOnlyBeUsedFromMediatorAttribute : BaseUsageValidationAttribute, IAspect<IMemberOrNamedType>
{
    private readonly Type[] _allowedMediatorTypes;

    public ShouldOnlyBeUsedFromMediatorAttribute( params Type[] allowedMediatorTypes)
    { 
        this._allowedMediatorTypes = allowedMediatorTypes;
    }

    public void BuildEligibility( IEligibilityBuilder<IMemberOrNamedType> builder ) { }

    public void BuildAspect( IAspectBuilder<IMemberOrNamedType> builder )
    {
        var predicateBuilder = new ReferencePredicateBuilder( ReferenceEndRole.Origin, builder );

        builder.Outbound.ValidateOutboundReferences(
            new ReferencePredicateValidator(
                new HasMediatorAccessPredicate( this._allowedMediatorTypes, predicateBuilder ),
                this.Description,
                this.ReferenceKinds ) );
    }
}
internal class HasMediatorAccessPredicate : ReferencePredicate
{
    private readonly Type[] _allowedMediatorTypes;

    public HasMediatorAccessPredicate( Type[] allowedMediatorTypes, ReferencePredicateBuilder builder ) : base( builder )
    {
        this._allowedMediatorTypes = allowedMediatorTypes;
    }

    public override bool IsMatch( ReferenceValidationContext context )
    {
        return context.References.All( r => r.ReferencingDeclaration is IMember { IsStatic:true } || this._allowedMediatorTypes.Any( amt => r.ReferencingDeclaration.GetTopmostNamedType().Is( TypeFactory.GetType( amt ) ) ) );
    }

    public override ReferenceGranularity Granularity => ReferenceGranularity.TopLevelType;
}