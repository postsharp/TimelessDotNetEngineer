// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// [<snippet cs-configuration>]
var acmeLogLevelSwitch = new LoggingLevelSwitch();
acmeLogLevelSwitch.MinimumLevel = LogEventLevel.Information;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override( "Microsoft", LogEventLevel.Warning )
    .MinimumLevel.Override( "Acme", acmeLogLevelSwitch )
    .WriteTo.File( "log.txt" )
    .WriteTo.Console( restrictedToMinimumLevel: LogEventLevel.Information )
    .CreateLogger();
// [<endsnippet cs-configuration>]



// [<snippet json-configuration>]
var configuration = new ConfigurationBuilder()
    .AddJsonFile( "appsettings.json" )
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration( configuration )
    .CreateLogger();
// [<endsnippet json-configuration>]

var services = new ServiceCollection()
    .AddLogging()
    .AddSerilog()
    .AddSingleton<DieRoller>();

using var serviceProvider = services.BuildServiceProvider();

var dieRoller = serviceProvider.GetRequiredService<DieRoller>();

Console.WriteLine(dieRoller.Roll());
