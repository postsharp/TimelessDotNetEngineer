using System.Data.Common;

namespace UnreliableDb;

public class DbNotAvailableException : DbException
{
    public DbNotAvailableException() : base("The database is not available at the moment. Please try again later.")
    {
    }
}