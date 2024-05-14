using System.Data;
using System.Data.Common;

namespace UnreliableDb;

public class UnreliableDbCommand : DbCommand
{
    private readonly UnreliableDbConnection _parent;
    private readonly DbCommand _underlyingCommand;

    public UnreliableDbCommand(DbCommand underlyingCommand, UnreliableDbConnection parent)
    {
        _underlyingCommand = underlyingCommand;
        _parent = parent;
    }

    public override string CommandText
    {
        get => _underlyingCommand.CommandText;
        set => _underlyingCommand.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => _underlyingCommand.CommandTimeout;
        set => _underlyingCommand.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => _underlyingCommand.CommandType;
        set => _underlyingCommand.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => _underlyingCommand.DesignTimeVisible;
        set => _underlyingCommand.DesignTimeVisible = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => _underlyingCommand.UpdatedRowSource;
        set => _underlyingCommand.UpdatedRowSource = value;
    }

    protected override DbConnection? DbConnection
    {
        get => _underlyingCommand.Connection;
        set => _underlyingCommand.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => _underlyingCommand.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => _underlyingCommand.Transaction;
        set => _underlyingCommand.Transaction = value;
    }

    private void EnsureIsAvailable()
    {
        if (!_parent.IsAvailable)
        {
            throw new DbNotAvailableException();
        }
    }

    public override int ExecuteNonQuery()
    {
        EnsureIsAvailable();
        return _underlyingCommand.ExecuteNonQuery();
    }

    public override object? ExecuteScalar()
    {
        EnsureIsAvailable();
        return _underlyingCommand.ExecuteScalar();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        EnsureIsAvailable();
        return _underlyingCommand.ExecuteReader(behavior);
    }

    public override void Prepare()
    {
        _underlyingCommand.Prepare();
    }

    protected override DbParameter CreateDbParameter()
    {
        return _underlyingCommand.CreateParameter();
    }

    public override void Cancel()
    {
        _underlyingCommand.Cancel();
    }
}