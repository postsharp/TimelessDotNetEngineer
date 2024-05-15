// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Castle.DynamicProxy;

internal class RetryInterceptor : IInterceptor
{
    private readonly int _retryAttempts;
    private readonly double _retryDelay;

    public RetryInterceptor( int retryAttempts = 3, double retryDelay = 1000 )
    {
        this._retryAttempts = retryAttempts;
        this._retryDelay = retryDelay;
    }

    public void Intercept( IInvocation invocation )
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                invocation.Proceed();
            }
            catch ( Exception ) when ( i < this._retryAttempts )
            {
                var delay = this._retryDelay * Math.Pow( 2, i );

                Console.WriteLine(
                    "Failed to receive message. " +
                    $"Retrying in {delay / 1000} seconds... ({i + 1}/{this._retryAttempts})" );

                Thread.Sleep( (int) delay );
            }
        }
    }
}