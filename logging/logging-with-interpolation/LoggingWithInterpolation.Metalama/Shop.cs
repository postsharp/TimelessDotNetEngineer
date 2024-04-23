namespace LoggingWithInterpolation.Metalama;

partial class Shop
{
    [Log]
    public void Sell(Product product, decimal price)
    {
    }
}

record Product(string Name)
{
    public override string ToString() => Name;
}