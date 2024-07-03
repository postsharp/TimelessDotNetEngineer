// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace LoggingWithInterpolation.WithMetalama;

#pragma warning disable CS8618

// [<snippet body>]
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;
using Microsoft.Extensions.Logging;

public class LogAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly ILogger _logger;

    public override dynamic? OverrideMethod()
    {
        // LogTrace can't be called as an extension method here,
        // because Metalama uses the dynamic type to represent run-time values
        // and dynamic is not compatible with extension methods.
        LoggingExtensions.LogTrace( this._logger, BuildInterpolatedString().ToValue() );

        return meta.Proceed();
    }

    [CompileTime]
    private static InterpolatedStringBuilder BuildInterpolatedString()
    {
        var stringBuilder = new InterpolatedStringBuilder();

        // Include the type and method name.
        stringBuilder.AddText( $"{meta.Target.Type}.{meta.Target.Method.Name}(" );

        var first = true;

        // Include each parameter.
        foreach ( var parameter in meta.Target.Parameters )
        {
            if ( !first )
            {
                stringBuilder.AddText( ", " );
            }

            stringBuilder.AddText( $"{parameter.Name}: " );

            stringBuilder.AddExpression( parameter.Value );

            first = false;
        }

        stringBuilder.AddText( ") started." );

        return stringBuilder;
    }
}
// [<endsnippet body>]