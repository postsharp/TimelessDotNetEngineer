using Polly;

namespace PollyMiddleware;

public class ResilientMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilientMiddleware(RequestDelegate next, ResiliencePipelineService resiliencePipelineService)
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
