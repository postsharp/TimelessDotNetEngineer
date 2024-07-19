using Serilog.Context;

namespace SerilogInAspNetCore;

public sealed class PushPropertiesMiddleware : IMiddleware
{
    public async Task InvokeAsync( HttpContext context, RequestDelegate next )
    {
        var requestId = Guid.NewGuid().ToString();

        using (LogContext.PushProperty( "Client", context.Request.Host ))
        using (LogContext.PushProperty( "RequestId", requestId ))
        {
            await next( context );
        }
    }
}