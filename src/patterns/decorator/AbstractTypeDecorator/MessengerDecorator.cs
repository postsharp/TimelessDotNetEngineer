// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public class MessengerDecorator( IMessenger underlying, IPolicy policy ) :
    AbstractDecorator( policy ), IMessenger
{
    public void Send( Message message ) => this.Invoke( () => underlying.Send( message ) );

    public Message Receive() => this.Invoke( underlying.Receive );
}