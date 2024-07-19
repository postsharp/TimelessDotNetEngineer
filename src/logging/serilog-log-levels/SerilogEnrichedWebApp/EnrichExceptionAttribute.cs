using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;

namespace SerilogEnrichedWebApp;

public class EnrichExceptionAttribute : OverrideMethodAspect
{
    public override dynamic? OverrideMethod()
    {
        try
        {
            return meta.Proceed();
        }
        catch ( Exception e )
        {
            // Compile-time code: create an interpolated string containing the method name and its arguments.
            var methodSignatureBuilder = new InterpolatedStringBuilder();
            methodSignatureBuilder.AddText( meta.Target.Type.ToString() );
            methodSignatureBuilder.AddText( "." );
            methodSignatureBuilder.AddText( meta.Target.Method.Name );
            methodSignatureBuilder.AddText( "(" );

            foreach ( var p in meta.Target.Parameters )
            {
                if ( p.Index > 0 )
                {
                    methodSignatureBuilder.AddText( ", " );
                }

                methodSignatureBuilder.AddExpression( p.Value );
            }

            methodSignatureBuilder.AddText( ")" );

            e.AppendContextFrame( (string) methodSignatureBuilder.ToValue() );

            throw;
        }
    }
}