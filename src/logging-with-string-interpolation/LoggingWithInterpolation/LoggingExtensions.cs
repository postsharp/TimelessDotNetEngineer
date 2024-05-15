// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace LoggingWithInterpolation;

public static class LoggingExtensions
{
    public static void LogTrace(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogTraceInterpolatedStringHandler handler )
        => handler.Log();

    public static void LogDebug(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogDebugInterpolatedStringHandler handler )
        => handler.Log();

    public static void LogInformation(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogInformationInterpolatedStringHandler handler )
        => handler.Log();

    public static void LogWarning(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogWarningInterpolatedStringHandler handler )
        => handler.Log();

    public static void LogError(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogErrorInterpolatedStringHandler handler )
        => handler.Log();

    public static void LogCritical(
        this ILogger logger,
        [InterpolatedStringHandlerArgument( nameof(logger) )]
        LogCriticalInterpolatedStringHandler handler )
        => handler.Log();
}