using Metalama.Framework.Aspects;
using System.Text;

namespace SerilogEnrichedWebApp;

public static class EnrichExceptionHelper
{
    private const string _slotName = "MetalamaEnrichExceptionContext";

    [ExcludeAspect( typeof(EnrichExceptionAttribute) )]
    public static void AppendContextFrame( this Exception e, string frame )
    {
        // Get or create a StringBuilder for the exception where we will add additional context data.
        var stringBuilder = (StringBuilder?) e.Data[_slotName];

        if ( stringBuilder == null )
        {
            stringBuilder = new StringBuilder();
            e.Data[_slotName] = stringBuilder;
        }

        // Add current context information to the string builder.
        stringBuilder.AppendLine( frame );
    }

    public static string? GetContextInfo( this Exception e )
        => ((StringBuilder?) e.Data[_slotName])?.ToString();
}