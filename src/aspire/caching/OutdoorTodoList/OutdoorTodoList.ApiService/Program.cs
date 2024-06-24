using OutdoorTodoList.ApiService.Interceptors;
using OutdoorTodoList.ApiService.Model;
using OutdoorTodoList.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache( "cache" );
builder.AddDistributedMetalamaCaching( "cache", "todolist-api" );

// Add services to the container.
builder.Services.AddProblemDetails();

var sqlSlowDown = new SqlSlowDownInterceptor();

builder.AddSqlServerDbContext<ApplicationDbContext>(
    "database",
    // Slow down the database to show the effect of caching.
    configureDbContextOptions: options => options.AddInterceptors( sqlSlowDown ) );

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

// Ensure the schema is created.
using ( var scope = app.Services.CreateScope() )
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
    sqlSlowDown.Enabled = true;
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseOutputCache();

// Map endpoints.
app.MapDefaultEndpoints();
app.MapWeatherForecastEndpoints();
app.MapTodoListEndpoints();

// Run.
app.Run();
