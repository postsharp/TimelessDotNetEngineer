// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

class StorageAdapterFactory : IStorageAdapterFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StorageAdapterFactory(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public IStorageAdapter CreateStorageAdapter(string url)
    {
        if (url.StartsWith("https://"))
        {
            return new HttpStorageAdapter(this._httpClientFactory, url);
        }
        else
        {
            return new FileSystemStorageAdapter(url);
        }
    }
}
