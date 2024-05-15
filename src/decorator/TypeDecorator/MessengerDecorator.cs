public abstract class MessengerDecorator : IMessenger
{
    protected MessengerDecorator(IMessenger underlying)
    {
        Underlying = underlying;
    }

    protected IMessenger Underlying { get; }

    public abstract void Send(Message message);

    public abstract Message Receive();
}