using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace SerilogInAspNetCore.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        this._logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get( int days = 5 )
    {
        // [<snippet BeginScope>]
        using ( this._logger.BeginScope( "Getting weather forecast for {ScopeDays} days", days ) )
        // [<endsnippet BeginScope>]
        {
            // [<snippet PushProperty>]
            using ( LogContext.PushProperty( "ClientHost", HttpContext.Request.Host ) )
            // [<endsnippet PushProperty>]
            {
                var today = DateOnly.FromDateTime( DateTime.Today );

                var forecast = Enumerable.Range( 1, days ).Select( index => new WeatherForecast
                {
                    Date = today.AddDays( index ),
                    Temperature = Random.Shared.Next( -20, 35 )
                } )
                .ToArray();

                // [<snippet LogMessage>]
                this._logger.LogDebug( "Returning weather forecast for the {days} days after today: {@forecast}", days, forecast );
                // [<endsnippet LogMessage>]

                return forecast;
            }
        }
    }
}
