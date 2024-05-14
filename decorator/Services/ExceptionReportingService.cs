namespace Services;

public class ExceptionReportingService : IExceptionReportingService
{
    public void ReportException(string v, Exception e)
    {
        // Simulate reporting exception
        Console.WriteLine($"{v}: {e.Message}");
    }
}