// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.DependencyInjection;


// [<snippet TypeDecoratorUsage>]
var originalMessenger = new Messenger();

var retryingMessenger = new ExceptionReportingMessenger(
    new RetryingMessenger( originalMessenger ),
    new ExceptionReportingService() );

var clientUsingDecorator = new Client( retryingMessenger );
clientUsingDecorator.Greet();
// [<endsnippet TypeDecoratorUsage>]

// [<snippet TypeDecoratorScrutor>]
var services = new ServiceCollection()
    .AddSingleton<IExceptionReportingService, ExceptionReportingService>()
    .AddSingleton<IMessenger, Messenger>()
    .AddSingleton<Client>()
    .Decorate<IMessenger, RetryingMessenger>()
    .Decorate<IMessenger, ExceptionReportingMessenger>()
    .BuildServiceProvider();

var client = services.GetRequiredService<Client>();
client.Greet();
// [<endsnippet TypeDecoratorScrutor>]