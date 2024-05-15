// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class Messenger
{
    private const int _retryAttempts = 3;
    private const int _retryDelay = 1000;

    private readonly IExceptionReportingService _reportingService;
    private int _receiveCount;

    private int _sendCount;

    public Messenger( IExceptionReportingService reportingService )
    {
        this._reportingService = reportingService;
    }

    public void Send( Message message )
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                Console.WriteLine( "Sending message..." );

                // Simulate unreliable message sending
                if ( ++this._sendCount % 3 == 0 )
                {
                    Console.WriteLine( "Message sent successfully." );

                    return;
                }

                throw new IOException( "Failed to send message." );
            }
            catch ( Exception e )
            {
                if ( i < _retryAttempts )
                {
                    var delay = _retryDelay * Math.Pow( 2, i );

                    Console.WriteLine( $"Failed to send message. Retrying in {delay / 1000} seconds... ({i + 1}/{_retryAttempts})" );
                    Thread.Sleep( (int) delay );
                }
                else
                {
                    this._reportingService.ReportException( "Failed to send message", e );

                    throw;
                }
            }
        }
    }

    public Message Receive()
    {
        for ( var i = 0;; i++ )
        {
            try
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
            catch ( Exception e )
            {
                if ( i < _retryAttempts )
                {
                    var delay = _retryDelay * Math.Pow( 2, i );

                    Console.WriteLine( $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{_retryAttempts})" );
                    Thread.Sleep( (int) delay );
                }
                else
                {
                    this._reportingService.ReportException( "Failed to receive message", e );

                    throw;
                }
            }
        }
    }
}