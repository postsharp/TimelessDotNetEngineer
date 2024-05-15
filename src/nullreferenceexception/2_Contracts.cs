// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace NullReferenceException.Contracts;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

internal class Contracts
{
    // [<snippet methods>]
    // This method requires a Customer but does not promise to return an Order.
    public Order? GetLastOrder( Customer customer ) => customer.Orders.OrderByDescending( o => o.Date ).FirstOrDefault();

    public void RepeatLastOrder( Customer customer )
    {
        var lastOrder = this.GetLastOrder( customer );

        // This will throw NullReferenceException if the customer never ordered.
        var newOrder = lastOrder.Clone( DateTime.Today );
        this.PostOrder( newOrder );
    }

    // [<endsnippet methods>]

    private void PostOrder( Order order ) { }
}

#pragma warning restore CS8602

internal class Customer
{
    public List<Order> Orders { get; } = new();
}

internal class Order( DateTime date )
{
    public DateTime Date { get; } = date;

    public Order Clone( DateTime date ) => new( date );
}