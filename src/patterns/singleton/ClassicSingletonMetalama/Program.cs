// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using ClassicSingletonMetalama;

PerformanceCounterManager.Instance.IncrementCounter( "some counter" );
PerformanceCounterManager.Instance.IncrementCounter( "another counter" );
PerformanceCounterManager.Instance.IncrementCounter( "some counter" );

PerformanceCounterManager.Instance.PrintAndReset();