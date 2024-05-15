// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace NullReferenceException.WithMetalama;

// [<snippet body>]
using Metalama.Framework.Fabrics;
using Metalama.Patterns.Contracts;

internal class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender ) => amender.Outbound.VerifyNotNullableDeclarations();
}

// [<endsnippet body>]