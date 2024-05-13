using Metalama.Framework.Aspects;
using MetalamaMethodDecorator;

[assembly: AspectOrder(typeof(ReportExceptionsAttribute), typeof(RetryAttribute))]