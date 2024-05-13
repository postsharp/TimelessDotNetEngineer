using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoggingWithInterpolation.Step1;

// [<snippet body>]
public static class LoggingExtensions
{
    public static void LogInformation(
        this ILogger logger,
        [InterpolatedStringHandlerArgument(nameof(logger))] LogInformationInterpolatedStringHandler handler)
        => handler.Log();
}

[InterpolatedStringHandler]
public ref struct LogInformationInterpolatedStringHandler
{
    private readonly StringBuilder messageBuilder;
    private readonly ILogger logger;

    public LogInformationInterpolatedStringHandler(
        int literalLength, int formattedCount, ILogger logger)
    {
        messageBuilder = new StringBuilder(literalLength + (formattedCount * 10));
        this.logger = logger;
    }

    public void AppendLiteral(string literal) => messageBuilder.Append(literal);

    public void AppendFormatted<T>(T value) => messageBuilder.Append(value?.ToString());

    public void Log() => logger.Log(LogLevel.Information, messageBuilder.ToString());
}
// [<endsnippet body>]