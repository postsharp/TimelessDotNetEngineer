// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public class Client( IMessenger messenger )
{
    public void Greet()
    {
        messenger.Send( new Message( "Hello, world" ) );
        Console.WriteLine( "--> " + messenger.Receive().Text );
    }
}