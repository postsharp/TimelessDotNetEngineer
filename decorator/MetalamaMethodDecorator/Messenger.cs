namespace MetalamaMethodDecorator;

// [<snippet MetalamaMethodDecorator>]
public partial class Messenger
{
    private int _sendCount = 0;
    private int _receiveCount = 0;

    [Retry, ReportExceptions]
    public void Send(Message message)
    {
        Console.WriteLine("Sending message...");

        // Simulate unreliable message sending
        if (++this._sendCount % 3 == 0)
        {
            Console.WriteLine("Message sent successfully.");
        }
        else
        {
            throw new InvalidOperationException("Failed to send message.");
        }
    }

    [Retry, ReportExceptions]
    public Message Receive()
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
}
// [<endsnippet MetalamaMethodDecorator>]
