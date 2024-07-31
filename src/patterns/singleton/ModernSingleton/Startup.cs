// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static void ConfigureServices( IServiceCollection serviceCollection )
    {
        serviceCollection
            .AddSingleton<IPerformanceCounterUploader, AwsPerformanceCounterUploader>();

        serviceCollection.AddSingleton<PerformanceCounterManager>();
    }
}
