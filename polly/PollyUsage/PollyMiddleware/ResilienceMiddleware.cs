using Polly;

namespace PollyMiddleware;

// [<snippet ResilienceMiddleware>]
public class ResilienceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilienceMiddleware(RequestDelegate next, ResiliencePipelineService resiliencePipelineService)
    {
        _next = next;
        _resiliencePipeline = resiliencePipelineService.Pipeline;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _resiliencePipeline.ExecuteAsync(
            async _ => await _next(httpContext),
            httpContext.RequestAborted);
    }
}
// [<endsnippet ResilienceMiddleware>]