// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

internal class HttpStorageAdapter : IStorageAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _url;

    public HttpStorageAdapter( IHttpClientFactory httpClientFactory, string url )
    {
        this._httpClientFactory = httpClientFactory;
        this._url = url;
    }

    public Task<Stream> OpenReadAsync()
    {
        var client = this._httpClientFactory.CreateClient();

        return client.GetStreamAsync( this._url );
    }

    public Task WriteAsync( Func<Stream, Task> write )
    {
        var content = new PushStreamContent( ( stream, _, _ ) => write( stream ) );
        var client = this._httpClientFactory.CreateClient();

        return client.PostAsync( this._url, content );
    }
}