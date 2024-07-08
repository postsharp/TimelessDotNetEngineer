using Metalama.Patterns.Caching;
using Metalama.Patterns.Caching.Aspects;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TodoList.ApiService.Model;

namespace TodoList.ApiService.Services;

public partial class TodoService( ApplicationDbContext db )
{
    // [<snippet Caching>]
    [Cache]
    public async Task<IEnumerable<Todo>> GetTodosAsync( [NotCacheKey] CancellationToken cancellationToken = default )
        => await db.Todos.ToListAsync( cancellationToken );

    [Cache]
    public async Task<Todo?> GetTodoAsync( int id, [NotCacheKey] CancellationToken cancellationToken = default )
        => await db.Todos.FindAsync( id );
    // [<endsnippet Caching>]

    [InvalidateCache( nameof(this.GetTodosAsync) )]
    public async Task<Todo> AddTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var newEntry = db.Todos.Add( todo );
        await db.SaveChangesAsync( cancellationToken );


        await this._cachingService.InvalidateAsync( this.GetTodoAsync, newEntry.Entity.Id, cancellationToken,
            cancellationToken );

        return newEntry.Entity;
    }


    // [<snippet ImperativeInvalidation>]
    public async Task<bool> UpdateTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var existingTodo = await this.GetTodoAsync( todo.Id, cancellationToken );

        if ( existingTodo is null )
        {
            return false;
        }

        existingTodo.IsCompleted = todo.IsCompleted;
        existingTodo.Title = todo.Title;
        db.Todos.Update( existingTodo );
        await db.SaveChangesAsync( cancellationToken );

        // Invalidate the cache for GetTodoAsync imperatively, as the todo.Id is not directly visible from the parameter.
        await this._cachingService.InvalidateAsync( this.GetTodoAsync, todo.Id, cancellationToken, cancellationToken );
        await this._cachingService.InvalidateAsync( this.GetTodosAsync, cancellationToken, cancellationToken );

        return true;
    }
    // [<endsnippet ImperativeInvalidation>]

    // [<snippet DeclarativeInvalidation>]
    [InvalidateCache( nameof(this.GetTodosAsync), nameof(this.GetTodoAsync) )]
    public async Task<bool> DeleteTodoAsync( int id, CancellationToken cancellationToken = default )
    {
        var todo = await this.GetTodoAsync( id, cancellationToken );

        if ( todo is null )
        {
            return false;
        }

        db.Todos.Remove( todo );
        await db.SaveChangesAsync( cancellationToken );

        return true;
    }
    // [<endsnippet DeclarativeInvalidation>]
}