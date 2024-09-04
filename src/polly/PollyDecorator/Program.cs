// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Data;

var services = new ServiceCollection();

// [<snippet DecoratorUsage>]
await using var connection =
    new UnreliableDbConnection( new SqliteConnection( "Data Source=:memory:" ) );

var resiliencePipeline = CreateRetryOnDbExceptionPipeline();
var resilientConnection = new ResilientDbConnection( connection, resiliencePipeline );
services.AddSingleton<DbConnection>( resilientConnection );

// [<endsnippet DecoratorUsage>]

services.AddSingleton<Accounts>();

var accounts = services.BuildServiceProvider().GetRequiredService<Accounts>();

await CreateSchemaAsync();
await PopulateDataAsync();

Console.WriteLine( "Before transfer:" );
await PrintAccountsAsync();

SimulateTemporaryFailure();

await accounts.TransferAsync( 0, 1, 100 );

Console.WriteLine();
Console.WriteLine( "After transfer:" );
await PrintAccountsAsync();

async Task CreateSchemaAsync()
{
    await using var createTableCommand = connection.CreateCommand();
    createTableCommand.CommandText = "CREATE TABLE accounts (id INT, name TEXT, balance INT)";
    await createTableCommand.ExecuteNonQueryAsync();
}

async Task PopulateDataAsync()
{
    await using var insertUserCommand = connection.CreateCommand();

    insertUserCommand.CommandText =
        "INSERT INTO accounts (id, name, balance) VALUES ($id, $name, $balance)";

    insertUserCommand.AddParameter( "$id", DbType.Int32 );
    insertUserCommand.AddParameter( "$name", DbType.String );
    insertUserCommand.AddParameter( "$balance", DbType.Int32 );

    var accounts = new (int id, string name, int balance)[] { (0, "Alice", 1000), (1, "Bob", 0) };

    foreach ( var user in accounts )
    {
        insertUserCommand.Parameters["$id"].Value = user.id;
        insertUserCommand.Parameters["$name"].Value = user.name;
        insertUserCommand.Parameters["$balance"].Value = user.balance;
        await insertUserCommand.ExecuteNonQueryAsync();
    }
}

async Task PrintAccountsAsync()
{
    await foreach ( var (id, name, balance) in accounts.ListAsync() )
    {
        Console.WriteLine( $"Id: {id}, Name: {name}, Balance: {balance}" );
    }
}

void SimulateTemporaryFailure()
{
    connection.IsAvailable = false;
    _ = Task.Delay( 1500 ).ContinueWith( _ => connection.IsAvailable = true );
}

ResiliencePipeline CreateRetryOnDbExceptionPipeline()
{
    return new ResiliencePipelineBuilder()
        .AddRetry(
            new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<DbException>(),
                Delay = TimeSpan.FromSeconds( 1 ),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential
            } )
        .ConfigureTelemetry( LoggerFactory.Create( builder => builder.AddConsole() ) )
        .Build();
}