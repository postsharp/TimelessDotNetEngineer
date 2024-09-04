// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace NullReferenceException.WithMetalama;

// [<snippet body>]
using Metalama.Framework.Fabrics;
using Metalama.Patterns.Contracts;

internal class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
        => amender.VerifyNotNullableDeclarations();
}

// [<endsnippet body>]