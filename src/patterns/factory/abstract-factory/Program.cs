// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace Factory;

internal class Program
{
    private static async Task Main( string[] args )
    {
        // [<snippet Initialize>]
        var services = new ServiceCollection();
        services.AddSingleton<IStorageAdapterFactory, StorageAdapterFactory>();
        services.AddHttpClient();
        // [<endsnippet Initialize>]

        var serviceProvider = services.BuildServiceProvider();
        
        // [<snippet Consume>]
        var factory = serviceProvider.GetRequiredService<IStorageAdapterFactory>();
        var storage = factory.CreateStorageAdapter( "https://www.google.com" );
        // [<endsnippet Consume>]
        
        await using var stream = await storage.OpenReadAsync();
        using var reader = new StreamReader( stream );
        Console.WriteLine( await reader.ReadToEndAsync() );
    }
}