using MetalamaMethodDecorator;
using Microsoft.Extensions.DependencyInjection;
using Services;

var collection = new ServiceCollection();
collection.AddSingleton<IExceptionReportingService, ExceptionReportingService>();
collection.AddSingleton<Messenger>();
var services = collection.BuildServiceProvider();

var messenger = services.GetRequiredService<Messenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");