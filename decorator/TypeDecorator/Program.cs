using Microsoft.Extensions.DependencyInjection;
using Services;
using TypeDecorator;

// [<snippet TypeDecoratorUsage>]
var originalMessenger = new Messenger();
var retryingMessenger = new RetryingMessenger(originalMessenger);
var retryingExceptionReportingMessenger = new ExceptionReportingMessenger(retryingMessenger, new ExceptionReportingService());

retryingExceptionReportingMessenger.Send(new Message("Hello!"));
// [<endsnippet TypeDecoratorUsage>]

// [<snippet TypeDecoratorScrutor>]
var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<IMessenger, Messenger>()
    .Decorate<IMessenger, RetryingMessenger>()
    .Decorate<IMessenger, ExceptionReportingMessenger>()
    .BuildServiceProvider();

var messenger = services.GetRequiredService<IMessenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");
// [<endsnippet TypeDecoratorScrutor>]