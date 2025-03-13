// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Extensions.Architecture.Aspects;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Extensions.Architecture.Validators;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Metalama.Framework.Validation;

[Inheritable]
public class MediatedByAttribute : BaseUsageValidationAttribute, IAspect<IMemberOrNamedType>
{
    private readonly Type[] _allowedMediatorTypes;

    public MediatedByAttribute( params Type[] allowedMediatorTypes )
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
    internal class HasMediatorAccessPredicate : ReferencePredicate
    {
        private readonly Type[] _allowedMediatorTypes;

        public HasMediatorAccessPredicate( Type[] allowedMediatorTypes, ReferencePredicateBuilder builder )
            : base( builder )
        {
            this._allowedMediatorTypes = allowedMediatorTypes;
        }

        public override bool IsMatch( ReferenceValidationContext context )
        {
            return context.References.All(
                r =>
                    r.ReferencingDeclaration is IMember { IsStatic: true }
                    || this._allowedMediatorTypes.Any(
                        amt => r.ReferencingDeclaration.GetTopmostNamedType()!.Is( TypeFactory.GetType( amt ) ) ) );
        }

        public override ReferenceGranularity Granularity => ReferenceGranularity.TopLevelType;
    }

}
