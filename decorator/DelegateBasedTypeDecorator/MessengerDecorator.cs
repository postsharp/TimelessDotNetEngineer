namespace TypeDecorator;

public class MessengerDecorator : AbstractDecorator, IMessenger
{
    public MessengerDecorator(IMessenger underlying, Func<Func<object?>, object?> decorator) : base(decorator)
    {
        Underlying = underlying;
    }

    protected IMessenger Underlying { get; }


    public void Send(Message message)
    {
        Invoke(() => Underlying.Send(message));
    }

    public Message Receive()
    {
        return Invoke(() => Underlying.Receive());
    }
}