// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Sample1.Data;

namespace Sample1.Pages;

public class NoCachingModel( IHttpClientFactory httpClientFactory ) : BaseModel
{
    public async Task<CoinCapData> GetCurrencyData( string id )
    {
        using var httpClient = httpClientFactory.CreateClient();

        var response =
            await httpClient.GetFromJsonAsync<CoinCapResponse>(
                $"https://api.coincap.io/v2/rates/{id}" );

        return response!.Data;
    }
}