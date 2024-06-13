// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

// [<endsnippet body>]

public interface IPerformanceCounterManager
{
    void IncrementCounter( string name );
    
    void UploadAndReset();
}