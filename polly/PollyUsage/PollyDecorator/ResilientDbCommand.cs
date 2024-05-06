using System.Data;
using System.Data.Common;

namespace PollyDecorator;

public class ResilientDbCommand : DbCommand
{
    private readonly DbCommand _underlying;

    public ResilientDbCommand(DbCommand underlying)
    {
        _underlying = underlying;
    }

    public override string CommandText
    {
        get => _underlying.CommandText;
        set => _underlying.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => _underlying.CommandTimeout;
        set => _underlying.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => _underlying.CommandType;
        set => _underlying.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => _underlying.DesignTimeVisible;
        set => _underlying.DesignTimeVisible = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => _underlying.UpdatedRowSource;
        set => _underlying.UpdatedRowSource = value;
    }
    protected override DbConnection? DbConnection
    {
        get => _underlying.Connection;
        set => _underlying.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => _underlying.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => _underlying.Transaction;
        set => _underlying.Transaction = value;
    }

    public override void Prepare()
    {
        _underlying.Prepare();
    }

    public override void Cancel()
    {
        _underlying.Cancel();
    }

    public override int ExecuteNonQuery()
    {
        return _underlying.ExecuteNonQuery();
    }

    public override object? ExecuteScalar()
    {
        return _underlying.ExecuteScalar();
    }

    protected override DbParameter CreateDbParameter()
    {
        return _underlying.CreateParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        return _underlying.ExecuteReader(behavior);
    }
}
