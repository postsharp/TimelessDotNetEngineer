namespace OutdoorTodoList.Web;

public class WeatherApiClient( HttpClient httpClient )
{
    private async Task<WeatherForecast[]> GetWeatherAsync( string endpoint, int maxItems, CancellationToken cancellationToken )
    {
        List<WeatherForecast>? forecasts = null;

        await foreach ( var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>( endpoint, cancellationToken ) )
        {
            if ( forecasts?.Count >= maxItems )
            {
                break;
            }
            if ( forecast is not null )
            {
                forecasts ??= [];
                forecasts.Add( forecast );
            }
        }

        return forecasts?.ToArray() ?? [];
    }

    public Task<WeatherForecast[]> GetWeatherAsync( int maxItems = 10, CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast", maxItems, cancellationToken );

    public Task<WeatherForecast[]> GetCachedWeatherAsync( int maxItems = 10, CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast-cached", maxItems, cancellationToken );

    public Task<WeatherForecast[]> GetCachedByAttributeWeatherAsync( int maxItems = 10, CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast-cached-attribute", maxItems, cancellationToken );
}