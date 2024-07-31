// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace SerilogInAspNetCore;

// [<snippet body>]
using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

internal class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender
            .Select( c => c.GlobalNamespace.GetDescendant( "SerilogInAspNetCore.Controllers" )! )
            .SelectMany( c => c.Types )
            .Where( t => t.Accessibility == Accessibility.Public )
            .SelectMany( c => c.Methods )
            .Where( m => m.Accessibility == Accessibility.Public )
            .AddAspectIfEligible<LogAttribute>();
    }
}

// [<endsnippet body>]