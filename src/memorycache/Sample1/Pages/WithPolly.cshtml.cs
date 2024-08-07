// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Polly;
using Polly.Registry;
using Sample1.Data;

namespace Sample1.Pages;

public class Step4Model : BaseModel
{
    // [<snippet constructor>]
    private readonly IAsyncPolicy _cachePolicy;
    private IHttpClientFactory _httpClientFactory;

    public Step4Model(
        IReadOnlyPolicyRegistry<string> policyRegistry,
        IHttpClientFactory httpClientFactory )
    {
        this._httpClientFactory = httpClientFactory;
        this._cachePolicy = policyRegistry.Get<IAsyncPolicy>( "defaultPolicy" );
    }

    // [<endsnippet constructor>]

    // [<snippet GetCurrencyData>]
    public async Task<CoinCapData> GetCurrencyData( string id )
    {
        return await this._cachePolicy.ExecuteAsync(
            async _ => await GetData(),
            new Context( $"{this.GetType().Name}.GetCurrencyData({id})" ) );

        async Task<CoinCapData> GetData()
        {
            using var httpClient = this._httpClientFactory.CreateClient();

            var response =
                await httpClient.GetFromJsonAsync<CoinCapResponse>(
                    $"https://api.coincap.io/v2/rates/{id}" );

            return response!.Data;
        }
    }

    // [<endsnippet GetCurrencyData>]
}