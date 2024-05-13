using Services;

namespace NoDecorator;

public class Messenger
{
    private const int _retryAttempts = 3;
    private const int _retryDelay = 1000;

    private readonly IExceptionReportingService _reportingService;

    private int _sendCount = 0;
    private int _receiveCount = 0;

    public Messenger(IExceptionReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    public void Send(Message message)
    {
        for (int i = 0; ; i++)
        {
            try
            {
                Console.WriteLine("Sending message...");

                // Simulate unreliable message sending
                if (++this._sendCount % 3 == 0)
                {
                    Console.WriteLine("Message sent successfully.");
                    return;
                }
                else
                {
                    throw new InvalidOperationException("Failed to send message.");
                }
            }
            catch (Exception e)
            {
                if (i < _retryAttempts)
                {
                    var delay = _retryDelay * Math.Pow(2, i);

                    Console.WriteLine($"Failed to send message. Retrying in {delay/1000} seconds... ({i + 1}/{_retryAttempts})");
                    Thread.Sleep((int)delay);
                }
                else
                {
                    this._reportingService.ReportException("Failed to send message", e);
                    throw;
                }
            }
        }
    }

    public Message Receive()
    {
        for (int i = 0; ; i++)
        {
            try
            {
                Console.WriteLine("Receiving message...");

                // Simulate unreliable message receiving
                if (++this._receiveCount % 3 == 0)
                {
                    Console.WriteLine("Message received successfully.");
                    return new("Hi!");
                }
                else
                {
                    throw new InvalidOperationException("Failed to receive message.");
                }
            }
            catch (Exception e)
            {
                if (i < _retryAttempts)
                {
                    var delay = _retryDelay * Math.Pow(2, i);

                    Console.WriteLine($"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{_retryAttempts})");
                    Thread.Sleep((int)delay);
                }
                else
                {
                    this._reportingService.ReportException("Failed to receive message", e);
                    throw;
                }
            }
        }
    }
}
