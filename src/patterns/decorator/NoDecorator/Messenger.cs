// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public class Messenger : IMessenger
{
    public void Send( Message message )
    {
        Console.WriteLine( message.Text );
    }

    public Message Receive()
    {
        return new Message( "Hi!" );
    }
}