using Microsoft.Extensions.Logging;

namespace LoggingWithInterpolation;

class Shop(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public void Sell(Product product, decimal price)
    {
        _logger.LogInformation($"Product {product} sold for {price}.");
    }
}

class Product;