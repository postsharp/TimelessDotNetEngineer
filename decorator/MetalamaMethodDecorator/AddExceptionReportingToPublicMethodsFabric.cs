using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

namespace MetalamaMethodDecorator;

internal class AddExceptionReportingToPublicMethodsFabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        amender.Outbound.SelectMany(t => t.AllTypes)
            .SelectMany(t => t.Methods)
            .Where(m => m.Accessibility == Accessibility.Public)
            .AddAspectIfEligible<ReportExceptionsAttribute>();
    }
}
