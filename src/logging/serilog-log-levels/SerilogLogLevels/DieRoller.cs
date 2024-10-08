// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

internal partial class DieRoller( ILogger<DieRoller> logger )
{
    // This field is currently required because of a bug in the generator, see https://github.com/dotnet/runtime/issues/104801.
    private readonly ILogger<DieRoller> _logger = logger;

    private readonly Random _random = new();

    // [<snippet LogRollingDie-definition>]
    [LoggerMessage( LogLevel.Information, "Rolling a {sides}-sided die." )]
    private partial void LogRollingDie( int sides );
    // [<endsnippet LogRollingDie-definition>]

    public int Roll( int sides = 6 )
    {
        // [<snippet ILogger>]
        this._logger.LogInformation( "Rolling a {sides}-sided die.", sides );
        // [<endsnippet ILogger>]

        // [<snippet Serilog.Log>]
        Log.Information( "Rolling a {sides}-sided die.", sides );
        // [<endsnippet Serilog.Log>]

        // [<snippet LogLevel>]
        this._logger.Log( LogLevel.Information, "Rolling a {sides}-sided die.", sides ); // Microsoft.Extensions.Logging API
        Log.Write( LogEventLevel.Information, "Rolling a {sides}-sided die.", sides ); // Serilog API
        // [<endsnippet LogLevel>]

        // [<snippet LogRollingDie-usage>]
        this.LogRollingDie( sides );
        // [<endsnippet LogRollingDie-usage>]

        return this._random.Next( sides ) + 1;
    }
}