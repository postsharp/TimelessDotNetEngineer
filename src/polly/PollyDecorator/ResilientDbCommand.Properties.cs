// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Data;
using System.Data.Common;

public partial class ResilientDbCommand
{
    public override string CommandText
    {
        get => underlyingCommand.CommandText;
        set => underlyingCommand.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => underlyingCommand.CommandTimeout;
        set => underlyingCommand.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => underlyingCommand.CommandType;
        set => underlyingCommand.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => underlyingCommand.DesignTimeVisible;
        set => underlyingCommand.DesignTimeVisible = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => underlyingCommand.UpdatedRowSource;
        set => underlyingCommand.UpdatedRowSource = value;
    }

    protected override DbConnection? DbConnection
    {
        get => underlyingCommand.Connection;
        set => underlyingCommand.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => underlyingCommand.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => underlyingCommand.Transaction;
        set => underlyingCommand.Transaction = value;
    }

    protected override DbParameter CreateDbParameter()
    {
        return underlyingCommand.CreateParameter();
    }
}