// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class MessengerDecorator : AbstractDecorator, IMessenger
{
    private readonly IMessenger _underlying;

    public MessengerDecorator( IMessenger underlying, Func<Func<object?>, object?> decorator ) : base( decorator )
    {
        this._underlying = underlying;
    }

    public void Send( Message message )
    {
        this.Invoke( () => this._underlying.Send( message ) );
    }

    public Message Receive()
    {
        return this.Invoke( () => this._underlying.Receive() );
    }
}