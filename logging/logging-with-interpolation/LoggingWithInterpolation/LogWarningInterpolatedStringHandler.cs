using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoggingWithInterpolation;

#pragma warning disable CS8618

[InterpolatedStringHandler]
public ref struct LogWarningInterpolatedStringHandler
{
    private readonly StringBuilder messageBuilder;
    private readonly object?[] arguments;
    private readonly ILogger logger;
    private readonly bool isEnabled;

    private int argumentsIndex;

    public LogWarningInterpolatedStringHandler(
        int literalLength, int formattedCount, ILogger logger, out bool isEnabled)
    {
        isEnabled = logger.IsEnabled(LogLevel.Warning);
        if (isEnabled)
        {
            messageBuilder = new StringBuilder(literalLength + (formattedCount * 10));
            arguments = new object?[formattedCount];
            this.logger = logger;
        }
        this.isEnabled = isEnabled;
    }

    public readonly void AppendLiteral(string literal)
        => messageBuilder.Append(literal.Replace("{", "{{", StringComparison.Ordinal).Replace("}", "}}", StringComparison.Ordinal));

    public void AppendFormatted<T>(T value, [CallerArgumentExpression(nameof(value))] string format = null!)
    {
        messageBuilder.Append($"{{{format}}}");
        arguments[argumentsIndex] = value;
        argumentsIndex++;
    }

    public readonly void Log()
    {
        if (isEnabled)
        {
            logger.Log(LogLevel.Warning, messageBuilder.ToString(), arguments);
        }
    }
}