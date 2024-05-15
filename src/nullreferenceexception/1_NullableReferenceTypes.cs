namespace NullReferenceException.NullableReferenceTypes;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

internal class IncorrectComputePrice
{
    // [<snippet incorrect-compute-price>]
    public decimal ComputePrice(Product product, decimal quantity, Discount? discount)
    {
        // C# reports a warning on `discount.Percent` because `discount` can be null.
        return product.Price * quantity * (1 - discount.Percent / 100m);
    }
    // [<endsnippet incorrect-compute-price>]
}

#pragma warning restore CS8602

internal class CorrectComputePrice
{
    // [<snippet correct-compute-price>]
    public decimal ComputePrice(Product product, decimal quantity, Discount? discount)
    {
        return product.Price * quantity * (1 - (discount?.Percent ?? 0) / 100m);
    }
    // [<endsnippet correct-compute-price>]
}

class Product
{
    public decimal Price { get; set; }
}

class Discount
{
    public decimal Percent { get; set; }
}