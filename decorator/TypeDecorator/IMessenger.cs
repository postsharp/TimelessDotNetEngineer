namespace TypeDecorator;

// [<snippet IMessenger>]
public interface IMessenger
{
    void Send(Message message);

    public Message Receive();
}
// [<endsnippet IMessenger>]