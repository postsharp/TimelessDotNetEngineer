// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Serilog;
using Serilog.Events;
using SerilogEnrichedWebApp;

var builder = WebApplication.CreateBuilder( args );

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override( "Microsoft", LogEventLevel.Information )
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllersWithViews();
// [<snippet enriched-handler-registration>]
builder.Services.AddExceptionHandler<EnchrichedExceptionLoggingHandler>();
// [<endsnippet enriched-handler-registration>]
builder.Services.AddSingleton<DieRoller>();
builder.Services.AddSerilog();
// [<snippet add-http-logging>]
builder.Services.AddHttpLogging( _ => { } );
// [<endsnippet add-http-logging>]

var app = builder.Build();

// Configure the HTTP request pipeline.
//if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler( "/Home/Error" );
}

// [<snippet use-http-logging>]
app.UseHttpLogging();
// [<endsnippet use-http-logging>]

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DieRoller}/{action=Index}" );

app.Run();