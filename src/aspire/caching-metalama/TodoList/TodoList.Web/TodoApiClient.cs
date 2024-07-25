// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace TodoList.Web;

public class TodoApiClient( HttpClient httpClient )
{
    public Task<Todo[]?> GetTodosAsync( CancellationToken cancellationToken = default )
        => httpClient.GetFromJsonAsync<Todo[]>( "/todo", cancellationToken );

    public Task<Todo?> GetTodoAsync( int id, CancellationToken cancellationToken = default )
        => httpClient.GetFromJsonAsync<Todo>( $"/todo/{id}", cancellationToken );

    public async Task CreateTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var result = await httpClient.PostAsJsonAsync( "/todo", todo, cancellationToken );
        result.EnsureSuccessStatusCode();
    }

    public async Task UpdateTodoAsync( Todo todo, CancellationToken cancellationToken = default )
    {
        var result = await httpClient.PutAsJsonAsync( $"/todo/{todo.Id}", todo, cancellationToken );
        result.EnsureSuccessStatusCode();
    }

    public async Task DeleteTodoAsync( int id, CancellationToken cancellationToken = default )
    {
        var result = await httpClient.DeleteAsync( $"/todo/{id}", cancellationToken );
        result.EnsureSuccessStatusCode();
    }
}