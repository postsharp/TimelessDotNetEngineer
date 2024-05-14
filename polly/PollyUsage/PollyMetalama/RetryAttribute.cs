﻿using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace PollyMetalama;

public class RetryAttribute : OverrideMethodAspect
{
    [IntroduceDependency] private readonly IResiliencePipelineFactory? _resiliencePipelineFactory;

    public RetryAttribute(StrategyKind kind = StrategyKind.Retry)
    {
        Kind = kind;
    }

    public StrategyKind Kind { get; }

    // Template for non-async methods.
    public override dynamic? OverrideMethod()
    {
        if (_resiliencePipelineFactory == null)
        {
            throw new InvalidOperationException("ResiliencePipelineFactory is not available.");
        }

        if (meta.Target.Method.ReturnType.SpecialType == SpecialType.Void)
        {
            void ExecuteVoid()
            {
                meta.Proceed();
            }

            var pipeline = _resiliencePipelineFactory.GetPipeline(Kind);
            pipeline.Execute(ExecuteVoid);
            return null; // Dummy
        }
        else
        {
            object? ExecuteCore()
            {
                return meta.Proceed();
            }

            var pipeline = _resiliencePipelineFactory.GetPipeline(Kind);
            return pipeline.Execute(ExecuteCore);
        }
    }

    // Template for async methods.
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        if (_resiliencePipelineFactory == null)
        {
            throw new InvalidOperationException("ResiliencePipelineFactory is not available.");
        }

        async Task<object?> ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            return await meta.ProceedAsync();
        }

        var cancellationTokenParameter
            = meta.Target.Parameters.LastOrDefault(p => p.Type.Is(typeof(CancellationToken)));

        var pipeline = _resiliencePipelineFactory.GetPipeline(Kind);
        return await pipeline.ExecuteAsync(async token => await ExecuteCoreAsync(token),
            cancellationTokenParameter != null
                ? (CancellationToken)cancellationTokenParameter.Value!
                : CancellationToken.None);
    }

    // Template for async enumerables.
    public override async IAsyncEnumerable<dynamic?> OverrideAsyncEnumerableMethod()
    {
        if (_resiliencePipelineFactory == null)
        {
            throw new InvalidOperationException("ResiliencePipelineFactory is not available.");
        }

        IAsyncEnumerable<object?> ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            return meta.ProceedAsyncEnumerable();
        }

        var cancellationTokenParameter
            = meta.Target.Parameters.LastOrDefault(p => p.Type.Is(typeof(CancellationToken)));
        var cancellationToken = cancellationTokenParameter != null
            ? (CancellationToken)cancellationTokenParameter.Value!
            : CancellationToken.None;

        var pipeline = _resiliencePipelineFactory.GetPipeline(Kind);
        var asyncEnumerable = pipeline.Execute(ExecuteCoreAsync);
        var asyncEnumerator = asyncEnumerable.GetAsyncEnumerator(cancellationToken);

        while (await pipeline.ExecuteAsync(async _ => await asyncEnumerator.MoveNextAsync()))
        {
            yield return meta.Cast(((INamedType)meta.Target.Method.ReturnType).TypeArguments[0],
                asyncEnumerator.Current);
        }
    }

    // Template for async enumerators.
    public override async IAsyncEnumerator<dynamic?> OverrideAsyncEnumeratorMethod()
    {
        if (_resiliencePipelineFactory == null)
        {
            throw new InvalidOperationException("ResiliencePipelineFactory is not available.");
        }

        IAsyncEnumerator<object?> ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            return meta.ProceedAsyncEnumerator();
        }

        var cancellationTokenParameter
            = meta.Target.Parameters.LastOrDefault(p => p.Type.Is(typeof(CancellationToken)));
        var cancellationToken = cancellationTokenParameter != null
            ? (CancellationToken)cancellationTokenParameter.Value!
            : CancellationToken.None;

        var pipeline = _resiliencePipelineFactory.GetPipeline(Kind);
        var asyncEnumerator = pipeline.Execute(() => ExecuteCoreAsync(cancellationToken));

        while (await pipeline.ExecuteAsync(async _ => await asyncEnumerator.MoveNextAsync()))
        {
            yield return asyncEnumerator.Current;
        }
    }
}