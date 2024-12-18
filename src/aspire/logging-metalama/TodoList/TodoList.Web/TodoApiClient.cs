// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace TodoList.Web;

public partial class TodoApiClient( HttpClient httpClient )
{
    public async Task<Todo[]?> GetTodosAsync( CancellationToken cancellationToken = default )
        => await httpClient.GetFromJsonAsync<Todo[]>( "/todo", cancellationToken );

    public async Task<Todo?> GetTodoAsync( int id, CancellationToken cancellationToken = default )
        => await httpClient.GetFromJsonAsync<Todo>( $"/todo/{id}", cancellationToken );

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