// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public class RetryPolicy( int retryAttempts = 3, int retryDelay = 1000 ) : IPolicy
{
    public T Invoke<T>( Func<T> func )
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                return func();
            }
            catch ( Exception ) when ( i < retryAttempts )
            {
                var delay = retryDelay * Math.Pow( 2, i );

                Console.WriteLine(
                    "Failed to receive message. " +
                    $"Retrying in {delay / 1000} seconds... ({i + 1}/{retryAttempts})" );

                Thread.Sleep( (int) delay );
            }
        }
    }
}