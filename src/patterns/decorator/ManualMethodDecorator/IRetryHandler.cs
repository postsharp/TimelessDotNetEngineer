// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public interface IRetryHandler
{
    void Retry( Action action, int? attempts = default, int? delay = default );

    T Retry<T>( Func<T> action, int? attempts = default, int? delay = default );
}