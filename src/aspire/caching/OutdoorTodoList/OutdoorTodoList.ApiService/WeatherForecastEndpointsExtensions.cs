// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.AspNetCore.OutputCaching;
using OutdoorTodoList.ApiService.Services;

internal static class WeatherForecastEndpointsExtensions
{
    public static WebApplication MapWeatherForecastEndpoints( this WebApplication app )
    {
        app.MapGet(
            "/weatherforecast",
            ( WeatherForecastService forecastService )
            => forecastService.GetWeatherForecast() );

        app.MapGet(
            "/weatherforecast-cached",
            ( WeatherForecastService forecastService )
            => forecastService.GetWeatherForecast() )
            .CacheOutput( policy => policy.Expire( TimeSpan.FromSeconds( 5 ) ) );

        app.MapGet(
            "/weatherforecast-cached-attribute",
            [OutputCache( Duration = 5 )] ( WeatherForecastService forecastService )
            => forecastService.GetWeatherForecast() );

        return app;
    }
}