using Polly;
using Polly.Registry;

namespace PollyMiddleware;

public class ResilienceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilienceMiddleware(RequestDelegate next, ResiliencePipelineProvider<string> pipelineProvider)
    {
        _next = next;
        _resiliencePipeline = pipelineProvider.GetPipeline("middleware");
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _resiliencePipeline.ExecuteAsync(
            async _ => await _next(httpContext),
            httpContext.RequestAborted);
    }
}