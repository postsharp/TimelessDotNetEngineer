using Polly;
using Polly.Retry;

namespace PollyMiddleware;

// [<snippet ResiliencePipelineService>]
public class ResiliencePipelineService
{
    public ResiliencePipelineService(ILoggerFactory loggerFactory)
    {
        Pipeline = new ResiliencePipelineBuilder()
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

    public ResiliencePipeline Pipeline { get; }
}
// [<endsnippet ResiliencePipelineService>]