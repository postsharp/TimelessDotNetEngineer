using Metalama.Framework.Aspects;

public partial class RetryAttribute
{
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var pipeline = _resiliencePipelineProvider.GetPipeline(_pipelineName);
        return await pipeline.ExecuteAsync(Invoke);

        async ValueTask<object?> Invoke(CancellationToken cancellationToken = default)
        {
            return await meta.ProceedAsync();
        }
    }
}