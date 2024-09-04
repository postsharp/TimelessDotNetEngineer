// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public class RetryingMessenger : MessengerDecorator
{
    private readonly int _retryAttempts;
    private readonly int _retryDelay;

    public RetryingMessenger(
        IMessenger underlying,
        int retryAttempts = 3,
        int retryDelay = 1000 ) : base( underlying )
    {
        this._retryAttempts = retryAttempts;
        this._retryDelay = retryDelay;
    }

    public override void Send( Message message )
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                this.Underlying.Send( message );

                return;
            }
            catch ( Exception ) when ( i < this._retryAttempts )
            {
                var delay = this._retryDelay * Math.Pow( 2, i );

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{this._retryAttempts})" );

                Thread.Sleep( (int) delay );
            }
        }
    }

    public override Message Receive()
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                return this.Underlying.Receive();
            }
            catch ( Exception ) when ( i < this._retryAttempts )
            {
                var delay = this._retryDelay * Math.Pow( 2, i );

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{this._retryAttempts})" );

                Thread.Sleep( (int) delay );
            }
        }
    }
}