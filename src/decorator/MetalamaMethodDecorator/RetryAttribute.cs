using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

internal class RetryAttribute : OverrideMethodAspect
{
    public int Attempts { get; set; } = 3;

    public double Delay { get; set; } = 1000;

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
                    $"Method {meta.Target.Method.DeclaringType.Name}.{meta.Target.Method} has failed " +
                    $" on {e.GetType().Name}. Retrying in {delay / 1000} seconds... ({i + 1}/{Attempts})");

                Thread.Sleep((int)delay);
            }
        }
    }
    
    // TODO: Implement OverrideMethodAsync and call Task.Delay instead of Thread.Sleep.
}