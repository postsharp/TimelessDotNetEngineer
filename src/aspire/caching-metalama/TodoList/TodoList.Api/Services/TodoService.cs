using Metalama.Patterns.Caching;
using Metalama.Patterns.Caching.Aspects;
using TodoList.ApiService.Model;

namespace TodoList.ApiService.Services;

public partial class TodoService(ApplicationDbContext db)
{
    [Cache]
    public Task<Todo[]> GetTodosAsync([NotCacheKey] CancellationToken cancellationToken = default)
        => Task.FromResult(db.Todos.ToArray());

    [Cache]
    public Task<Todo?> GetTodoAsync(int id, [NotCacheKey] CancellationToken cancellationToken = default)
        => Task.FromResult(db.Todos.Find(id));

    [InvalidateCache(nameof(this.GetTodosAsync))]
    public async Task<Todo> AddTodoAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var newEntry = db.Todos.Add(todo);
        await db.SaveChangesAsync(cancellationToken);

        // Invalidate the cache for GetTodoAsync imperatively, as we don't know the new entry's ID upfront.
        await this._cachingService.InvalidateAsync(this.GetTodoAsync, newEntry.Entity.Id, cancellationToken);

        return newEntry.Entity;
    }

    [InvalidateCache(nameof(this.GetTodosAsync), nameof(this.GetTodoAsync))]
    public async Task<bool> UpdateTodoAsync(int id, Todo todo, CancellationToken cancellationToken = default)
    {
        var existingTodo = await GetTodoAsync(id, cancellationToken);

        if (existingTodo is null)
        {
            return false;
        }

        existingTodo.IsCompleted = todo.IsCompleted;
        existingTodo.Title = todo.Title;
        db.Todos.Update(existingTodo);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    [InvalidateCache(nameof(this.GetTodosAsync), nameof(this.GetTodoAsync))]
    public async Task<bool> DeleteTodoAsync(int id, CancellationToken cancellationToken = default)
    {
        var todo = await GetTodoAsync(id, cancellationToken);

        if (todo is null)
        {
            return false;
        }

        db.Todos.Remove(todo);
        await db.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
