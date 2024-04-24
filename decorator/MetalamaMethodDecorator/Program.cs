using MetalamaMethodDecorator;
using Microsoft.Extensions.DependencyInjection;
using Services;

var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<Messenger>()
    .BuildServiceProvider();

var messenger = services.GetRequiredService<Messenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");