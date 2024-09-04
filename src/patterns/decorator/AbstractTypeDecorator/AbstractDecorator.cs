// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public abstract class AbstractDecorator( IPolicy policy )
{
    protected T Invoke<T>( Func<T> func ) => policy.Invoke( func );

    protected void Invoke( Action action )
        => policy.Invoke<object?>(
            () =>
            {
                action();

                return null!;
            } );
}