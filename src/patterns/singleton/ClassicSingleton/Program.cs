// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

PerformanceCounterManager.Instance.IncrementCounter("some counter");
PerformanceCounterManager.Instance.IncrementCounter("another counter");
PerformanceCounterManager.Instance.IncrementCounter("some counter");

PerformanceCounterManager.Instance.PrintAndReset();