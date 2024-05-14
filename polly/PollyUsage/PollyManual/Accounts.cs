using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace PollyManual;

internal class Accounts( 
    DbConnection connection, 
    [FromKeyedServices("db-pipeline")] ResiliencePipeline resiliencePipeline )
{
    public async IAsyncEnumerable<(int Id, string Name, int Balance)> ListAsync()
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        await using var reader = await resiliencePipeline.ExecuteAsync(
            async t => await command.ExecuteReaderAsync(t));
        while (await resiliencePipeline.ExecuteAsync(
                   async ct => await reader.ReadAsync(ct)))
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
        await resiliencePipeline.ExecuteAsync(async t =>
            {
                var transaction = await connection.BeginTransactionAsync(t);
                try
                {
                    await using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
                        command.AddParameter( "$id", sourceAccountId);
                        command.AddParameter("$amount", amount);
                        await command.ExecuteNonQueryAsync(t);
                    }

                    await using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
                        command.AddParameter( "$id", targetAccountId);
                        command.AddParameter("$amount", amount);
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