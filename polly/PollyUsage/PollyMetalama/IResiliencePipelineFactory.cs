﻿using Polly;

namespace PollyMetalama;

public interface IResiliencePipelineFactory : IDisposable
{
    ResiliencePipeline GetPipeline( StrategyKind strategyKind );

    ResiliencePipeline<T> GetPipeline<T>( StrategyKind strategyKind );
}