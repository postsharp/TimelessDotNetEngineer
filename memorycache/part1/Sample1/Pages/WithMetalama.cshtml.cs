using Metalama.Patterns.Caching.Aspects;
using Sample1.Data;

namespace Sample1.Pages;

public partial class WithMetalamaModel(IHttpClientFactory httpClientFactory) : BaseModel
{
    // [<snippet GetCurrencyData>]
    [Cache(AbsoluteExpiration = 0.5)]
    public async Task<CoinCapData> GetCurrencyData(string id)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.GetFromJsonAsync<CoinCapResponse>($"https://api.coincap.io/v2/rates/{id}");

        return response!.Data;
    }
    // [<endsnippet GetCurrencyData>]
}