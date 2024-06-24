// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using OutdoorTodoList.ApiService.Services;
using System.Globalization;

internal static class TodoListEndpointsExtensions
{
    public static WebApplication MapTodoListEndpoints( this WebApplication app )
    {
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

        return app;
    }
}