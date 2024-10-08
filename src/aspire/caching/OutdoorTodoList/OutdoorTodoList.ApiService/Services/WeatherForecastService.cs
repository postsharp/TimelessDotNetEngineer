// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;

namespace OutdoorTodoList.ApiService.Services;

public class WeatherForecastService
{
    private readonly ImmutableArray<string> _summaries =
        ["🌨️", "☀️", "🌩️", "⛈️", "🌧️", "☁️", "🌦️", "🌥️", "⛅", "🌤️"];

    public WeatherForecast[] GetWeatherForecast()
        => Enumerable.Range( 1, 5 )
            .Select(
                index =>
                    new WeatherForecast(
                        DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
                        Random.Shared.Next( -20, 55 ),
                        this._summaries[Random.Shared.Next( this._summaries.Length )] ) )
            .ToArray();
}