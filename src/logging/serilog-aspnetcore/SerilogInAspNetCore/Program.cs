// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

#define SCOPE_CONFIGURATION

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SerilogInAspNetCore;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#if BASIC_CONFIGURATION
// [<snippet AddSerilog>]
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSerilog();

// [<endsnippet AddSerilog>]

var app = builder.Build();

#elif SERILOG_REQUEST_LOGGING
// [<snippet RequestLogging>]
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override( "Microsoft.AspNetCore", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.Extensions.Hosting", LogEventLevel.Information )
    .MinimumLevel.Override( "Microsoft.Hosting", LogEventLevel.Information )
    .CreateLogger();

builder.Services.AddSerilog();
var app = builder.Build();
app.UseSerilogRequestLogging();

// [<endsnippet RequestLogging>]
#elif SCOPE_CONFIGURATION

// [<snippet ScopeConfiguration>]
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}" )

// [<endsnippet ScopeConfiguration>]    
    .MinimumLevel.Verbose()
    .MinimumLevel.Override( "Microsoft.AspNetCore", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.Extensions.Hosting", LogEventLevel.Information )
    .MinimumLevel.Override( "Microsoft.Hosting", LogEventLevel.Information )
    .CreateLogger();

builder.Services.AddSerilog();

// [<snippet AddMiddleware1>]
builder.Services.AddSingleton<PushPropertiesMiddleware>();

// [<endsnippet AddMiddleware1>]

var app = builder.Build();
app.UseSerilogRequestLogging();

// [<snippet AddMiddleware2>]
app.UseMiddleware<PushPropertiesMiddleware>();

// [<endsnippet AddMiddleware2>]

#endif

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();