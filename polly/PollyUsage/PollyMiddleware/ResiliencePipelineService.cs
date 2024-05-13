using Polly;
using Polly.Retry;

namespace PollyMiddleware;

// [<snippet ResiliencePipelineService>]
public class ResiliencePipelineService
{
    public ResiliencePipeline Pipeline { get; }

    public ResiliencePipelineService(ILoggerFactory loggerFactory)
    {
        this.Pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential
            })
            .ConfigureTelemetry(loggerFactory)
            .Build();
    }
}
// [<endsnippet ResiliencePipelineService>]