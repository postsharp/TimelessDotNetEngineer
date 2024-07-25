// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

// [<snippet AddExceptionReportingToPublicMethodsFabric>]
internal class AddExceptionReportingToPublicMethodsFabric : ProjectFabric
{
    public override void AmendProject( IProjectAmender amender )
    {
        amender
            .SelectMany( t => t.AllTypes )
            .SelectMany( t => t.Methods )
            .Where( m => m.Accessibility == Accessibility.Public )
            .AddAspectIfEligible<ReportExceptionsAttribute>();
    }
}

// [<endsnippet AddExceptionReportingToPublicMethodsFabric>]