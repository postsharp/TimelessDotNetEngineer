public class ExceptionReportingMessenger : MessengerDecorator
{
    private readonly IExceptionReportingService _reportingService;

    public ExceptionReportingMessenger(IMessenger underlying, IExceptionReportingService reportingService) :
        base(underlying)
    {
        _reportingService = reportingService;
    }

    public override void Send(Message message)
    {
        try
        {
            Underlying.Send(message);
        }
        catch (Exception e)
        {
            _reportingService.ReportException("Failed to send message", e);
            throw;
        }
    }

    // [<snippet ExceptionReportingMessenger>]
    public override Message Receive()
    {
        try
        {
            return Underlying.Receive();
        }
        catch (Exception e)
        {
            _reportingService.ReportException("Failed to receive message", e);
            throw;
        }
    }
    // [<endsnippet ExceptionReportingMessenger>]
}