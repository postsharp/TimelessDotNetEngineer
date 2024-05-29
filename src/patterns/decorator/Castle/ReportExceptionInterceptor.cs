// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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