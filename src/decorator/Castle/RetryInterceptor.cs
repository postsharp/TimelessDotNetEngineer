// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Castle.DynamicProxy;

internal class RetryInterceptor( int retryAttempts = 3, double retryDelay = 1000 ) : IInterceptor
{
    public void Intercept( IInvocation invocation )
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                invocation.Proceed();
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