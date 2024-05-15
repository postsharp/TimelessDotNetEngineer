// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddResiliencePipeline(
    "middleware",
    pipelineBuilder =>
    {
        pipelineBuilder.AddRetry(
                new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<IOException>(),
                    Delay = TimeSpan.FromSeconds( 1 ),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential
                } )
            .ConfigureTelemetry( LoggerFactory.Create( loggingBuilder => loggingBuilder.AddConsole() ) );
    } );

// [<snippet MiddlewareUsage>]
var app = builder.Build();
app.UseMiddleware<ResilienceMiddleware>();

// [<endsnippet MiddlewareUsage>]

var attempts = 0;

app.MapGet(
    "/",
    () =>
    {
        attempts++;

        if ( attempts % 3 != 0 )
        {
            throw new IOException();
        }

        return Task.FromResult( Results.Ok() );
    } );

app.Run();