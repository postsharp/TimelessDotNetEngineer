public abstract class AbstractDecorator
{
    private readonly Func<Func<object?>, object?> _policy;

    protected AbstractDecorator(Func<Func<object?>, object?> policy)
    {
        _policy = policy;
    }


    protected void Invoke(Action action)
    {
        _policy(() =>
        {
            action();
            return null!;
        });
    }

    protected T Invoke<T>(Func<T> func)
    {
        return (T)_policy(() => func())!;
    }
}