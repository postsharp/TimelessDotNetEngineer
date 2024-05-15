// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Data.Common;

public class DbNotAvailableException : DbException
{
    public DbNotAvailableException() : base( "The database is not available at the moment. Please try again later." ) { }
}