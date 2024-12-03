// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal class Program
{
    private static async Task Main( string[] args )
    {
        var storage = new FileSystemStorageAdapter( "C:\\data.txt" );
        await using var stream = await storage.OpenReadAsync();
        using var reader = new StreamReader( stream );
        Console.WriteLine( await reader.ReadToEndAsync() );
    }
}