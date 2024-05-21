// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class MessengerDecorator( IMessenger underlying, IPolicy policy ) :
    AbstractDecorator( policy ), IMessenger
{
    public void Send( Message message ) => this.Invoke( () => underlying.Send( message ) );

    public Message Receive() => this.Invoke( underlying.Receive );
}