using Metalama.Framework.Aspects;

namespace PollyMetalama;

[CompileTime]
public class RetryOnDbExceptionAttribute : RetryAttribute
{
    public RetryOnDbExceptionAttribute() : base(StrategyKind.RetryOnDbException)
    {
    }
}