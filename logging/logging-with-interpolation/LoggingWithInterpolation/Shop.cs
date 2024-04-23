using Microsoft.Extensions.Logging;

namespace LoggingWithInterpolation;

class Shop(ILogger<Shop> logger)
{
    private readonly ILogger<Shop> _logger = logger;

    public void Sell(Product product, decimal price)
    {
        _logger.LogInformation($"Product {product} sold for {price:productPrice}.");
    }
}

record Product(string Name)
{
    public override string ToString() => Name;
}