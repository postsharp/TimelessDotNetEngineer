// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.Extensions.Caching.Memory;
using Sample1.Data;

namespace Sample1.Pages;

// [<snippet constructor>]
public class WithMemoryCacheModel( IHttpClientFactory httpClientFactory, IMemoryCache memoryCache ) : BaseModel

// [<endsnippet constructor>]
{
    // [<snippet GetCurrencyData>]
    public async Task<CoinCapData> GetCurrencyData( string id )
    {
        return (await memoryCache.GetOrCreateAsync(
            $"{this.GetType().Name}.GetCurrencyData({id})",
            _ => GetData() ))!;

        async Task<CoinCapData> GetData()
        {
            using var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<CoinCapResponse>( $"https://api.coincap.io/v2/rates/{id}" );

            return response!.Data;
        }
    }

    // [<endsnippet GetCurrencyData>]
}