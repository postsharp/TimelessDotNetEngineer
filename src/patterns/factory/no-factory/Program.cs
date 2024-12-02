// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

class Program
{
    static async Task Main(string[] args)
    {
        // Direct instantiation
        var storageAdapter = new FileSystemStorageAdapter("path/to/file.txt");
        // Or:
        // var storageAdapter = new HttpStorageAdapter(httpClientFactory, "https://example.com");

        await using var stream = await storageAdapter.OpenReadAsync();
        using var reader = new StreamReader(stream);
        Console.WriteLine(await reader.ReadToEndAsync());
    }
}