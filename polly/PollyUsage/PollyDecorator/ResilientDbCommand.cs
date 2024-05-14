using System.Data;
using System.Data.Common;
using Polly;

namespace PollyDecorator;

public partial class ResilientDbCommand : DbCommand
{
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly DbCommand _underlyingCommand;

    public ResilientDbCommand(DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline)
    {
        _underlyingCommand = underlyingCommand;
        _resiliencePipeline = resiliencePipeline;
    }

    public override int ExecuteNonQuery() => _resiliencePipeline.Execute(_underlyingCommand.ExecuteNonQuery);

    public override object? ExecuteScalar() => _resiliencePipeline.Execute(_underlyingCommand.ExecuteScalar);

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) 
        => _resiliencePipeline.Execute(() => _underlyingCommand.ExecuteReader(behavior));

    public override void Prepare() => _resiliencePipeline.Execute(_underlyingCommand.Prepare);

}