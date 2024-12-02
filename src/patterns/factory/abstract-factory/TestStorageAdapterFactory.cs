// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

class TestStorageAdapterFactory : IStorageAdapterFactory
{
    private readonly ConcurrentDictionary<string, TestStorageAdapter> _storageAdapters = new();

    public IStorageAdapter CreateStorageAdapter(string url)
    {
        return this._storageAdapters.GetOrAdd(url, s => new TestStorageAdapter());
    }
}
