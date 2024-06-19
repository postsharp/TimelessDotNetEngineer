// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

// [<snippet AddExceptionReportingToPublicMethodsFabric>]
internal class AddExceptionReportingToPublicMethodsFabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender.Outbound.SelectMany( t => t.AllTypes )
            .SelectMany( t => t.Methods )
            .Where( m => m.Accessibility == Accessibility.Public )
            .AddAspectIfEligible<ReportExceptionsAttribute>();
    }
}

// [<endsnippet AddExceptionReportingToPublicMethodsFabric>]