// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.AspNetCore.Diagnostics;

namespace SerilogEnrichedWebApp;

public class EnchrichedExceptionLoggingHandler( ILogger<EnchrichedExceptionLoggingHandler> logger ) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken )
    {
        if ( exception.GetContextInfo() is { } contextInfo )
        {
            logger.LogError( "{contextInfo}", contextInfo );
        }

        // Never say that we handled the exception.
        return ValueTask.FromResult( false );
    }
}