// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Data;
using System.Data.Common;

public static class DbCommandExtensions
{
    public static DbParameter CreateParameter( this DbCommand command, string name, object? value )
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;

        return parameter;
    }

    public static DbCommand AddParameter( this DbCommand command, string name, DbType type )
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.DbType = type;
        command.Parameters.Add( parameter );

        return command;
    }

    public static DbCommand AddParameter( this DbCommand command, string name, object? value )
    {
        var parameter = command.CreateParameter( name, value );
        command.Parameters.Add( parameter );

        return command;
    }
}