using Polly;
using System.Data;
using System.Data.Common;

namespace PollyDecorator;

public class ResilientDbCommand : DbCommand
{
    private readonly DbCommand _underlyingCommand;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilientDbCommand(DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline)
    {
        this._underlyingCommand = underlyingCommand;
        this._resiliencePipeline = resiliencePipeline;
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

    public override int ExecuteNonQuery()
        => this._resiliencePipeline.Execute(this._underlyingCommand.ExecuteNonQuery);

    public override object? ExecuteScalar()
        => this._resiliencePipeline.Execute(this._underlyingCommand.ExecuteScalar);

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        => this._resiliencePipeline.Execute(() => this._underlyingCommand.ExecuteReader(behavior));

    public override void Prepare()
        => this._resiliencePipeline.Execute(this._underlyingCommand.Prepare);

    protected override DbParameter CreateDbParameter()
        => this._underlyingCommand.CreateParameter();

    public override void Cancel()
        => this._resiliencePipeline.Execute(this._underlyingCommand.Cancel);
}
