// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal static class WeatherForecastEndpointsExtensions
{
    public static WebApplication MapWeatherForecastEndpoints( this WebApplication app )
    {
        var summaries = new[]
        {
            "🌨️", "☀️", "🌩️", "⛈️", "🌧️", "☁️", "🌦️", "🌥️", "⛅", "🌤️"
        };

        app.MapGet( "/weatherforecast", () =>
        {
            var forecast = Enumerable.Range( 1, 5 ).Select( index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
                    Random.Shared.Next( -20, 55 ),
                    summaries[Random.Shared.Next( summaries.Length )]
                ) )
                .ToArray();
            return forecast;
        } );

        return app;
    }
}

record WeatherForecast( DateOnly Date, int TemperatureC, string? Summary )
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}