// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Fabrics;

namespace SerilogEnrichedWebApp;

public class Fabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender.Outbound
            .SelectMany( p => p.Types )
            .SelectMany( t => t.Methods )
            .AddAspectIfEligible<EnrichExceptionAttribute>();
    }
}
