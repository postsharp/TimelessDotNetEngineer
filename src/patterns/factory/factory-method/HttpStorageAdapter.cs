// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Factory;

class HttpStorageAdapter : IStorageAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _url;

    public HttpStorageAdapter(IHttpClientFactory httpClientFactory, string url)
    {
        _httpClientFactory = httpClientFactory;
        _url = url;
    }

    public Task<Stream> OpenReadAsync()
    {
        var client = _httpClientFactory.CreateClient();
        return client.GetStreamAsync(_url);
    }

    public Task WriteAsync(Func<Stream, Task> write)
    {
        var content = new PushStreamContent((stream, _, _) => write(stream));
        var client = _httpClientFactory.CreateClient();
        return client.PostAsync(_url, content);
    }
}
