// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Extensions.Architecture;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

public class SingletonAttribute : TypeAspect
{
    public override void BuildAspect( IAspectBuilder<INamedType> builder )
    {
        builder.Outbound
            .SelectMany( t => t.Constructors )
            .CanOnlyBeUsedFrom(
                scope => scope.Type( typeof(Startup) ).Or().Namespace( "**.Tests.**" ),
                description: "The class is a [Singleton]." );
    }
}