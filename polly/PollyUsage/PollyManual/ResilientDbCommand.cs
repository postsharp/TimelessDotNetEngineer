using System.Data;
using System.Data.Common;

namespace PollyManual;

public class ResilientDbCommand : DbCommand
{
    private readonly DbCommand _underlying;

    public ResilientDbCommand(DbCommand underlying)
    {
        _underlying = underlying;
    }

    public override string CommandText
    {
        get => this._underlying.CommandText;
        set => this._underlying.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => this._underlying.CommandTimeout;
        set => this._underlying.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => this._underlying.CommandType;
        set => this._underlying.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => this._underlying.DesignTimeVisible;
        set => this._underlying.DesignTimeVisible = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => this._underlying.UpdatedRowSource;
        set => this._underlying.UpdatedRowSource = value;
    }
    protected override DbConnection? DbConnection
    {
        get => this._underlying.Connection;
        set => this._underlying.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => this._underlying.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => this._underlying.Transaction;
        set => this._underlying.Transaction = value;
    }

    public override void Prepare()
    {
        this._underlying.Prepare();
    }

    public override void Cancel()
    {
        this._underlying.Cancel();
    }

    public override int ExecuteNonQuery()
    {
        return this._underlying.ExecuteNonQuery();
    }

    public override object? ExecuteScalar()
    {
        return this._underlying.ExecuteScalar();
    }

    protected override DbParameter CreateDbParameter()
    {
        return this._underlying.CreateParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        return this._underlying.ExecuteReader(behavior);
    }
}
