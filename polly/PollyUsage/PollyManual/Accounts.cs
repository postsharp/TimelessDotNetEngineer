using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using Polly;

namespace PollyManual;

internal class Accounts
{
    private readonly DbConnection _connection;
    private readonly ResiliencePipeline _resiliencePipeline;

    public Accounts(DbConnection connection, ResiliencePipeline resiliencePipeline)
    {
        _connection = connection;
        _resiliencePipeline = resiliencePipeline;
    }

    public async IAsyncEnumerable<(int Id, string Name, int Balance)> ListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        using var reader = await _resiliencePipeline.ExecuteAsync(
            async t => await command.ExecuteReaderAsync(t), cancellationToken);
        while (await _resiliencePipeline.ExecuteAsync(
                   async t => await reader.ReadAsync(cancellationToken)))
        {
            yield return (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
        }
    }

    // [<snippet ExecutePipeline>]
    public async Task TransferAsync(
        int sourceAccountId,
        int targetAccountId,
        int amount,
        CancellationToken cancellationToken = default)
    {
        await _resiliencePipeline.ExecuteAsync(async t =>
            {
                var transaction = await _connection.BeginTransactionAsync(t);
                try
                {
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
                        command.Parameters.Add(new SqliteParameter("$id", sourceAccountId));
                        command.Parameters.Add(new SqliteParameter("$amount", amount));
                        await command.ExecuteNonQueryAsync(t);
                    }

                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
                        command.Parameters.Add(new SqliteParameter("$id", targetAccountId));
                        command.Parameters.Add(new SqliteParameter("$amount", amount));
                        await command.ExecuteNonQueryAsync(t);
                    }

                    await transaction.CommitAsync(t);
                }
                catch
                {
                    await transaction.RollbackAsync(t);
                    throw;
                }
            },
            cancellationToken);
    }
    // [<endsnippet ExecutePipeline>]
}