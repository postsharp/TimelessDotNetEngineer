// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public abstract class AbstractDecorator
{
    private readonly Func<Func<object?>, object?> _policy;

    protected AbstractDecorator( Func<Func<object?>, object?> policy )
    {
        this._policy = policy;
    }

    protected void Invoke( Action action )
    {
        this._policy(
            () =>
            {
                action();

                return null!;
            } );
    }

    protected T Invoke<T>( Func<T> func )
    {
        return (T) this._policy( () => func() )!;
    }
}