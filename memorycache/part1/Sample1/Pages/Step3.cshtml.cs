using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text.Json;

namespace Sample1.Pages
{
    public class Step3Model : BaseModel
    {
        private const string _keyPrefix = "step3";

        private readonly ILogger<Step1Model> _logger;
        private readonly IMemoryCache _memoryCache;

        public Step3Model(ILogger<Step1Model> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public void OnGet()
        {

        }

        public async Task<(string Symbol, string Type, decimal Rate)> GetCurrencyData(string id)
        {
            return
                await _memoryCache.GetOrCreateAsync(
                    $"{_keyPrefix}.{id}",
                    async entry =>
                    {
                        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                        return await GetData(id);
                    });

            async Task<(string Symbol, string Type, decimal Rate)> GetData(string id)
            {
                using var httpClient = new HttpClient();

                // Get the live data.
                using var rateResponse = await httpClient.GetStreamAsync($"https://api.coincap.io/v2/rates/{id}");

                // Parse as JSON.
                var rateJson = await JsonDocument.ParseAsync(rateResponse);

                var symbol = rateJson.RootElement.GetProperty("data").GetProperty("symbol").GetString()!;
                var type = rateJson.RootElement.GetProperty("data").GetProperty("type").GetString()!;
                var rateUsd = decimal.Parse(rateJson.RootElement.GetProperty("data").GetProperty("rateUsd").GetString()!, CultureInfo.InvariantCulture);

                return (symbol, type, rateUsd);
            }
        }
    }
}
