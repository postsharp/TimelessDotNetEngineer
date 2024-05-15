using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// [<snippet ComponentSpecificApi>]
const string httpClientName = "MyClient";

var services = new ServiceCollection()
    .AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Debug))
    .AddHttpClient(httpClientName)
    .AddStandardResilienceHandler()
    .Services
    .BuildServiceProvider();

var clientFactory = services.GetRequiredService<IHttpClientFactory>();
var client = clientFactory.CreateClient(httpClientName);
var response = await client.GetAsync("http://localhost:52394/FailEveryOtherTime");
Console.WriteLine(await response.Content.ReadAsStringAsync());
// [<endsnippet ComponentSpecificApi>]