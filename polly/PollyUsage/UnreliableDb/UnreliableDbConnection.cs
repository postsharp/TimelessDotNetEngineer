﻿using System.Data;
using System.Data.Common;

namespace UnreliableDb;

public class UnreliableDbConnection : DbConnection
{
    private readonly DbConnection _underlyingConnection;

    public bool IsAvailable { get; set; } = true;

    public UnreliableDbConnection(DbConnection underlyingConnection)
    {
        this._underlyingConnection = underlyingConnection;
    }

    public override string ConnectionString
    {
        get => this._underlyingConnection.ConnectionString;
        set => this._underlyingConnection.ConnectionString = value;
    }

    public override string Database => this._underlyingConnection.Database;

    public override string DataSource => this._underlyingConnection.DataSource;

    public override string ServerVersion => this._underlyingConnection.ServerVersion;

    public override ConnectionState State => this._underlyingConnection.State;

    public override void ChangeDatabase(string databaseName)
        => this._underlyingConnection.ChangeDatabase(databaseName);

    public override void Close()
        => this._underlyingConnection.Close();

    public override void Open()
        => this._underlyingConnection.Open();

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        => this._underlyingConnection.BeginTransaction(isolationLevel);

    protected override DbCommand CreateDbCommand()
        => new UnreliableDbCommand(this._underlyingConnection.CreateCommand(), this);
}
