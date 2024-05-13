using Microsoft.Data.Sqlite;
using PollyMetalama;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace PollyDecorator;

internal partial class Accounts
{
    private readonly DbConnection _connection;

    public Accounts(DbConnection connection)
    {
        _connection = connection;
    }

    [RetryOnDbException]
    public async IAsyncEnumerable<Account> ListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
        }
    }

    // [<snippet AspectUsage>]
    [RetryOnDbException]
    public async Task TransferAsync(int sourceAccountId, int targetAccountId, int amount, CancellationToken cancellationToken = default)
    {
        var transaction = await _connection.BeginTransactionAsync(cancellationToken);
        try
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
                command.Parameters.Add(new SqliteParameter("$id", sourceAccountId));
                command.Parameters.Add(new SqliteParameter("$amount", amount));
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
                command.Parameters.Add(new SqliteParameter("$id", targetAccountId));
                command.Parameters.Add(new SqliteParameter("$amount", amount));
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    // [<endsnippet AspectUsage>]
}
