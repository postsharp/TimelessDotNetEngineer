using Metalama.Framework.Code;

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

        try
        {
            _logger.LogDebug( formatString + " started.", (object[])meta.Target.Parameters.ToValueArray() );

            return meta.Proceed();
        }
        finally
        {
            _logger.LogDebug( formatString + " finished.", (object[])meta.Target.Parameters.ToValueArray() );
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