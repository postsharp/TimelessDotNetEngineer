// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

internal sealed class AwsPerformanceCounterUploader : IPerformanceCounterUploader
{
    public void UploadCounter( string name, double value )
    {
        Console.WriteLine( $"Uploading.... {name} -> {value}" );
    }
}