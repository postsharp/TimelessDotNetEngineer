using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace SerilogInAspNetCore.Controllers;

// [<snippet PullDependency>]
[ApiController]
[Route( "[controller]" )]
public class WeatherForecastController( ILogger<WeatherForecastController> logger ) : ControllerBase

// [<endsnippet PullDependency>]
{
    [HttpGet( Name = "GetWeatherForecast" )]
    public IEnumerable<WeatherForecast> Get( int days = 5 )
    {
        // [<snippet BeginScope>]
        using (logger.BeginScope( "Getting weather forecast for {ScopeDays} days", days ))

            // [<endsnippet BeginScope>]
        {
            var today = DateOnly.FromDateTime( DateTime.Today );

            var forecast = Enumerable.Range( 1, days )
                .Select( index => new WeatherForecast { Date = today.AddDays( index ), Temperature = Random.Shared.Next( -20, 35 ) } )
                .ToArray();

            // [<snippet LogMessage>]
            logger.LogDebug(
                "Returning weather forecast for the {days} days after today: {@forecast}",
                days,
                forecast );

            // [<endsnippet LogMessage>]

            return forecast;
        }
    }
}