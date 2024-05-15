using System.Data.Common;

public class DbNotAvailableException : DbException
{
    public DbNotAvailableException() : base("The database is not available at the moment. Please try again later.")
    {
    }
}