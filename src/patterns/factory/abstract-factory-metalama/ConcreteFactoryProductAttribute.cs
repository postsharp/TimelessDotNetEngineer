// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Extensions.Architecture;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Factory;

internal class ConcreteFactoryProductAttribute : TypeAspect
{
    public override void BuildAspect(IAspectBuilder<INamedType> builder) =>
        builder.Outbound.SelectMany(t => t.Constructors)
            .CanOnlyBeUsedFrom(scope => scope.Namespace("**.Tests")
                    .Or().Type(typeof(IStorageAdapterFactory)),
                "The class is a concrete factory.");
}
