// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

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

        // [<snippet OutputCachingExtensionMethod>]
        app.MapGet(
                "/weatherforecast-cached",
                ( WeatherForecastService forecastService )
                    => forecastService.GetWeatherForecast() )
            .CacheOutput( policy => policy.Expire( TimeSpan.FromSeconds( 5 ) ) );

        // [<endsnippet OutputCachingExtensionMethod>]

        // [<snippet OutputCachingCustomAttribute>]
        app.MapGet(
            "/weatherforecast-cached-attribute",
            [OutputCache( Duration = 5 )]( WeatherForecastService forecastService )
                => forecastService.GetWeatherForecast() );

        // [<endsnippet OutputCachingCustomAttribute>]

        return app;
    }
}