using System.Data;
using System.Data.Common;
using Polly;

public partial class ResilientDbCommand : DbCommand
{
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly DbCommand _underlyingCommand;

    public ResilientDbCommand(DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline)
    {
        _underlyingCommand = underlyingCommand;
        _resiliencePipeline = resiliencePipeline;
    }

    public override int ExecuteNonQuery()
    {
        return _resiliencePipeline.Execute(_underlyingCommand.ExecuteNonQuery);
    }

    public override object? ExecuteScalar()
    {
        return _resiliencePipeline.Execute(_underlyingCommand.ExecuteScalar);
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        return _resiliencePipeline.Execute(() => _underlyingCommand.ExecuteReader(behavior));
    }

    public override void Prepare()
    {
        _resiliencePipeline.Execute(_underlyingCommand.Prepare);
    }
}