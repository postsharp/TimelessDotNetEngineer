// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Data.Common;

public class DbNotAvailableException : DbException
{
    public DbNotAvailableException() : base(
        "The database is not available at the moment. Please try again later." ) { }
}