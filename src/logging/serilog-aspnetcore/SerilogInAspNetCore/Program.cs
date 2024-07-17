// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [<snippet AddSerilog>]
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSerilog();
// [<endsnippet AddSerilog>]

var app = builder.Build();

// [<snippet RequestLogging>]
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override( "Microsoft.AspNetCore.Hosting", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.AspNetCore.Mvc", LogEventLevel.Warning )
    .MinimumLevel.Override( "Microsoft.AspNetCore.Routing", LogEventLevel.Warning )
    .CreateLogger();

app.UseSerilogRequestLogging();
// [<endsnippet RequestLogging>]

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
