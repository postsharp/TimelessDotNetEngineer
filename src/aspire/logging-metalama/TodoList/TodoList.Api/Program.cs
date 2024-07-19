using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoList.ApiService.Model;
using TodoList.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.

// Keep the database connection open for the lifetime of the application, so the in-memory database is not lost after each operation.
// https://github.com/dotnet/efcore/issues/9842#issuecomment-1634427346
using var dbConnection = new SqliteConnection( "Data Source=:memory:" );
await dbConnection.OpenAsync();
builder.Services.AddDbContext<ApplicationDbContext>( dbOptions => dbOptions.UseSqlite( dbConnection ) );

builder.Services.AddScoped<TodoService>();

var app = builder.Build();

using ( var scope = app.Services.CreateScope() )
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database;
    db.EnsureCreated();
}

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
