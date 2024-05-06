using Microsoft.Data.Sqlite;
using PollyManual;
using UnreliableDb;

using var connection = new UnreliableDbConnection( new SqliteConnection($"Data Source=:memory:"));

connection.Open();

await CreateSchemaAsync();
await PopulateDataAsync();

var accounts = new Accounts(connection);

Console.WriteLine("Before transfer:");
await PrintAccountsAsync();

// Simulate a glitch in the database connection
connection.IsAvailable = false;

_ = Task.Run(async () =>
{
    await Task.Delay(1000);
    connection.IsAvailable = true;
});

await accounts.TransferAsync(0, 1, 100);

Console.WriteLine();
Console.WriteLine("After transfer:");
await PrintAccountsAsync();

async Task CreateSchemaAsync()
{
    using var createTableCommand = connection.CreateCommand();
    createTableCommand.CommandText = "CREATE TABLE accounts (id INT, name TEXT, balance INT)";
    await createTableCommand.ExecuteNonQueryAsync();
}

async Task PopulateDataAsync()
{
    using var insertUserCommand = connection.CreateCommand();
    insertUserCommand.CommandText = "INSERT INTO accounts (id, name, balance) VALUES ($id, $name, $balance)";
    insertUserCommand.Parameters.Add(new SqliteParameter("$id", SqliteType.Integer));
    insertUserCommand.Parameters.Add(new SqliteParameter("$name", SqliteType.Text));
    insertUserCommand.Parameters.Add(new SqliteParameter("$balance", SqliteType.Integer));

    var accounts = new (int id, string name, int balance)[]
    {
        (0, "Alice", 1000), (1, "Bob", 0)
    };
    
    foreach (var user in accounts)
    {
        insertUserCommand.Parameters["$id"].Value = user.id;
        insertUserCommand.Parameters["$name"].Value = user.name;
        insertUserCommand.Parameters["$balance"].Value = user.balance;
        await insertUserCommand.ExecuteNonQueryAsync();
    }
}

async Task PrintAccountsAsync()
{
    await foreach (var (id, name, balance) in accounts.ListAsync())
    {
        Console.WriteLine($"Id: {id}, Name: {name}, Balance: {balance}");
    }
}