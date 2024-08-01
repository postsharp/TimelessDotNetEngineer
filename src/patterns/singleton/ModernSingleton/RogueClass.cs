// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace ModernSingleton;

public class RogueClass
{
    public PerformanceCounterManager Manager { get;  } = new PerformanceCounterManager( new AwsPerformanceCounterUploader() );
}