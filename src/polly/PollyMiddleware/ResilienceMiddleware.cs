﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Polly.Registry;

// Noted that keyed service does not seem available for middleware.
public class ResilienceMiddleware( RequestDelegate next, ResiliencePipelineProvider<string> pipelineProvider )
{
    public async Task InvokeAsync( HttpContext httpContext )
    {
        var pipeline = pipelineProvider.GetPipeline( "middleware" );

        var bufferingContext = new RestartableHttpContext( httpContext );
        await bufferingContext.InitializeAsync( httpContext.RequestAborted );

        await pipeline.ExecuteAsync(
            async _ =>
            {
                bufferingContext.Reset();
                await next( bufferingContext );
            },
            httpContext.RequestAborted );

        await bufferingContext.AcceptAsync();
    }
}