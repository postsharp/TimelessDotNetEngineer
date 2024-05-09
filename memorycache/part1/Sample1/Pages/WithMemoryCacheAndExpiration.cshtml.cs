using Microsoft.Extensions.Caching.Memory;
using Sample1.Data;

namespace Sample1.Pages;

public class WithMemoryCacheAndExpirationModel(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
    : BaseModel
{
    // [<snippet GetCurrencyData>]
    public async Task<CoinCapData> GetCurrencyData(string id)
    {
        return
            (await memoryCache.GetOrCreateAsync(
                $"{GetType().Name}.{id}",
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                    return await GetData();
                }))!;

        async Task<CoinCapData> GetData()
        {
            using var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<CoinCapResponse>($"https://api.coincap.io/v2/rates/{id}");

            return response!.Data;
        }
    }
    // [<endsnippet GetCurrencyData>]
}