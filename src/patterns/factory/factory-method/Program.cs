// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal class Program
{
    private static async Task Main( string[] args )
    {
        var factory = new StorageAdapterFactory(); // Factory is used here
        var storageAdapter = factory.CreateStorageAdapter( "path/to/file.txt" );

        await using var stream = await storageAdapter.OpenReadAsync();
        using var reader = new StreamReader( stream );
        Console.WriteLine( await reader.ReadToEndAsync() );
    }
}