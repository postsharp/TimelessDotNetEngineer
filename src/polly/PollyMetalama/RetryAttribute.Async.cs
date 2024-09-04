// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Framework.Aspects;

public partial class RetryAttribute
{
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var pipeline = this._resiliencePipelineProvider.GetPipeline( this._pipelineName );

        return await pipeline.ExecuteAsync( Invoke );

        async ValueTask<object?> Invoke( CancellationToken cancellationToken = default )
        {
            return await meta.ProceedAsync();
        }
    }
}