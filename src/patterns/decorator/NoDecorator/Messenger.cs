// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class Messenger : IMessenger
{
    public void Send( Message message )
    {
        Console.WriteLine(message.Text);
    }

    public Message Receive()
    {
        return new Message( "Hi!" );
    }
}