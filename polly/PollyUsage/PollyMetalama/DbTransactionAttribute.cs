using System.Data.Common;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;

public class DbTransactionAttribute : OverrideMethodAspect
{
    private static readonly DiagnosticDefinition<INamedType> _missingField = new("DBT001",
        Severity.Error,
        "The type '{0}' must have a field 'connection' of type DbConnection because of the [DbTransaction] aspect.");

    public override void BuildAspect(IAspectBuilder<IMethod> builder)
    {
        base.BuildAspect(builder);

        var connectionField = builder.Target.DeclaringType.AllFields.OfName("_connection").SingleOrDefault();
        if (connectionField == null || !connectionField.Type.Is(typeof(DbConnection)))
        {
            builder.Diagnostics.Report(_missingField.WithArguments(builder.Target.DeclaringType));
        }
    }

    public override dynamic? OverrideMethod()
    {
        var transaction = ((DbConnection)meta.This._connection).BeginTransaction();

        try
        {
            var result = meta.Proceed();
            transaction.CommitAsync();
            return result;
        }
        catch
        {
            transaction.RollbackAsync();
            throw;
        }
    }

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var transaction = await ((DbConnection)meta.This._connection).BeginTransactionAsync();

        try
        {
            var result = await meta.ProceedAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}