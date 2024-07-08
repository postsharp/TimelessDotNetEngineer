using Metalama.Patterns.Caching.Backends.Redis;
using Metalama.Patterns.Caching.Building;
using StackExchange.Redis;
using System.Globalization;
using TodoList.ApiService.Model;
using TodoList.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// [<snippet AddRedis>]
builder.AddRedisClient( "cache" );
// [<endsnippet AddRedis>]

// [<snippet AddMetalamaCaching>]
builder.Services.AddMetalamaCaching( 
    caching => caching.WithBackend( backend => backend.Redis(  )) );
// [<endsnippet AddMetalamaCaching>]

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

app.MapGet( "/todo/{id}", ( TodoService todos, int id, CancellationToken cancellationToken )
    => todos.GetTodoAsync( id, cancellationToken ) );

app.MapPost( "/todo", async ( TodoService todos, Todo todo, CancellationToken cancellationToken ) =>
{
    var newTodo = await todos.AddTodoAsync( todo, cancellationToken );
    return Results.Created( $"/todo/{newTodo.Id}", newTodo );
} );

app.MapPut( "/todo/{id}", async ( TodoService todos, int id, Todo todo, CancellationToken cancellationToken ) =>
{
    if ( id != todo.Id )
    {
        return Results.BadRequest( "The ID in the URL does not match the ID in the request body." );
    }

    if ( await todos.UpdateTodoAsync( todo, cancellationToken ) )
    {
        return Results.NoContent();

    }
    else
    {
        return Results.NotFound();
    }
} );

app.MapDelete( "/todo/{id}", async ( TodoService todos, int id, CancellationToken cancellationToken ) =>
{
    if ( await todos.DeleteTodoAsync( id, cancellationToken ) )
    {
        return Results.NoContent();

    }
    else
    {
        return Results.NotFound();
    }
} );

app.Run();
