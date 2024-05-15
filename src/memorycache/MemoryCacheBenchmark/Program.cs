// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Caching.Memory;

BenchmarkRunner.Run<MemoryCacheTest>();

public class MemoryCacheTest
{
    private const int n = 10000;
    private readonly (string Key, object Item)[] _existingItems;
    private readonly MemoryCache _memoryCache = new( new MemoryCacheOptions() );
    private readonly (string Key, object Item)[] _newItems;
    private int _nextExistingIndex;
    private int _nextNewIndex;

    public MemoryCacheTest()
    {
        this._existingItems = Enumerable.Range( 0, n ).Select( x => (x.ToString(), new object()) ).ToArray();
        this._newItems = Enumerable.Range( n, 2 * n ).Select( x => (x.ToString(), new object()) ).ToArray();

        foreach ( var item in this._existingItems )
        {
            this._memoryCache.Set( item.Key, item.Item );
        }
    }

    [Benchmark( Baseline = true )]
    public void AllocateObject()
    {
        _ = new object();
    }

    [Benchmark]
    public void Overhead()
    {
        var index = Interlocked.Increment( ref this._nextExistingIndex ) % n;
        _ = this._existingItems[index].Key;
    }

    [Benchmark]
    public void MemoryCache_Get()
    {
        var index = Interlocked.Increment( ref this._nextExistingIndex ) % n;

        this._memoryCache.TryGetValue( this._existingItems[index].Key, out _ );
    }

    [Benchmark]
    public void MemoryCache_Set()
    {
        var index = Interlocked.Increment( ref this._nextExistingIndex ) % n;

        var item = this._existingItems[index];
        this._memoryCache.Set( item.Key, item.Item );
    }

    [Benchmark]
    public void MemoryCache_Add()
    {
        var index = Interlocked.Increment( ref this._nextNewIndex ) % n;

        var item = this._newItems[index];
        this._memoryCache.Set( item.Key, item.Item );
    }
}