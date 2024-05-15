// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Caching.Memory;

BenchmarkRunner.Run<MemoryCacheTest>();

public class MemoryCacheTest
{
    private const int n = 10000;
    private readonly (string Key, object Item)[] _existingItems;
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly (string Key, object Item)[] _newItems;
    private int _nextExistingIndex;
    private int _nextNewIndex;

    public MemoryCacheTest()
    {
        _existingItems = Enumerable.Range(0, n).Select(x => (x.ToString(), new object())).ToArray();
        _newItems = Enumerable.Range(n, 2 * n).Select(x => (x.ToString(), new object())).ToArray();

        foreach (var item in _existingItems) _memoryCache.Set(item.Key, item.Item);
    }


    [Benchmark(Baseline = true)]
    public void AllocateObject()
    {
        _ = new object();
    }

    [Benchmark]
    public void Overhead()
    {
        var index = Interlocked.Increment(ref _nextExistingIndex) % n;
        _ = _existingItems[index].Key;
    }

    [Benchmark]
    public void MemoryCache_Get()
    {
        var index = Interlocked.Increment(ref _nextExistingIndex) % n;

        _memoryCache.TryGetValue(_existingItems[index].Key, out _);
    }

    [Benchmark]
    public void MemoryCache_Set()
    {
        var index = Interlocked.Increment(ref _nextExistingIndex) % n;

        var item = _existingItems[index];
        _memoryCache.Set(item.Key, item.Item);
    }

    [Benchmark]
    public void MemoryCache_Add()
    {
        var index = Interlocked.Increment(ref _nextNewIndex) % n;

        var item = _newItems[index];
        _memoryCache.Set(item.Key, item.Item);
    }
}