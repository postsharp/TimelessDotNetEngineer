using System.Data.Common;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace PollyMetalama;

internal class ResiliencePipelineFactory : IResiliencePipelineFactory
{
    private readonly ResiliencePipelineRegistry<StrategyKind> _registry = new();

    public ResiliencePipeline GetPipeline(StrategyKind strategyKind)
    {
        return _registry.GetOrAddPipeline(strategyKind, (builder, context) =>
        {
            switch (context.PipelineKey)
            {
                case StrategyKind.Retry:
                    builder.AddRetry(
                            new RetryStrategyOptions
                            {
                                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                                Delay = TimeSpan.FromSeconds(1),
                                BackoffType = DelayBackoffType.Exponential,
                                MaxRetryAttempts = 3
                            })
                        .ConfigureTelemetry(LoggerFactory.Create(builder => builder.AddConsole()));
                    break;

                case StrategyKind.RetryOnDbException:
                    builder.AddRetry(
                            new RetryStrategyOptions
                            {
                                ShouldHandle = new PredicateBuilder().Handle<DbException>(),
                                Delay = TimeSpan.FromSeconds(1),
                                BackoffType = DelayBackoffType.Exponential,
                                MaxRetryAttempts = 3
                            })
                        .ConfigureTelemetry(LoggerFactory.Create(builder => builder.AddConsole()));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyKind));
            }
        });
    }

    public ResiliencePipeline<T> GetPipeline<T>(StrategyKind strategyKind)
    {
        return _registry.GetOrAddPipeline<T>(strategyKind, (builder, context) =>
        {
            switch (context.PipelineKey)
            {
                case StrategyKind.Retry:
                    builder.AddRetry(
                            new RetryStrategyOptions<T>
                            {
                                ShouldHandle = new PredicateBuilder<T>().Handle<Exception>(),
                                Delay = TimeSpan.FromSeconds(1),
                                BackoffType = DelayBackoffType.Exponential,
                                MaxRetryAttempts = 3
                            })
                        .ConfigureTelemetry(LoggerFactory.Create(builder => builder.AddConsole()));
                    break;

                case StrategyKind.RetryOnDbException:
                    builder.AddRetry(
                            new RetryStrategyOptions<T>
                            {
                                ShouldHandle = new PredicateBuilder<T>().Handle<DbException>(),
                                Delay = TimeSpan.FromSeconds(1),
                                BackoffType = DelayBackoffType.Exponential,
                                MaxRetryAttempts = 3
                            })
                        .ConfigureTelemetry(LoggerFactory.Create(builder => builder.AddConsole()));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyKind));
            }
        });
    }

    public void Dispose()
    {
        _registry.Dispose();
    }
}