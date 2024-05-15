﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoggingWithInterpolation;

#pragma warning disable CS8618

// [<snippet body>]
[InterpolatedStringHandler]
public ref struct LogInformationInterpolatedStringHandler
{
    private readonly StringBuilder messageBuilder;
    private readonly object?[] arguments;
    private readonly ILogger logger;
    private readonly bool isEnabled;

    private int argumentsIndex;

    public LogInformationInterpolatedStringHandler( int literalLength, int formattedCount, ILogger logger, out bool isEnabled )
    {
        isEnabled = logger.IsEnabled( LogLevel.Information );

        if ( isEnabled )
        {
            this.messageBuilder = new StringBuilder( literalLength + (formattedCount * 10) );
            this.arguments = new object?[formattedCount];
            this.logger = logger;
        }

        this.isEnabled = isEnabled;
    }

    public readonly void AppendLiteral( string literal )
        => this.messageBuilder.Append( literal.Replace( "{", "{{", StringComparison.Ordinal ).Replace( "}", "}}", StringComparison.Ordinal ) );

    public void AppendFormatted<T>( T value, [CallerArgumentExpression( nameof(value) )] string format = null! )
    {
        this.messageBuilder.Append( $"{{{format}}}" );
        this.arguments[this.argumentsIndex] = value;
        this.argumentsIndex++;
    }

    public readonly void Log()
    {
        if ( this.isEnabled )
        {
            this.logger.Log( LogLevel.Information, this.messageBuilder.ToString(), this.arguments );
        }
    }
}

// [<endsnippet body>]