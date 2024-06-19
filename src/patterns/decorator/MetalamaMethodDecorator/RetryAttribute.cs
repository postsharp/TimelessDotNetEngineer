// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

internal class RetryAttribute : OverrideMethodAspect
{
    public int Attempts { get; set; } = 3;

    public double Delay { get; set; } = 1000;

    public override dynamic? OverrideMethod()
    {
        for ( var i = 0;; i++ )
        {
            try
            {
                return meta.Proceed();
            }
            catch ( Exception e ) when ( i < this.Attempts )
            {
                var delay = this.Delay * Math.Pow( 2, i + 1 );

                Console.WriteLine(
                    $"Method {meta.Target.Method.DeclaringType.Name}.{meta.Target.Method} has failed " +
                    $" on {e.GetType().Name}. Retrying in {delay / 1000} seconds... ({i + 1}/{this.Attempts})" );

                Thread.Sleep( (int) delay );
            }
        }
    }

    // TODO: Implement OverrideMethodAsync and call Task.Delay instead of Thread.Sleep.
}