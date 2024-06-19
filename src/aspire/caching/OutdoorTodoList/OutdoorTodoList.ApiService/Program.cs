using OutdoorTodoList.ApiService.Model;
using OutdoorTodoList.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddDistributedMetalamaCaching( "cache", "todolist-api" );

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddSqlServerDbContext<ApplicationDbContext>( "database" );
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

// Ensure the schema is created.
using ( var scope = app.Services.CreateScope() )
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapWeatherForecastEndpoints();
app.MapTodoListEndpoints();

app.Run();
