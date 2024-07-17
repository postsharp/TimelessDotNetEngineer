// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#define  SERILOG_REQUEST_LOGGING
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

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
#elif SERILOG_REQUEST_LOGGING
// [<snippet RequestLogging>
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();
// [<endsnippet RequestLogging>
#elif SCOPE_CONFIGURATION 

// [<snippet ScopeConfiguration>]
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}")
// [<endsnippet ScopeConfiguration>]    
    .MinimumLevel.Verbose()
    .MinimumLevel.Override( "Microsoft.AspNetCore.Hosting", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.AspNetCore.Mvc", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.AspNetCore.Routing", LogEventLevel.Warning )
    .CreateLogger();

builder.Services.AddSerilog();
#endif



var app = builder.Build();

#if SERILOG_REQUEST_LOGGING
// [<snippet RequestLogging>]
app.UseSerilogRequestLogging();
// [<endsnippet RequestLogging>]
#endif







// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
