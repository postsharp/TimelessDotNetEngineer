// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

// [<snippet body>]

using Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static void ConfigureServices( IServiceCollection serviceCollection )
    {
        serviceCollection
            .AddSingleton<IPerformanceCounterUploader, AwsPerformanceCounterUploader>();

        serviceCollection.AddSingleton<IPerformanceCounterManager, PerformanceCounterManager>();
    }
}

// [<endsnippet body>]