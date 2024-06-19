// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal sealed class AwsPerformanceCounterUploader : IPerformanceCounterUploader
{
    public void UploadCounter( string name, double value )
    {
        Console.WriteLine($"Uploading.... {name} -> {value}");
    }
}