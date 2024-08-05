// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using OutdoorTodoList.Web;
using OutdoorTodoList.Web.Components;

var builder = WebApplication.CreateBuilder( args );

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache( "cache" );
builder.AddRedisDistributedCache( "cache" );

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherApiClient>(
    client => client.BaseAddress = new Uri( "https+http://apiservice" ) );

builder.Services.AddHttpClient<TodoApiClient>(
    client => client.BaseAddress = new Uri( "https+http://apiservice" ) );

var app = builder.Build();

if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();