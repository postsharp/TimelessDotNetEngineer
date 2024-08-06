// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace OutdoorTodoList.Web;

public partial class WeatherApiClient
{
    public Task<WeatherForecast[]> GetWeatherAsync(
        int maxItems = 10,
        CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast", maxItems, cancellationToken );

    public Task<WeatherForecast[]> GetCachedWeatherAsync(
        int maxItems = 10,
        CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast-cached", maxItems, cancellationToken );

    public Task<WeatherForecast[]> GetCachedByAttributeWeatherAsync(
        int maxItems = 10,
        CancellationToken cancellationToken = default )
        => this.GetWeatherAsync( "/weatherforecast-cached-attribute", maxItems, cancellationToken );
}