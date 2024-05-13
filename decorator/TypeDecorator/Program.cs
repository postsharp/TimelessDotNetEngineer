using Microsoft.Extensions.DependencyInjection;
using Services;
using TypeDecorator;

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