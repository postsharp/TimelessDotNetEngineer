// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Data;
using System.Data.Common;
using Polly;

public partial class ResilientDbCommand( DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline ) : DbCommand
{
    public override int ExecuteNonQuery() => resiliencePipeline.Execute( underlyingCommand.ExecuteNonQuery );

    public override object? ExecuteScalar() => resiliencePipeline.Execute( underlyingCommand.ExecuteScalar );

    protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
        => resiliencePipeline.Execute( () => underlyingCommand.ExecuteReader( behavior ) );

    public override void Prepare() => resiliencePipeline.Execute( underlyingCommand.Prepare );
    
    public override void Cancel() => resiliencePipeline.Execute( underlyingCommand.Cancel );
}