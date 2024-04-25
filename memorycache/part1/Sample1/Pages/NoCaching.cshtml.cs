
using System.Globalization;
using System.Text.Json;

namespace Sample1.Pages
{
    public class NoCachingModel : BaseModel
    {
        public async Task<(string Symbol, string Type, decimal Rate)> GetCurrencyData(string id)
        {
            using var httpClient = new HttpClient();

            // Get the live data.
            await using var rateResponse = await httpClient.GetStreamAsync($"https://api.coincap.io/v2/rates/{id}");

            // Parse as JSON.
            var rateJson = await JsonDocument.ParseAsync(rateResponse);

            var symbol = rateJson.RootElement.GetProperty("data").GetProperty("symbol").GetString()!;
            var type = rateJson.RootElement.GetProperty("data").GetProperty("type").GetString()!;
            var rateUsd = decimal.Parse(rateJson.RootElement.GetProperty("data").GetProperty("rateUsd").GetString()!, CultureInfo.InvariantCulture);

            return (symbol, type, rateUsd);
        }
    }
}
