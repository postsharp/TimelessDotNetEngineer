// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Data;
using System.Data.Common;
using Polly;

public partial class ResilientDbCommand : DbCommand
{
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly DbCommand _underlyingCommand;

    public ResilientDbCommand( DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline )
    {
        this._underlyingCommand = underlyingCommand;
        this._resiliencePipeline = resiliencePipeline;
    }

    public override int ExecuteNonQuery()
    {
        return this._resiliencePipeline.Execute( this._underlyingCommand.ExecuteNonQuery );
    }

    public override object? ExecuteScalar()
    {
        return this._resiliencePipeline.Execute( this._underlyingCommand.ExecuteScalar );
    }

    protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
    {
        return this._resiliencePipeline.Execute( () => this._underlyingCommand.ExecuteReader( behavior ) );
    }

    public override void Prepare()
    {
        this._resiliencePipeline.Execute( this._underlyingCommand.Prepare );
    }
}