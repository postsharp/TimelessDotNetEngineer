namespace TypeDecorator;

public class MessengerDecorator : AbstractDecorator, IMessenger
{
    private readonly IMessenger _underlying;

    public MessengerDecorator(IMessenger underlying, Func<Func<object?>, object?> decorator) : base(decorator)
    {
        _underlying = underlying;
    }

    public void Send(Message message) => Invoke(() => _underlying.Send(message));

    public Message Receive() => Invoke(() => _underlying.Receive());
}