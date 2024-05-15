using System.Data;
using System.Data.Common;

public class UnreliableDbConnection : DbConnection
{
    private readonly DbConnection _underlyingConnection;

    public UnreliableDbConnection(DbConnection underlyingConnection)
    {
        _underlyingConnection = underlyingConnection;
    }

    public bool IsAvailable { get; set; } = true;

    public override string ConnectionString
    {
        get => _underlyingConnection.ConnectionString;
        set => _underlyingConnection.ConnectionString = value;
    }

    public override string Database => _underlyingConnection.Database;

    public override string DataSource => _underlyingConnection.DataSource;

    public override string ServerVersion => _underlyingConnection.ServerVersion;

    public override ConnectionState State => _underlyingConnection.State;

    public override void ChangeDatabase(string databaseName)
    {
        _underlyingConnection.ChangeDatabase(databaseName);
    }

    public override void Close()
    {
        _underlyingConnection.Close();
    }

    public override void Open()
    {
        _underlyingConnection.Open();
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        return _underlyingConnection.BeginTransaction(isolationLevel);
    }

    protected override DbCommand CreateDbCommand()
    {
        return new UnreliableDbCommand(_underlyingConnection.CreateCommand(), this);
    }
}