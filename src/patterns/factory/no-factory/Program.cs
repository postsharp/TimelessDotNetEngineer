// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal class Program
{
    private static async Task Main( string[] args )
    {
        var url = args[0];
        
        // Direct instantiation
        IStorageAdapter storageAdapter = url.StartsWith( "https://" ) 
            ? new HttpStorageAdapter(url)
            : new FileSystemStorageAdapter( "url" );

        // Using the adapter.
        await using var stream = await storageAdapter.OpenReadAsync();
        using var reader = new StreamReader( stream );
        Console.WriteLine( await reader.ReadToEndAsync() );
    }
}