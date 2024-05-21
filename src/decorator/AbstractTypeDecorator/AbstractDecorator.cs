// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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