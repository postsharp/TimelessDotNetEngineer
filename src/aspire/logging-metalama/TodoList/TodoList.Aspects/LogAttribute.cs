using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8618, CS0649

public class LogAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly ILogger _logger;

    public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
    {
        base.BuildEligibility(builder);

        // We cannot introduce dependencies to static classes.
        builder.DeclaringType().MustNotBeStatic();
    }

    public override dynamic? OverrideMethod()
    {
        // Determine if tracing is enabled.
        var isTracingEnabled = this._logger.IsEnabled( LogLevel.Trace );

        // Write entry message.
        if ( isTracingEnabled )
        {
            using ( var guard = LoggingRecursionGuard.Begin() )
            {
                if ( guard.CanLog )
                {
                    var entryMessage = BuildInterpolatedString();
                    var arguments = BuildArguments();
                    entryMessage.AddText( " started." );

                    this._logger.LogTrace( (string) entryMessage.ToValue(), (object?[]) arguments.ToValue()! );
                }
            }
        }

        try
        {
            // Invoke the method and store the result in a variable.
            var result = meta.Proceed();

            if ( isTracingEnabled )
            {
                using ( var guard = LoggingRecursionGuard.Begin() )
                {
                    if ( guard.CanLog )
                    {
                        // Display the success message. The message is different when the method is void.
                        var successMessage = BuildInterpolatedString( true );
                        ExpressionBuilder arguments;
                        var isVoid = meta.Target.Method.GetAsyncInfo().ResultType.Is( typeof( void ) );

                        if ( isVoid )
                        {
                            // When the method is void, display a constant text.
                            successMessage.AddText( " succeeded." );
                            arguments = BuildArguments( true );
                        }
                        else
                        {
                            // When the method has a return value, add it to the message.
                            successMessage.AddText( " returned {result}." );

                            if ( SensitiveParameterFilter.IsSensitive( meta.Target.Method.ReturnParameter ) )
                            {
                                arguments = BuildArguments( true, true, true );
                            }
                            else
                            {
                                arguments = BuildArguments( true, true );
                            }
                        }
                        this._logger.LogTrace( (string) successMessage.ToValue(), (object?[]) arguments.ToValue()! );
                    }
                }
            }

            return result;
        }
        catch ( Exception e ) when ( this._logger.IsEnabled( LogLevel.Warning ) )
        {
            using ( var guard = LoggingRecursionGuard.Begin() )
            {
                if ( guard.CanLog )
                {
                    // Display the failure message.
                    var failureMessage = BuildInterpolatedString();
                    var arguments = BuildArguments();
                    failureMessage.AddText( " failed: " );
                    failureMessage.AddExpression( e.Message );


                    this._logger.LogWarning( (string) failureMessage.ToValue(), (object?[]) arguments.ToValue()! );
                }
            }

            throw;
        }
    }

    // Builds an InterpolatedStringBuilder with the beginning of the message.
    private static InterpolatedStringBuilder BuildInterpolatedString( bool includeOutParameters = false )
    {
        var stringBuilder = new InterpolatedStringBuilder();

        // Include the type and method name.
        stringBuilder.AddText( meta.Target.Type.ToDisplayString( CodeDisplayFormat.MinimallyQualified ) );
        stringBuilder.AddText( "." );
        stringBuilder.AddText( meta.Target.Method.Name );
        stringBuilder.AddText( "(" );
        var isFirst = true;

        // Include a placeholder for each parameter.
        foreach ( var p in meta.Target.Parameters )
        {
            if (!isFirst)
            {
                stringBuilder.AddText( ", " );
            }

            stringBuilder.AddText($"{{{p.Name}}}");

            isFirst = false;
        }

        stringBuilder.AddText( ")" );

        return stringBuilder;
    }

    private static ExpressionBuilder BuildArguments(bool includeOutParameters = false, bool includeResult = false, bool isResultRedacted = false)
    {
        var arguments = new ExpressionBuilder();
        arguments.AppendVerbatim("[");
        var isFirst = true;

        foreach ( var p in meta.Target.Parameters )
        {
            if (!isFirst)
            {
                arguments.AppendVerbatim(", ");
            }

            if ( SensitiveParameterFilter.IsSensitive( p ) )
            {
                // Do not log sensitive parameters.
                arguments.AppendVerbatim( "<redacted>" );
            }
            else if ( p.RefKind == RefKind.Out && !includeOutParameters )
            {
                // When the parameter is 'out', we cannot read the value before the method succeeds.
                arguments.AppendVerbatim( "<out>" );
            }
            else
            {
                // Otherwise, add the parameter value.
                arguments.AppendExpression( p );
            }

            isFirst = false;
        }

        if (includeResult)
        {
            if (!isFirst)
            {
                arguments.AppendVerbatim(", ");
            }

            if (isResultRedacted)
            {
                arguments.AppendVerbatim("\"<redacted>\"");
            }
            else
            {
                arguments.AppendVerbatim("result");
            }
        }

        arguments.AppendVerbatim("]");
        return arguments;
    }
}