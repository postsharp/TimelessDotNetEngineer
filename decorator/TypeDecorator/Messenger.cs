namespace TypeDecorator;

public class Messenger : IMessenger
{
    private int _receiveCount;
    private int _sendCount;

    public void Send(Message message)
    {
        Console.WriteLine("Sending message...");

        // Simulate unreliable message sending
        if (++_sendCount % 3 == 0)
        {
            Console.WriteLine("Message sent successfully.");
        }
        else
        {
            throw new IOException("Failed to send message.");
        }
    }

    public Message Receive()
    {
        Console.WriteLine("Receiving message...");

        // Simulate unreliable message receiving
        if (++_receiveCount % 3 == 0)
        {
            Console.WriteLine("Message received successfully.");
            return new Message("Hi!");
        }

        throw new IOException("Failed to receive message.");
    }
}