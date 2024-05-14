using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace MetalamaMethodDecorator;

internal class RetryAttribute : OverrideMethodAspect
{
    public int Attempts { get; set; } = 3;

    public double Delay { get; set; } = 1000;

    // Template for non-async methods.
    public override dynamic? OverrideMethod()
    {
        for (var i = 0;; i++)
        {
            try
            {
                return meta.Proceed();
            }
            catch (Exception e) when (i < Attempts)
            {
                var delay = Delay * Math.Pow(2, i + 1);

                Console.WriteLine(
                    $"Method {meta.Target.Method.DeclaringType.Name}.{meta.Target.Method} has failed on {e.GetType().Name}. Retrying in {delay / 1000} seconds... ({i + 1}/{Attempts})");

                Thread.Sleep((int)delay);
            }
        }
    }

    // Template for async methods.
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var cancellationTokenParameter
            = meta.Target.Parameters.LastOrDefault(p => p.Type.Is(typeof(CancellationToken)));

        for (var i = 0;; i++)
        {
            try
            {
                return await meta.ProceedAsync();
            }
            catch (Exception e) when (i < Attempts)
            {
                var delay = Delay * Math.Pow(2, i + 1);

                Console.WriteLine(
                    $"Method {meta.Target.Method.DeclaringType.Name}.{meta.Target.Method} has failed on {e.GetType().Name}. Retrying in {delay / 1000} seconds... ({i + 1}/{Attempts})");

                if (cancellationTokenParameter != null)
                {
                    await Task.Delay((int)delay, cancellationTokenParameter.Value);
                }
                else
                {
                    await Task.Delay((int)delay);
                }
            }
        }
    }
}