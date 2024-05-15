using System.Data.Common;

internal static class DbCommandExtensions
{
    public static DbParameter CreateParameter(this DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;

        return parameter;
    }

    public static DbCommand AddParameter(this DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter(name, value);
        command.Parameters.Add(parameter);
        return command;
    }
}