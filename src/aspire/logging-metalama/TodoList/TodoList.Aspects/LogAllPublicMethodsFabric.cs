// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Fabrics;
using Metalama.Framework.Code;

// [<snippet Fabric>]
internal class LogAllPublicMethodsFabric : TransitiveProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
        => amender
            .SelectTypes()
            .Where( type => type.Accessibility == Accessibility.Public )
            .SelectMany( type => type.Methods )
            .Where( method => method.Accessibility == Accessibility.Public && method.Name != "ToString" )
            .AddAspectIfEligible<LogAttribute>();
}

// [<endsnippet Fabric>]