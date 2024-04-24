using Microsoft.Extensions.DependencyInjection;
using Services;
using TypeDecorator;

var collection = new ServiceCollection();
collection.AddSingleton<IExceptionReportingService>(new ExceptionReportingService());
collection.AddSingleton<IMessenger>(new Messenger());
collection.Decorate<IMessenger, RetryingMessenger>();
collection.Decorate<IMessenger, ExceptionReportingMessenger>();
var services = collection.BuildServiceProvider();

var messenger = services.GetRequiredService<IMessenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");