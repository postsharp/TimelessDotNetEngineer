// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoggingWithInterpolation.Step2;

public static class LoggingExtensions
{
    public static void LogInformation(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogInformationInterpolatedStringHandler handler )
        => handler.Log();
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

[InterpolatedStringHandler]
public ref struct LogInformationInterpolatedStringHandler
{
    private readonly StringBuilder messageBuilder;
    private readonly ILogger logger;
    private readonly bool isEnabled;

    // [<snippet handler-constructor>]
    public LogInformationInterpolatedStringHandler( int literalLength, int formattedCount, ILogger logger, out bool isEnabled )
    {
        isEnabled = logger.IsEnabled( LogLevel.Information );

        if ( isEnabled )
        {
            this.messageBuilder = new StringBuilder( literalLength + (formattedCount * 10) );
            this.logger = logger;
        }

        this.isEnabled = isEnabled;
    }
    // [<endsnippet handler-constructor>]

    public void AppendLiteral( string literal ) => this.messageBuilder.Append( literal );

    public void AppendFormatted<T>( T value ) => this.messageBuilder.Append( value?.ToString() );

    public void Log() => this.logger.Log( LogLevel.Information, this.messageBuilder.ToString() );
}