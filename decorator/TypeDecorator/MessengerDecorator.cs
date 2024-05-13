namespace TypeDecorator;

public abstract class MessengerDecorator : IMessenger
{
    protected IMessenger Underlying { get; }

    protected MessengerDecorator(IMessenger underlying)
    {
        Underlying = underlying;
    }

    public abstract void Send(Message message);

    public abstract Message Receive();
}
