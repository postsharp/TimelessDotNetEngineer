namespace LoggingWithInterpolation.WithMetalama;

public partial class Shop
{
    public void Sell(Product product, decimal price)
    {
    }
}

public record Product(string Name)
{
    public override string ToString() => Name;
}