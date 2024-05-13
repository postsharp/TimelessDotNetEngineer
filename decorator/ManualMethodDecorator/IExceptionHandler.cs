namespace ManualMethodDecorator;

public interface IExceptionHandler
{
    void ReportWhenFails(Action action, string message);

    T ReportWhenFails<T>(Func<T> action, string message);
}
