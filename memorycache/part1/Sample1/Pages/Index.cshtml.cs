using Castle.DynamicProxy;
using Metalama.Patterns.Caching.Aspects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text.Json;

namespace Sample1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMemoryCache _memoryCache;

        private ICryptoDataSource _dataSource;

        public IndexModel(ILogger<IndexModel> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            var generator = new ProxyGenerator();
            var obj = new CryptoDataSource();
            _dataSource = generator.CreateInterfaceProxyWithTarget<ICryptoDataSource>(obj, new CachingInterceptor(memoryCache));
        }

        public void OnGet()
        {

        }

        public async Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetCryptocurrencyDataLocal()
        {
            return 
                await _memoryCache.GetOrCreateAsync(
                    "cryptoData",
                    async entry =>
                    {
                        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                        return await GetData();
                    });

            async Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetData()
            {
                var httpClient = new HttpClient();

                // Get the live data.
                var ratesResponse = await httpClient.GetStreamAsync("https://api.coincap.io/v2/rates");

                // Parse as JSON.
                var ratesJson = await JsonDocument.ParseAsync(ratesResponse);

                var rates = new List<(string Symbol, decimal Rate)>();

                // Iterate over the data.
                foreach (var currency in ratesJson.RootElement.GetProperty("data").EnumerateArray())
                {
                    var symbol = currency.GetProperty("symbol").GetString();

                    // We are interested only in BTC and Ethereum.
                    if (symbol is not "BTC" and not "ETH")
                    {
                        continue;
                    }

                    // Parse the USD rate as decimal (for brevity we assume not invalid data from the source).
                    var rate = decimal.Parse(currency.GetProperty("rateUsd").GetString()!, CultureInfo.InvariantCulture);

                    rates.Add((symbol, rate));
                }

                return rates;
            }
        }

        [Cache(AbsoluteExpiration = 0.5)]
        public async Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetCryptocurrencyDataMetalama()
        {
            var httpClient = new HttpClient();

            // Get the live data.
            var ratesResponse = await httpClient.GetStreamAsync("https://api.coincap.io/v2/rates");

            // Parse as JSON.
            var ratesJson = await JsonDocument.ParseAsync(ratesResponse);

            var rates = new List<(string Symbol, decimal Rate)>();

            // Iterate over the data.
            foreach (var currency in ratesJson.RootElement.GetProperty("data").EnumerateArray())
            {
                var symbol = currency.GetProperty("symbol").GetString();

                // We are interested only in BTC and Ethereum.
                if (symbol is not "BTC" and not "ETH")
                {
                    continue;
                }

                // Parse the USD rate as decimal (for brevity we assume not invalid data from the source).
                var rate = decimal.Parse(currency.GetProperty("rateUsd").GetString()!, CultureInfo.InvariantCulture);

                rates.Add((symbol, rate));
            }

            return rates;
        }

        public Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetCryptocurrencyDataCastle()
        {
            return _dataSource.GetCryptocurrencyData();
        }
    }

    public class CachingInterceptor : IInterceptor
    {
        private readonly IMemoryCache _memoryCache;

        public CachingInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Intercept(IInvocation invocation)
        {
            var returnTask = _memoryCache.GetOrCreateAsync(
                "cryptoData",
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                    invocation.Proceed();
                    return await (Task<IReadOnlyList<(string Symbol, decimal Rate)>>)invocation.ReturnValue;
                });

            invocation.ReturnValue = returnTask;
        }
    }

    public interface ICryptoDataSource
    {
        Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetCryptocurrencyData();
    }

    public class CryptoDataSource : ICryptoDataSource
    {
        public async Task<IReadOnlyList<(string Symbol, decimal Rate)>> GetCryptocurrencyData()
        {
            var httpClient = new HttpClient();

            // Get the live data.
            var ratesResponse = await httpClient.GetStreamAsync("https://api.coincap.io/v2/rates");

            // Parse as JSON.
            var ratesJson = await JsonDocument.ParseAsync(ratesResponse);

            var rates = new List<(string Symbol, decimal Rate)>();

            // Iterate over the data.
            foreach (var currency in ratesJson.RootElement.GetProperty("data").EnumerateArray())
            {
                var symbol = currency.GetProperty("symbol").GetString();

                // We are interested only in BTC and Ethereum.
                if (symbol is not "BTC" and not "ETH")
                {
                    continue;
                }

                // Parse the USD rate as decimal (for brevity we assume not invalid data from the source).
                var rate = decimal.Parse(currency.GetProperty("rateUsd").GetString()!, CultureInfo.InvariantCulture);

                rates.Add((symbol, rate));
            }

            return rates;
        }
    }
}
