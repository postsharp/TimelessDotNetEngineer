// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace LoggingWithInterpolation.WithMetalama;

// [<snippet body>]
using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

internal class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender
            .SelectMany( c => c.Types )
            .Where( t => t.TypeKind is not TypeKind.RecordClass or TypeKind.RecordStruct )
            .Where( t => t.Accessibility == Accessibility.Public )
            .SelectMany( c => c.Methods )
            .Where( m => m.Accessibility == Accessibility.Public )
            .AddAspectIfEligible<LogAttribute>();
    }
}

// [<endsnippet body>]