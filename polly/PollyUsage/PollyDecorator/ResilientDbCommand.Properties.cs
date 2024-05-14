using Polly;
using System.Data;
using System.Data.Common;

namespace PollyDecorator;

public partial class ResilientDbCommand 
{
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

}
