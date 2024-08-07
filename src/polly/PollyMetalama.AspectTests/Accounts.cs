﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Data.Common;
using Microsoft.Data.Sqlite;

internal class Accounts( DbConnection connection )
{
    private readonly DbConnection _connection = connection;

    [Retry]
    public async Task<IReadOnlyList<Account>> ListAsync(
        CancellationToken cancellationToken = default )
    {
        var list = new List<Account>();
        await using var command = this._connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        await using var reader = await command.ExecuteReaderAsync( cancellationToken );

        while ( await reader.ReadAsync( cancellationToken ) )
        {
            list.Add(
                new Account( reader.GetInt32( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ) ) );
        }

        return list;
    }

    [Retry]
    [DbTransaction]
    public async Task TransferAsync(
        int sourceAccountId,
        int targetAccountId,
        int amount,
        CancellationToken cancellationToken = default )
    {
        await using ( var command = this._connection.CreateCommand() )
        {
            command.CommandText = "UPDATE accounts SET balance = balance - $amount WHERE id = $id";
            command.AddParameter( "$id", sourceAccountId );
            command.AddParameter( "$amount", amount );
            await command.ExecuteNonQueryAsync( cancellationToken );
        }

        await using ( var command = this._connection.CreateCommand() )
        {
            command.CommandText = "UPDATE accounts SET balance = balance + $amount WHERE id = $id";
            command.AddParameter( "$id", targetAccountId );
            command.AddParameter( "$amount", amount );
            await command.ExecuteNonQueryAsync( cancellationToken );
        }
    }
}