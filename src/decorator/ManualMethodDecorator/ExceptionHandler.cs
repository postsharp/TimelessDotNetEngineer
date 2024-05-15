// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class ExceptionHandler : IExceptionHandler
{
    private readonly IExceptionReportingService _reportingService;

    public ExceptionHandler( IExceptionReportingService reportingService )
    {
        this._reportingService = reportingService;
    }

    // [<snippet ManualMethodDecoratorImpl>]
    public void ReportWhenFails( Action action, string message )
    {
        try
        {
            action();
        }
        catch ( Exception e )
        {
            this._reportingService.ReportException( message, e );

            throw;
        }
    }

    // [<endsnippet ManualMethodDecoratorImpl>]

    public T ReportWhenFails<T>( Func<T> action, string message )
    {
        try
        {
            return action();
        }
        catch ( Exception e )
        {
            this._reportingService.ReportException( message, e );

            throw;
        }
    }
}