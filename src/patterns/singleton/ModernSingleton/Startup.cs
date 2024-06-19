// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

// [<snippet body>]
using Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static void ConfigureServices( IServiceCollection serviceCollection )
    {
        serviceCollection.AddSingleton<IPerformanceCounterUploader, AwsPerformanceCounterUploader>();
        serviceCollection.AddSingleton<IPerformanceCounterManager, PerformanceCounterManager>();
    }
}
// [<endsnippet body>]