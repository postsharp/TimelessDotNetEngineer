using Castle.DynamicProxy;

internal class ReportExceptionInterceptor : IInterceptor
{
    private readonly IExceptionReportingService _reportingService;

    public ReportExceptionInterceptor(IExceptionReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    public void Intercept(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
        }
        catch (Exception e)
        {
            _reportingService.ReportException("Failed to send message", e);
            throw;
        }
    }
}