// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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