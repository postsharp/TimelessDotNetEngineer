// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main()
    {
        var services = new ServiceCollection();
        Startup.ConfigureServices( services );
        var serviceProvider = services.BuildServiceProvider();

        var performanceCounter = serviceProvider.GetRequiredService<IPerformanceCounterManager>();

        for ( var i = 0; i < 100; i++ )
        {
            performanceCounter.IncrementCounter( "Foo" );
            Thread.Sleep( Random.Shared.Next( 10 ) );
        }

        performanceCounter.UploadAndReset();
    }
}