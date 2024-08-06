// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OutdoorTodoList.ApiService.Interceptors;

public class SqlSlowDownInterceptor : IDbCommandInterceptor
{
    private static readonly AsyncLocal<bool> _isExecuting = new();

    public bool Enabled { get; set; }

    //public DbCommand CommandCreated( CommandEndEventData eventData, DbCommand result )
    //{
    //    if ( this.Enabled && !_isExecuting.Value )
    //    {
    //        _isExecuting.Value = true;

    //        try
    //        {
    //            eventData.Context!.Database.ExecuteSqlRaw( "WAITFOR DELAY '00:00:02';" );
    //        }
    //        finally
    //        {
    //            _isExecuting.Value = false;
    //        }
    //    }

    //    return result;
    //}

    public DbCommand CommandInitialized( CommandEndEventData eventData, DbCommand result )
    {
        //if ( this.Enabled /*&& !_isExecuting.Value*/ )
        //{
        //    _isExecuting.Value = true;

        //    try
        //    {
        result.CommandText = "WAITFOR DELAY '00:00:02'; " + result.CommandText;

        //    }
        //    finally
        //    {
        //        _isExecuting.Value = false;
        //    }
        //}

        return result;
    }
}