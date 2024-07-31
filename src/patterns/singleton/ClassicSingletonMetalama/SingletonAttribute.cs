// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;

namespace ClassicSingletonMetalama;

public sealed class SingletonAttribute : TypeAspect
{
    public override void BuildAspect( IAspectBuilder<INamedType> builder )
    {
        base.BuildAspect( builder );

        // Check that there is no public or internal constructor.
        var publicConstructors =
            builder.Target.Constructors.Where(
                    c => c is
                    {
                        IsImplicitlyDeclared: false,
                        Accessibility: Accessibility.Public or Accessibility.Internal
                    } )
                .ToList();

        if ( publicConstructors.Count > 0 )
        {
            foreach ( var constructor in publicConstructors )
            {
                builder.Diagnostics.Report(
                    DiagnosticDefinitions.SingletonCannotDefineConstructor.WithArguments(
                        builder.Target ),
                    constructor );
            }

            return;
        }

        // If there is no default constructor, introduce a private one private constructor.
        var defaultConstructor = builder.Target.Constructors.SingleOrDefault(
            c => c.Parameters.Count == 0 && !c.IsImplicitlyDeclared );

        if ( defaultConstructor == null )
        {
            defaultConstructor = builder.IntroduceConstructor( nameof(this.ConstructorTemplate) )
                .Declaration;
        }

        // Introduce the Instance static property.
        builder.IntroduceAutomaticProperty(
            "Instance",
            builder.Target,
            buildProperty: property =>
            {
                property.Accessibility = Accessibility.Public;
                property.IsStatic = true;
                property.InitializerExpression = defaultConstructor.CreateInvokeExpression();
            } );
    }

    [Template]
    private void ConstructorTemplate() { }

    public override void BuildEligibility( IEligibilityBuilder<INamedType> builder )
    {
        base.BuildEligibility( builder );
        
        builder.MustNotBeStatic();

        builder.MustSatisfy(
            t => t.TypeKind is TypeKind.Class or TypeKind.RecordClass,
            t => $"{t} must be a class" );
    }
}