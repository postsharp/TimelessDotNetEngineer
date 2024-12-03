// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal class HttpStorageAdapter : IStorageAdapter
{
    private readonly string _url;

    public HttpStorageAdapter( string url )
    {
        this._url = url;
    }

    public Task<Stream> OpenReadAsync()
    {
        var client = new HttpClient();

        return client.GetStreamAsync( this._url );
    }

    public Task WriteAsync( Func<Stream, Task> write )
    {
        var content = new PushStreamContent( ( stream, _, _ ) => write( stream ) );
        var client = new HttpClient();

        return client.PostAsync( this._url, content );
    }
}