// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Concurrent;
using System.Diagnostics;

// [<snippet body>]
public class PerformanceCounterManager
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private readonly ConcurrentDictionary<string, int> _counters = new();

    private PerformanceCounterManager() { }

    public static PerformanceCounterManager Instance { get; } = new();

    public void IncrementCounter( string name )
        => this._counters.AddOrUpdate( name, 1, ( _, value ) => value + 1 );

    public void PrintAndReset()
    {
        Dictionary<string, int> oldCounters;
        TimeSpan elapsed;

        lock ( this._stopwatch )
        {
            oldCounters = this._counters.RemoveAll();

            elapsed = this._stopwatch.Elapsed;
            this._stopwatch.Restart();
        }

        foreach ( var counter in oldCounters )
        {
            Console.WriteLine(
                $"{counter.Key}: {counter.Value / elapsed.TotalSeconds:f2} calls/s" );
        }
    }
}

// [<endsnippet body>]