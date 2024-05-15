// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public interface IExceptionHandler
{
    void ReportWhenFails( Action action, string message );

    T ReportWhenFails<T>( Func<T> action, string message );
}