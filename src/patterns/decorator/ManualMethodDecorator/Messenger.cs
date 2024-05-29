// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class Messenger
{
    private readonly IExceptionHandler _exceptionHandler;
    private readonly IRetryHandler _retryHandler;
    private int _receiveCount;

    private int _sendCount;

    public Messenger( IRetryHandler retryHandler, IExceptionHandler exceptionHandler )
    {
        this._retryHandler = retryHandler;
        this._exceptionHandler = exceptionHandler;
    }

    // [<snippet ManualMethodDecoratorUsage>]
    public void Send( Message message )
    {
        void SendImpl( Message message )
        {
            Console.WriteLine( "Sending message..." );

            // Simulate unreliable message sending
            if ( ++this._sendCount % 3 == 0 )
            {
                Console.WriteLine( "Message sent successfully." );
            }
            else
            {
                throw new IOException( "Failed to send message." );
            }
        }

        this._exceptionHandler.ReportWhenFails(
            () => this._retryHandler.Retry( () => SendImpl( message ) ),
            "Failed to send message" );
    }

    // [<endsnippet ManualMethodDecoratorUsage>]

    public Message Receive()
    {
        Message ReceiveImpl()
        {
            Console.WriteLine( "Receiving message..." );

            // Simulate unreliable message receiving
            if ( ++this._receiveCount % 3 == 0 )
            {
                Console.WriteLine( "Message received successfully." );

                return new Message( "Hi!" );
            }

            throw new IOException( "Failed to receive message." );
        }

        return this._exceptionHandler.ReportWhenFails(
            () => this._retryHandler.Retry( () => ReceiveImpl() ),
            "Failed to receive message" );
    }
}