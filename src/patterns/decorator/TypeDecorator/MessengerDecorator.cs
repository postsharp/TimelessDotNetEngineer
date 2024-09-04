// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public abstract class MessengerDecorator : IMessenger
{
    protected MessengerDecorator( IMessenger underlying )
    {
        this.Underlying = underlying;
    }

    protected IMessenger Underlying { get; }

    public abstract void Send( Message message );

    public abstract Message Receive();
}