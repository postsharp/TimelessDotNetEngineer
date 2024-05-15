// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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