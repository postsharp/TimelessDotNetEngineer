public interface IRetryHandler
{
    void Retry(Action action, int? attempts = default, int? delay = default);

    T Retry<T>(Func<T> action, int? attempts = default, int? delay = default);
}