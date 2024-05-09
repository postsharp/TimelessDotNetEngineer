using Polly;
using Polly.Registry;
using Sample1.Data;

namespace Sample1.Pages;

public class Step4Model : BaseModel
{
    private readonly IAsyncPolicy _cachePolicy;
    private IHttpClientFactory _httpClientFactory;

    public Step4Model(IReadOnlyPolicyRegistry<string> policyRegistry, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _cachePolicy = policyRegistry.Get<IAsyncPolicy>("defaultPolicy");
    }

    public async Task<CoinCapData> GetCurrencyData(string id)
    {
        return await _cachePolicy.ExecuteAsync(async _ => await GetData(),
            new Context($"{GetType().Name}.GetCurrencyData({id})"));

        async Task<CoinCapData> GetData()
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<CoinCapResponse>($"https://api.coincap.io/v2/rates/{id}");

            return response!.Data;
        }
    }
}