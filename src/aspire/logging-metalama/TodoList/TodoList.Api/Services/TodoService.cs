using Microsoft.EntityFrameworkCore;
using TodoList.ApiService.Model;

namespace TodoList.ApiService.Services;

public partial class TodoService( ApplicationDbContext db )
{
    public async Task<IEnumerable<Todo>> GetTodosAsync( CancellationToken cancellationToken = default )
        => await db.Todos.ToArrayAsync( cancellationToken );

    // <snippet> LogUsingAttribute
    [Log]
    public async Task<Todo?> GetTodoAsync( int id, CancellationToken cancellationToken = default )
        => await db.Todos.FindAsync( id );
    // <endsnippet> LogUsingAttribute

    public async Task<Todo> AddTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var newEntry = db.Todos.Add( todo );
        await db.SaveChangesAsync( cancellationToken );

        return newEntry.Entity;
    }

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

        return true;
    }

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
}