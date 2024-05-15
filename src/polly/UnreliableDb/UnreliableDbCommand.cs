// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Data;
using System.Data.Common;

public class UnreliableDbCommand : DbCommand
{
    private readonly UnreliableDbConnection _parent;
    private readonly DbCommand _underlyingCommand;

    public UnreliableDbCommand( DbCommand underlyingCommand, UnreliableDbConnection parent )
    {
        this._underlyingCommand = underlyingCommand;
        this._parent = parent;
    }

    public override string CommandText
    {
        get => this._underlyingCommand.CommandText;
        set => this._underlyingCommand.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => this._underlyingCommand.CommandTimeout;
        set => this._underlyingCommand.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => this._underlyingCommand.CommandType;
        set => this._underlyingCommand.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => this._underlyingCommand.DesignTimeVisible;
        set => this._underlyingCommand.DesignTimeVisible = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => this._underlyingCommand.UpdatedRowSource;
        set => this._underlyingCommand.UpdatedRowSource = value;
    }

    protected override DbConnection? DbConnection
    {
        get => this._underlyingCommand.Connection;
        set => this._underlyingCommand.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => this._underlyingCommand.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => this._underlyingCommand.Transaction;
        set => this._underlyingCommand.Transaction = value;
    }

    private void EnsureIsAvailable()
    {
        if ( !this._parent.IsAvailable )
        {
            throw new DbNotAvailableException();
        }
    }

    public override int ExecuteNonQuery()
    {
        this.EnsureIsAvailable();

        return this._underlyingCommand.ExecuteNonQuery();
    }

    public override object? ExecuteScalar()
    {
        this.EnsureIsAvailable();

        return this._underlyingCommand.ExecuteScalar();
    }

    protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
    {
        this.EnsureIsAvailable();

        return this._underlyingCommand.ExecuteReader( behavior );
    }

    public override void Prepare()
    {
        this._underlyingCommand.Prepare();
    }

    protected override DbParameter CreateDbParameter()
    {
        return this._underlyingCommand.CreateParameter();
    }

    public override void Cancel()
    {
        this._underlyingCommand.Cancel();
    }
}