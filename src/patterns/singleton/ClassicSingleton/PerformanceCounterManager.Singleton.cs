// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public partial class PerformanceCounterManager
{
    private PerformanceCounterManager() { }

    public static PerformanceCounterManager Instance { get; } = new();
}