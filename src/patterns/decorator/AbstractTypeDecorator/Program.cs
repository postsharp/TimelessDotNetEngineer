// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.DependencyInjection;

Console.WriteLine( "======= Explicit creation ==============" );

// [<snippet TypeDecoratorUsage>]
var originalMessenger = new Messenger();

var retryingMessenger = new MessengerDecorator(
    originalMessenger,
    new RetryPolicy() );

var exceptionReportingService = new ExceptionReportingService();

var retryingExceptionReportingMessenger =
    new MessengerDecorator(
        retryingMessenger,
        new ReportExceptionPolicy( exceptionReportingService ) );

retryingExceptionReportingMessenger.Send( new Message( "Hello!" ) );

// [<endsnippet TypeDecoratorUsage>]

Console.WriteLine( "======= Scrutor ==============" );

// [<snippet TypeDecoratorScrutor>]
var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<IMessenger, Messenger>()
    .Decorate<IMessenger>(
        ( inner, _ ) => new MessengerDecorator(
            inner,
            new RetryPolicy() ) )
    .Decorate<IMessenger>(
        ( inner, serviceProvider ) => new MessengerDecorator(
            inner,
            new ReportExceptionPolicy(
                serviceProvider.GetRequiredService<IExceptionReportingService>() ) ) )
    .BuildServiceProvider();

var client = services.GetRequiredService<Client>();
client.Greet();

// [<endsnippet TypeDecoratorScrutor>]