// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Framework.Code;
using Serilog.Events;

namespace SerilogInAspNetCore;

#pragma warning disable CS8618

// [<snippet body>]
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Microsoft.Extensions.Logging;

public class LogAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly ILogger _logger;

    public override dynamic? OverrideMethod()
    {
        var formatString = BuildFormatString();

        var isLoggingEnabled = this._logger.IsEnabled( LogLevel.Trace );

        try
        {
            if ( isLoggingEnabled )
            {
                this._logger.LogTrace(
                    formatString + " started.",
                    (object[]) meta.Target.Parameters.ToValueArray() );
            }

            return meta.Proceed();
        }
        finally
        {
            if ( isLoggingEnabled )
            {
                this._logger.LogTrace(
                    formatString + " finished.",
                    (object[]) meta.Target.Parameters.ToValueArray() );
            }
        }
    }

    [CompileTime]
    private static string BuildFormatString()
    {
        var parameters = meta.Target.Parameters
            .Where( x => x.RefKind != RefKind.Out )
            .Select( p => $"{p.Name}: {{{p.Name}}}" );

        return $"{meta.Target.Type}.{meta.Target.Method.Name}({string.Join( ", ", parameters )})";
    }
}

// [<endsnippet body>]