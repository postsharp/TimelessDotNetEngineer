// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal class LoggingService : ILoggingService
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}