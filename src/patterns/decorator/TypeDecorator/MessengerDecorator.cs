// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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