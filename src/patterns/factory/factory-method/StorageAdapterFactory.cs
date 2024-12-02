// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    class StorageAdapterFactory
    {
        public IStorageAdapter CreateStorageAdapter(string pathOrUrl)
        {
            // Logic to determine storage adapter type
            if (pathOrUrl.StartsWith("https://"))
            {
                var httpClientFactory = new DefaultHttpClientFactory(); // Simulated DI
                return new HttpStorageAdapter(httpClientFactory, pathOrUrl);
            }
            return new FileSystemStorageAdapter(pathOrUrl);
        }
    }
}
