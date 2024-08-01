// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public interface IPerformanceCounterManager
{
    void IncrementCounter( string name );

    void UploadAndReset();
}