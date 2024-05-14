using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

// [<snippet ProxyGenerator>]
var proxyGenerator = new ProxyGenerator();
// [<endsnippet ProxyGenerator>]

Console.WriteLine("======= Explicit creation ==============");
// [<snippet TypeDecoratorUsage>]
var originalMessenger = new Messenger();
var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<IMessenger>(
    originalMessenger,
    new RetryInterceptor(),
    new ReportExceptionInterceptor(new ExceptionReportingService()));

proxy.Send(new Message("Hello!"));
// [<endsnippet TypeDecoratorUsage>]


Console.WriteLine("======= Scrutor ==============");
// [<snippet TypeDecoratorScrutor>]
var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<IMessenger, Messenger>()
    .Decorate<IMessenger>(
        (inner, serviceProvider) => proxyGenerator.CreateInterfaceProxyWithTarget(
            inner, new RetryInterceptor(),
            new ReportExceptionInterceptor(serviceProvider.GetRequiredService<IExceptionReportingService>())))
    .BuildServiceProvider();

var messenger = services.GetRequiredService<IMessenger>();
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");
// [<endsnippet TypeDecoratorScrutor>]