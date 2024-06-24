// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public record WeatherForecast( DateOnly Date, int TemperatureC, string? Summary )
{
    public int TemperatureF => 32 + (int) (this.TemperatureC / 0.5556);
}