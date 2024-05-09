using Microsoft.Extensions.Caching.Memory;
using Sample1.Data;

namespace Sample1.Pages;

public class WithMemoryCacheModel(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache) : BaseModel
{
    public async Task<CoinCapData> GetCurrencyData(string id)
    {
        return
            await memoryCache.GetOrCreateAsync(
                $"{GetType().Name}.GetCurrencyData({id})",
                _ => GetData());

        async Task<CoinCapData> GetData()
        {
            using var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<CoinCapResponse>($"https://api.coincap.io/v2/rates/{id}");

            return response!.Data;
        }
    }
}