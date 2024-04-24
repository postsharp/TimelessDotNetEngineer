using MetalamaMethodDecorator;

namespace NoDecorator;

public class Messenger
{
    private int _sendCount = 0;
    private int _receiveCount = 0;

    public void Send(Message message)
    {
        Console.WriteLine("Sending message...");

        // Simulate sending message
        if (this._sendCount++ % 2 == 0)
        {
            Console.WriteLine("Message sent successfully.");
        }
        else
        {
            throw new InvalidOperationException("Failed to send message.");
        }
    }

    public Message Receive()
    {
        Console.WriteLine("Receiving message...");

        // Simulate receiving message
        if (this._receiveCount++ % 2 == 0)
        {
            Console.WriteLine("Message received successfully.");
            return new();
        }
        else
        {
            throw new InvalidOperationException("Failed to receive message.");
        }
    }
}
