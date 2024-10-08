// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.Caching.Memory;

namespace OutdoorTodoList.Web;

// [<snippet HttpClientCache>]        
public partial class WeatherApiClient( HttpClient httpClient, IMemoryCache cache )
{
    private async Task<WeatherForecast[]> GetWeatherAsync(
        string endpoint,
        int maxItems,
        CancellationToken cancellationToken )
    {
        var forecast = await cache.GetOrCreateAsync(
            CacheKeyFactory.GetWeather( endpoint ),
            async _ => await httpClient.GetFromJsonAsync<WeatherForecast[]>(
                endpoint,
                cancellationToken ) );

        return forecast!.Take( maxItems ).ToArray();
    }

// [<endsnippet HttpClientCache>]        
}