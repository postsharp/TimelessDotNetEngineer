// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Patterns.Contracts;

namespace NullReferenceException.WithMetalama;

internal class Attribute
{
    // [<snippet attribute>]
    public Order? GetLastOrder( [NotNull] Customer customer )
        => customer.Orders.OrderByDescending( o => o.Date ).FirstOrDefault();
    // [<endsnippet attribute>]
}

internal class Customer
{
    public List<Order> Orders { get; } = new();
}

internal class Order
{
    public DateTime Date { get; }
}