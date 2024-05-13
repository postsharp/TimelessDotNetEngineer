using Polly;

namespace PollyMiddleware;

// [<snippet ResilienceMiddleware>]
public class ResilienceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilienceMiddleware(RequestDelegate next, ResiliencePipelineService resiliencePipelineService)
    {
        this._next = next;
        this._resiliencePipeline = resiliencePipelineService.Pipeline;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        await this._resiliencePipeline.ExecuteAsync(
            async _ => await this._next(httpContext),
            httpContext.RequestAborted);
    }
}
// [<endsnippet ResilienceMiddleware>]