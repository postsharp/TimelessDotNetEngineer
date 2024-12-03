// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal class FileSystemStorageAdapter : IStorageAdapter
{
    private readonly string _filePath;

    public FileSystemStorageAdapter( string filePath )
    {
        this._filePath = filePath;
    }

    public Task<Stream> OpenReadAsync() 
        => Task.FromResult( (Stream) File.OpenRead( this._filePath ) );

    public async Task WriteAsync( Func<Stream, Task> write )
    {
        await using var stream = File.OpenWrite( this._filePath );
        await write( stream );
    }
}