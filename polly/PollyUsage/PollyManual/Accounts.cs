using Microsoft.Data.Sqlite;
using Polly;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace PollyManual;

internal class Accounts
{
    private readonly DbConnection _connection;
    private readonly ResiliencePipeline _resiliencePipeline;

    public Accounts(DbConnection connection, ResiliencePipeline resiliencePipeline)
    {
        this._connection = connection;
        this._resiliencePipeline = resiliencePipeline;
    }

    public async IAsyncEnumerable<(int Id, string Name, int Balance)> ListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var command = this._connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        using var reader = await this._resiliencePipeline.ExecuteAsync(
            async t => await command.ExecuteReaderAsync(t), cancellationToken);
        while (await this._resiliencePipeline.ExecuteAsync(
            async t => await reader.ReadAsync(cancellationToken)))
        {
            yield return (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
        }
    }

    // [<snippet ExecutePipeline>]
    public async Task TransferAsync(int sourceAccountId, int targetAccountId, int amount, CancellationToken cancellationToken = default)
        => await this._resiliencePipeline.ExecuteAsync(async t =>
        {
            var transaction = await this._connection.BeginTransactionAsync(t);
            try
            {
                using (var command = this._connection.CreateCommand())
                {
                    command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
                    command.Parameters.Add(new SqliteParameter("$id", sourceAccountId));
                    command.Parameters.Add(new SqliteParameter("$amount", amount));
                    await command.ExecuteNonQueryAsync(t);
                }

                using (var command = this._connection.CreateCommand())
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
    // [<endsnippet ExecutePipeline>]
}
