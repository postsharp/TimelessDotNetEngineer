namespace ManualMethodDecorator;

public class Messenger
{
    private readonly IRetryHandler _retryHandler;
    private readonly IExceptionHandler _exceptionHandler;

    public Messenger(IRetryHandler retryHandler, IExceptionHandler exceptionHandler)
    {
        _retryHandler = retryHandler;
        _exceptionHandler = exceptionHandler;
    }

    private int _sendCount = 0;
    private int _receiveCount = 0;

    // [<snippet ManualMethodDecoratorUsage>]
    public void Send(Message message)
    {
        void SendImpl(Message message)
        {
            Console.WriteLine("Sending message...");

            // Simulate unreliable message sending
            if (++_sendCount % 3 == 0)
            {
                Console.WriteLine("Message sent successfully.");
            }
            else
            {
                throw new InvalidOperationException("Failed to send message.");
            }
        }

        _exceptionHandler.ReportWhenFails(
            () => _retryHandler.Retry(() => SendImpl(message)),
            "Failed to send message");
    }
    // [<endsnippet ManualMethodDecoratorUsage>]

    public Message Receive()
    {
        Message ReceiveImpl()
        {
            Console.WriteLine("Receiving message...");

            // Simulate unreliable message receiving
            if (++_receiveCount % 3 == 0)
            {
                Console.WriteLine("Message received successfully.");
                return new("Hi!");
            }
            else
            {
                throw new InvalidOperationException("Failed to receive message.");
            }
        }

        return _exceptionHandler.ReportWhenFails(() => _retryHandler.Retry(() => ReceiveImpl()), "Failed to receive message");
    }
}
