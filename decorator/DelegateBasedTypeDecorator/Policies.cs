using Services;

namespace TypeDecorator;

public static class Policies
{
    public static object? Retry(Func<object?> func, int retryAttempts = 3, int retryDelay = 1000)
    {
        for (var i = 0;; i++)
        {
            try
            {
                return func();
            }
            catch (Exception) when (i < retryAttempts)
            {
                var delay = retryDelay * Math.Pow(2, i);

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{retryAttempts})");
                Thread.Sleep((int)delay);
            }
        }
    }

    public static object? ReportException(Func<object?> func, IExceptionReportingService reportingService)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            reportingService.ReportException("Failed to send message", e);
            throw;
        }
    }
}