using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("======= Explicit creation ==============");
// [<snippet TypeDecoratorUsage>]
var originalMessenger = new Messenger();
var retryingMessenger = new MessengerDecorator(
    originalMessenger,
    func => Policies.Retry(func));

var exceptionReportingService = new ExceptionReportingService();

var retryingExceptionReportingMessenger =
    new MessengerDecorator(
        retryingMessenger,
        func => Policies.ReportException(func, exceptionReportingService));

retryingExceptionReportingMessenger.Send(new Message("Hello!"));
// [<endsnippet TypeDecoratorUsage>]


Console.WriteLine("======= Scrutor ==============");
// [<snippet TypeDecoratorScrutor>]
var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<IMessenger, Messenger>()
    .Decorate<IMessenger>(
        (inner, _) => new MessengerDecorator(
            inner,
            func => Policies.Retry(func)))
    .Decorate<IMessenger>(
        (inner, serviceProvider) => new MessengerDecorator(
            inner,
            func => Policies.ReportException(
                func,
                serviceProvider.GetRequiredService<IExceptionReportingService>())))
    .BuildServiceProvider();

var messenger = services.GetRequiredService<IMessenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");
// [<endsnippet TypeDecoratorScrutor>]