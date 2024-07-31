// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public interface IPerformanceCounterUploader
{
    void UploadCounter( string name, double value );
}