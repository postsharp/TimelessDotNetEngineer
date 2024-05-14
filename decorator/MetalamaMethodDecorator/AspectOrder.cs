using Metalama.Framework.Aspects;

[assembly: AspectOrder(typeof(ReportExceptionsAttribute), typeof(RetryAttribute))]