// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace NullReferenceException.Preconditions;

internal class IfStatement
{
    public Order? GetLastOrder( Customer customer )
    {
        // [<snippet if-statement>]
        if ( customer == null )
        {
            throw new ArgumentNullException( nameof(customer) );
        }

        // [<endsnippet if-statement>]

        return customer
            .Orders
            .OrderByDescending( o => o.Date )
            .FirstOrDefault();
    }
}

internal class ThrowIfNull
{
    public Order? GetLastOrder( Customer customer )
    {
        // [<snippet throwifnull>]
        ArgumentNullException.ThrowIfNull( customer );

        // [<endsnippet throwifnull>]

        return customer
            .Orders
            .OrderByDescending( o => o.Date )
            .FirstOrDefault();
    }
}

internal class ThrowExpression
{
    // [<snippet throw-expression>]
    public Order? GetLastOrder( Customer customer )
        => (customer ?? throw new ArgumentNullException( nameof(customer) ))
            .Orders
            .OrderByDescending( o => o.Date )
            .FirstOrDefault();

    // [<endsnippet throw-expression>]
}

internal class Customer
{
    public List<Order> Orders { get; } = new();
}

internal class Order
{
    public DateTime Date { get; }
}