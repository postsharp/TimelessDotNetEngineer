// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace SerilogInAspNetCore;

#pragma warning disable CS8618

// [<snippet body>]
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Microsoft.Extensions.Logging;
using System.Text;

public class LogAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly ILogger _logger;

    public override dynamic? OverrideMethod()
    {
        this._logger.LogTrace( BuildFormatString().ToString(), (object[]) meta.Target.Parameters.ToValueArray() );

        return meta.Proceed();
    }

    [CompileTime]
    private static StringBuilder BuildFormatString()
    {
        var stringBuilder = new StringBuilder();

        // Include the type and method name.
        stringBuilder.Append( $"{meta.Target.Type}.{meta.Target.Method.Name}(" );

        var first = true;

        // Include each parameter.
        foreach ( var parameter in meta.Target.Parameters )
        {
            if ( !first )
            {
                stringBuilder.Append( ", " );
            }

            stringBuilder.Append( $"{parameter.Name}: {{{parameter.Name}}}" );

            first = false;
        }

        stringBuilder.Append( ") started." );

        return stringBuilder;
    }
}
// [<endsnippet body>]