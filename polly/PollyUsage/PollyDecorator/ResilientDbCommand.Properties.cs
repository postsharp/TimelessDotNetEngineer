using System.Data;
using System.Data.Common;

namespace PollyDecorator;

public partial class ResilientDbCommand
{
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
    
    protected override DbParameter CreateDbParameter() => _underlyingCommand.CreateParameter();

    public override void Cancel() => _resiliencePipeline.Execute(_underlyingCommand.Cancel);
}