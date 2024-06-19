// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class Client( IMessenger messenger )
{ 
    public void Greet()
    {
        messenger.Send( new Message( "Hello, world" ) );
        Console.WriteLine( "--> " + messenger.Receive().Text );
    }
}