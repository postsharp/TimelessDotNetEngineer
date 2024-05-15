// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace NullReferenceException.NullChecking;

internal class Statements
{
    private void M( Shop shop, Cart cart, Product product, Discount discount )
    {
        // [<snippet statements>]
        // Simple null check.
        if ( product != null )
        {
            cart.Add( product );
        }

        // Pattern matching with an empty pattern checks for null and assigns to a variable.
        if ( shop.SpecialOffer is { } specialOffer )
        {
            cart.SetDiscount( specialOffer );
        }

        // Pattern matching using negated null pattern.
        if ( shop is { SpecialOffer: not null } )
        {
            cart.SetDiscount( shop.SpecialOffer );
        }

        // Null-conditional and null-coalescing operators combined to set a default value when the reference is null.
        var actualDiscount = discount?.Percent ?? 0;

        // [<endsnippet statements>]
    }
}

internal class Shop
{
    public SpecialOffer? SpecialOffer { get; set; }
}

internal class SpecialOffer;

internal class Cart
{
    internal void Add( Product product ) { }

    internal void SetDiscount( SpecialOffer specialOffer ) { }
}

internal class Product;

internal class Discount
{
    public decimal Percent { get; set; }
}