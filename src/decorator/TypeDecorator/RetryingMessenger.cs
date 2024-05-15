public class RetryingMessenger : MessengerDecorator
{
    private readonly int _retryAttempts;
    private readonly int _retryDelay;

    public RetryingMessenger(IMessenger underlying, int retryAttempts = 3, int retryDelay = 1000) : base(underlying)
    {
        _retryAttempts = retryAttempts;
        _retryDelay = retryDelay;
    }

    public override void Send(Message message)
    {
        for (var i = 0;; i++)
        {
            try
            {
                Underlying.Send(message);
                return;
            }
            catch (Exception) when (i < _retryAttempts)
            {
                var delay = _retryDelay * Math.Pow(2, i);

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{_retryAttempts})");
                Thread.Sleep((int)delay);
            }
        }
    }

    public override Message Receive()
    {
        for (var i = 0;; i++)
        {
            try
            {
                return Underlying.Receive();
            }
            catch (Exception) when (i < _retryAttempts)
            {
                var delay = _retryDelay * Math.Pow(2, i);

                Console.WriteLine(
                    $"Failed to receive message. Retrying in {delay / 1000} seconds... ({i + 1}/{_retryAttempts})");
                Thread.Sleep((int)delay);
            }
        }
    }
}