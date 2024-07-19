// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoggingWithInterpolation.Step1;

// [<snippet body>]
public static class LoggingExtensions
{
    public static void LogInformation(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogInformationInterpolatedStringHandler handler )
        => handler.Log();
}

[InterpolatedStringHandler]
public ref struct LogInformationInterpolatedStringHandler
{
    private readonly StringBuilder messageBuilder;
    private readonly ILogger logger;

    public LogInformationInterpolatedStringHandler( int literalLength, int formattedCount, ILogger logger )
    {
        this.messageBuilder = new StringBuilder( literalLength + (formattedCount * 10) );
        this.logger = logger;
    }

    public void AppendLiteral( string literal ) => this.messageBuilder.Append( literal );

    public void AppendFormatted<T>( T value ) => this.messageBuilder.Append( value?.ToString() );

    public void Log() => this.logger.Log( LogLevel.Information, this.messageBuilder.ToString() );
}

// [<endsnippet body>]