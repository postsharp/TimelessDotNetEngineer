public class ReportExceptionPolicy( IExceptionReportingService reportingService ) : IPolicy
{
    public T Invoke<T>( Func<T> func )
    {
        try
        {
            return func();
        }
        catch ( Exception e )
        {
            reportingService.ReportException( "Failed to send message", e );

            throw;
        }
    }
}