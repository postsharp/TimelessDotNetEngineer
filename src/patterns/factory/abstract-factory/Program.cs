// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace Factory;
class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IStorageAdapterFactory, StorageAdapterFactory>();
        services.AddHttpClient();

        var factory = services.BuildServiceProvider().GetRequiredService<IStorageAdapterFactory>();

        var storage = factory.CreateStorageAdapter("https://www.google.com");
        await using var stream = await storage.OpenReadAsync();
        using var reader = new StreamReader(stream);
        Console.WriteLine(await reader.ReadToEndAsync());

    }
}
