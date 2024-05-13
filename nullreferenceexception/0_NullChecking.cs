namespace NullReferenceException.NullChecking;

internal class Statements
{
    void M(Shop shop, Cart cart, Product product, Discount discount)
    {
        // [<snippet statements>]
        // Simple null check.
        if (product != null)
        {
            cart.Add(product);
        }

        // Pattern matching with an empty pattern checks for null and assigns to a variable.
        if (shop.SpecialOffer is { } specialOffer)
        {
            cart.SetDiscount(specialOffer);
        }

        // Pattern matching using negated null pattern.
        if (shop is { SpecialOffer: not null })
        {
            cart.SetDiscount(shop.SpecialOffer);
        }

        // Null-conditional and null-coalescing operators combined to set a default value when the reference is null.
        var actualDiscount = discount?.Percent ?? 0;
        // [<endsnippet statements>]
    }
}

class Shop
{
    public SpecialOffer? SpecialOffer { get; set; }
}

class SpecialOffer;

class Cart
{
    internal void Add(Product product) { }

    internal void SetDiscount(SpecialOffer specialOffer) { }
}

class Product;

class Discount
{
    public decimal Percent { get; set; }
}