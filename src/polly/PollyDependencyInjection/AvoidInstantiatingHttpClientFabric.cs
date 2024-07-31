// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Extensions.Architecture;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

// [<snippet AvoidInstantiatingHttpClientFabric>]
internal class AvoidInstantiatingHttpClientFabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender
            .SelectReflectionType( typeof(HttpClient) )
            .SelectMany( t => t.Constructors )
            .CannotBeUsedFrom(
                r => r.Always(),
                $"Use {nameof(IHttpClientFactory)} instead." );
    }
}

// [<endsnippet AvoidInstantiatingHttpClientFabric>]