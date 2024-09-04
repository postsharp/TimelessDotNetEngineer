// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Polly.Registry;

public partial class RetryAttribute : OverrideMethodAspect
{
    private readonly string _pipelineName;

    [IntroduceDependency]
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

    public RetryAttribute( string pipelineName = "default" )
    {
        this._pipelineName = pipelineName;
    }

    public override dynamic? OverrideMethod()
    {
        var pipeline = this._resiliencePipelineProvider.GetPipeline( this._pipelineName );

        return pipeline.Execute( Invoke );

        object? Invoke( CancellationToken cancellationToken = default )
        {
            return meta.Proceed();
        }
    }
}