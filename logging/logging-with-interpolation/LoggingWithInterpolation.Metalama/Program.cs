using LoggingWithInterpolation.Metalama;
using Microsoft.Extensions.Logging;

bool logJson = false;

using ILoggerFactory factory = LoggerFactory.Create(builder =>
{
    if (logJson)
        builder.AddJsonConsole(options => options.JsonWriterOptions = new() { Indented = true });
    else
        builder.AddSimpleConsole();
});
var logger = factory.CreateLogger<Shop>();

var shop = new Shop(logger);
shop.Sell(new Product("llama T-shirt"), 10.0m);