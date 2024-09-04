// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Castle.DynamicProxy;

internal class ReportExceptionInterceptor : IInterceptor
{
    private readonly IExceptionReportingService _reportingService;

    public ReportExceptionInterceptor( IExceptionReportingService reportingService )
    {
        this._reportingService = reportingService;
    }

    public void Intercept( IInvocation invocation )
    {
        try
        {
            invocation.Proceed();
        }
        catch ( Exception e )
        {
            this._reportingService.ReportException( "Failed to send message", e );

            throw;
        }
    }
}