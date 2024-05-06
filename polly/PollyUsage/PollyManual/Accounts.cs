using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace PollyManual;

internal class Accounts
{
    private readonly DbConnection _connection;

    public Accounts(DbConnection connection)
    {
        this._connection = connection;
    }

    public async IAsyncEnumerable<(int Id, string Name, int Balance)> ListAsync()
    {
        using var command = this._connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            yield return (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
        }
    }

    public async Task TransferAsync(int sourceAccountId, int targetAccountId, int amount)
    {
        var transaction = await this._connection.BeginTransactionAsync();
        try
        {
            using (var command = this._connection.CreateCommand())
            {
                command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
                command.Parameters.Add(new SqliteParameter("$id", sourceAccountId));
                command.Parameters.Add(new SqliteParameter("$amount", amount));
                await command.ExecuteNonQueryAsync();
            }

            using (var command = this._connection.CreateCommand())
            {
                command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
                command.Parameters.Add(new SqliteParameter("$id", targetAccountId));
                command.Parameters.Add(new SqliteParameter("$amount", amount));
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
