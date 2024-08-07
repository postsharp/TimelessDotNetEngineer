﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

internal class Accounts( DbConnection connection )
{
    public async IAsyncEnumerable<(int Id, string Name, int Balance)> ListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default )
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, name, balance FROM accounts";
        await using var reader = await command.ExecuteReaderAsync( cancellationToken );

        while ( await reader.ReadAsync( cancellationToken ) )
        {
            yield return (reader.GetInt32( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ));
        }
    }

    // [<snippet NoBoilerplate>]
    public async Task TransferAsync(
        int sourceAccountId,
        int targetAccountId,
        int amount,
        CancellationToken cancellationToken = default )
    {
        var transaction = await connection.BeginTransactionAsync( cancellationToken );

        try
        {
            await using ( var command = connection.CreateCommand() )
            {
                command.CommandText =
                    "UPDATE accounts SET balance = balance - $amount WHERE id = $id";

                command.AddParameter( "$id", sourceAccountId );
                command.AddParameter( "$amount", amount );
                await command.ExecuteNonQueryAsync( cancellationToken );
            }

            await using ( var command = connection.CreateCommand() )
            {
                command.CommandText =
                    "UPDATE accounts SET balance = balance + $amount WHERE id = $id";

                command.AddParameter( "$id", targetAccountId );
                command.AddParameter( "$amount", amount );
                await command.ExecuteNonQueryAsync( cancellationToken );
            }

            await transaction.CommitAsync( cancellationToken );
        }
        catch
        {
            await transaction.RollbackAsync( cancellationToken );

            throw;
        }
    }

    // [<endsnippet NoBoilerplate>]
}