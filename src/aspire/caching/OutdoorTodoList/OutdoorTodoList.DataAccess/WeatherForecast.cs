// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public record WeatherForecast( DateOnly Date, int TemperatureC, string? Summary )
{
    public int TemperatureF => 32 + (int) (this.TemperatureC / 0.5556);
}