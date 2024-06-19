using System.Globalization;
using TodoList.ApiService.Extensions;
using TodoList.ApiService.Model;
using TodoList.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// [<snippet CacheConfiguration>]
// Add Metalama Caching with Redis.
builder.AddDistributedMetalamaCaching( "cache", "todolist-api" );
// [<endsnippet CacheConfiguration>]

// Add services to the container.
builder.AddSqlServerDbContext<ApplicationDbContext>( "database" );
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Ensure the schema is created.
using ( var scope = app.Services.CreateScope() )
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapGet( "/todo", ( TodoService todos, CancellationToken cancellationToken )
    => todos.GetTodosAsync( cancellationToken ) );

app.MapGet( "/todo/{id}", ( TodoService todos, string id, CancellationToken cancellationToken )
    => todos.GetTodoAsync( int.Parse( id, CultureInfo.InvariantCulture ), cancellationToken ) );

app.MapPost( "/todo", async ( TodoService todos, Todo todo, CancellationToken cancellationToken ) =>
{
    var newTodo = await todos.AddTodoAsync( todo, cancellationToken );
    return Results.Created( $"/todo/{newTodo.Id}", newTodo );
} );

app.MapPut( "/todo/{id}", async ( TodoService todos, string id, Todo todo, CancellationToken cancellationToken ) =>
{
    if ( await todos.UpdateTodoAsync( int.Parse( id, CultureInfo.InvariantCulture ), todo, cancellationToken ) )
    {
        return Results.NoContent();

    }
    else
    {
        return Results.NotFound();
    }
} );

app.MapDelete( "/todo/{id}", async ( TodoService todos, string id, CancellationToken cancellationToken ) =>
{
    if ( await todos.DeleteTodoAsync( int.Parse( id, CultureInfo.InvariantCulture ), cancellationToken ) )
    {
        return Results.NoContent();

    }
    else
    {
        return Results.NotFound();
    }
} );

app.Run();
