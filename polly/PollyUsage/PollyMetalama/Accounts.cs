﻿using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using PollyMetalama;

namespace PollyDecorator;

internal partial class Accounts(DbConnection connection)
{
    private readonly DbConnection _connection = connection;

    [Retry]
    public async Task<IReadOnlyList<Account>> ListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

    // [<snippet AspectUsage>]
    [Retry]
    [DbTransaction]
    public async Task TransferAsync(
        int sourceAccountId,
        int targetAccountId,
        int amount,
        CancellationToken cancellationToken = default)
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
    // [<endsnippet AspectUsage>]
}