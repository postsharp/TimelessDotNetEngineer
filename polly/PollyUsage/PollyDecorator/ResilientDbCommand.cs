using Polly;
using System.Data;
using System.Data.Common;

namespace PollyDecorator;

public partial class ResilientDbCommand : DbCommand
{
    private readonly DbCommand _underlyingCommand;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilientDbCommand(DbCommand underlyingCommand, ResiliencePipeline resiliencePipeline)
    {
        this._underlyingCommand = underlyingCommand;
        this._resiliencePipeline = resiliencePipeline;
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
