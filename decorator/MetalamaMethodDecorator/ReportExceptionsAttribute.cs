using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Services;

namespace MetalamaMethodDecorator;

public class ReportExceptionsAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly IExceptionReportingService? _reportingService;

    public override dynamic? OverrideMethod()
    {
        try
        {
            return meta.Proceed();
        }
        catch (Exception e)
        {
            if (this._reportingService == null)
            {
                throw new AggregateException(
                    e,
                    new InvalidOperationException(
                        $"Failed to report exception. The {nameof(_reportingService)} is missing."));
            }

            this._reportingService.ReportException($"Method '{meta.Target.Method.DeclaringType.Name}.{meta.Target.Method}' failed.", e);

            throw;
        }
    }

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        try
        {
            return await meta.Proceed();
        }
        catch (Exception e)
        {
            if (this._reportingService == null)
            {
                throw new AggregateException(
                    e,
                    new InvalidOperationException(
                        $"Failed to report exception. The {nameof(_reportingService)} is missing."));
            }

            this._reportingService.ReportException($"Method '{meta.Target.Method.DeclaringType.Name}.{meta.Target.Method}' failed.", e);

            throw;
        }
    }
}