using Metalama.Patterns.Caching;
using Metalama.Patterns.Caching.Aspects;
using Microsoft.EntityFrameworkCore;
using TodoList.ApiService.Model;

namespace TodoList.ApiService.Services;

public partial class TodoService( ApplicationDbContext db )
{
    // [<snippet Caching>]
    [Cache]
    public async Task<IEnumerable<Todo>> GetTodosAsync( [NotCacheKey] CancellationToken cancellationToken = default )
        => await db.Todos.ToListAsync();

    [Cache]
    public async Task<Todo?> GetTodoAsync( int id, [NotCacheKey] CancellationToken cancellationToken = default )
        => await db.Todos.FindAsync( id );
    // [<endsnippet Caching>]

    // [<snippet ImperativeInvalidation>]
    [InvalidateCache( nameof( this.GetTodosAsync ) )]
    public async Task<Todo> AddTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var newEntry = db.Todos.Add( todo );
        await db.SaveChangesAsync( cancellationToken );

        // Invalidate the cache for GetTodoAsync imperatively, as we don't know the new entry's ID upfront.
        await this._cachingService.InvalidateAsync( this.GetTodoAsync, newEntry.Entity.Id, cancellationToken );

        return newEntry.Entity;
    }
    // [<endsnippet ImperativeInvalidation>]

    [InvalidateCache( nameof( this.GetTodosAsync ), nameof( this.GetTodoAsync ) )]
    public async Task<bool> UpdateTodoAsync( int id, Todo todo, CancellationToken cancellationToken = default )
    {
        var existingTodo = await this.GetTodoAsync( id, cancellationToken );

        if ( existingTodo is null )
        {
            return false;
        }

        existingTodo.IsCompleted = todo.IsCompleted;
        existingTodo.Title = todo.Title;
        db.Todos.Update( existingTodo );
        await db.SaveChangesAsync( cancellationToken );

        return true;
    }

    // [<snippet DeclarativeInvalidation>]
    [InvalidateCache( nameof( this.GetTodosAsync ), nameof( this.GetTodoAsync ) )]
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