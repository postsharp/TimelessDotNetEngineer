using System.Data.Common;
using Microsoft.Data.Sqlite;
using Polly.Registry;
internal class Accounts
{
  private readonly DbConnection _connection;
  [Retry]
  public async Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default)
  {
    var pipeline = _resiliencePipelineProvider.GetPipeline("default");
    return (IReadOnlyList<Account>)await pipeline.ExecuteAsync(Invoke);
    async ValueTask<object?> Invoke(CancellationToken cancellationToken_1 = default)
    {
      return await this.ListAsync_Source(cancellationToken);
    }
  }
  private async Task<IReadOnlyList<Account>> ListAsync_Source(CancellationToken cancellationToken = default)
  {
    var list = new List<Account>();
    await using var command = _connection.CreateCommand();
    command.CommandText = "SELECT id, name, balance FROM accounts";
    await using var reader = await command.ExecuteReaderAsync(cancellationToken);
    while (await reader.ReadAsync(cancellationToken))
    {
      list.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
    }
    return list;
  }
  [Retry]
  [DbTransaction]
  public async Task TransferAsync(int sourceAccountId, int targetAccountId, int amount, CancellationToken cancellationToken = default)
  {
    var pipeline = _resiliencePipelineProvider.GetPipeline("default");
    await pipeline.ExecuteAsync(Invoke);
    return;
    async ValueTask<object?> Invoke(CancellationToken cancellationToken_1 = default)
    {
      await this.TransferAsync_DbTransaction(sourceAccountId, targetAccountId, amount, cancellationToken);
      return default;
    }
  }
  private async Task TransferAsync_Source(int sourceAccountId, int targetAccountId, int amount, CancellationToken cancellationToken = default)
  {
    await using (var command = _connection.CreateCommand())
    {
      command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
      command.Parameters.Add(new SqliteParameter("$id", sourceAccountId));
      command.Parameters.Add(new SqliteParameter("$amount", amount));
      await command.ExecuteNonQueryAsync(cancellationToken);
    }
    await using (var command = _connection.CreateCommand())
    {
      command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
      command.Parameters.Add(new SqliteParameter("$id", targetAccountId));
      command.Parameters.Add(new SqliteParameter("$amount", amount));
      await command.ExecuteNonQueryAsync(cancellationToken);
    }
  }
  private async Task TransferAsync_DbTransaction(int sourceAccountId, int targetAccountId, int amount, CancellationToken cancellationToken)
  {
    var transaction = await _connection.BeginTransactionAsync();
    try
    {
      await this.TransferAsync_Source(sourceAccountId, targetAccountId, amount, cancellationToken);
      object result = null;
      await transaction.CommitAsync();
      return;
    }
    catch
    {
      await transaction.RollbackAsync();
      throw;
    }
  }
  private ResiliencePipelineProvider<string> _resiliencePipelineProvider;
  public Accounts(DbConnection connection, ResiliencePipelineProvider<string>? resiliencePipelineProvider = default)
  {
    this._connection = connection;
    this._resiliencePipelineProvider = resiliencePipelineProvider ?? throw new System.ArgumentNullException(nameof(resiliencePipelineProvider));
  }
}