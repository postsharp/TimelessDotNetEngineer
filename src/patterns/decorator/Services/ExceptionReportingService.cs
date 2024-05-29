// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public class ExceptionReportingService : IExceptionReportingService
{
    public void ReportException( string v, Exception e )
    {
        // Simulate reporting exception
        Console.WriteLine( $"{v}: {e.Message}" );
    }
}