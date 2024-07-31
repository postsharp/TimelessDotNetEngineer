// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace ModernSingleton;

public class RogueClass
{
    private PerformanceCounterManager _manager =
        new PerformanceCounterManager( new AwsPerformanceCounterUploader() );
}