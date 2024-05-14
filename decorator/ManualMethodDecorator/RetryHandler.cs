namespace ManualMethodDecorator;

public class RetryHandler : IRetryHandler
{
    protected const int _defaultAttempts = 3;
    protected const int _defaultDelay = 1000;

    public void Retry(Action action, int? attempts = default, int? delay = default)
    {
        attempts ??= _defaultAttempts;
        delay ??= _defaultDelay;

        for (var i = 0;; i++)
        {
            try
            {
                action();
                return;
            }
            catch (Exception) when (i < attempts)
            {
                var backoffDelay = delay * Math.Pow(2, i);

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {backoffDelay / 1000} seconds... ({i + 1}/{attempts})");
                Thread.Sleep((int)backoffDelay);
            }
        }
    }

    public T Retry<T>(Func<T> action, int? attempts = default, int? delay = default)
    {
        attempts ??= _defaultAttempts;
        delay ??= _defaultDelay;

        for (var i = 0;; i++)
        {
            try
            {
                return action();
            }
            catch (Exception) when (i < attempts)
            {
                var backoffDelay = delay * Math.Pow(2, i);

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {backoffDelay / 1000} seconds... ({i + 1}/{attempts})");
                Thread.Sleep((int)backoffDelay);
            }
        }
    }
}