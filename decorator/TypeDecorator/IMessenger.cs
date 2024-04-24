namespace TypeDecorator;

public interface IMessenger
{
    void Send(Message message);

    public Message Receive();
}
