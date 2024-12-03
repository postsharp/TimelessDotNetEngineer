// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Concurrent;

namespace Factory;

internal class TestStorageAdapterFactory : IStorageAdapterFactory
{
    private readonly ConcurrentDictionary<string, TestStorageAdapter> _storageAdapters = new();

    public IStorageAdapter CreateStorageAdapter( string url ) 
        => this._storageAdapters.GetOrAdd( url, s => new TestStorageAdapter() );
}