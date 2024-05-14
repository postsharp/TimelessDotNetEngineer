using Polly;
using Polly.Retry;
using PollyMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResiliencePipeline("middleware", pipelineBuilder =>
{
    pipelineBuilder.AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential
        })
        .ConfigureTelemetry(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()));
});

// [<snippet MiddlewareUsage>]
var app = builder.Build();
app.UseMiddleware<ResilienceMiddleware>();
// [<endsnippet MiddlewareUsage>]

app.MapGet("/", async () =>
{
    using var client = new HttpClient();
    var response = await client.GetAsync("http://localhost:52394/FailEveryOtherTime");
    response.EnsureSuccessStatusCode();
    var responseContent = await response.Content.ReadAsStringAsync();
    return Results.Ok($"The service returned {response.StatusCode}: {responseContent}");
});

app.Run();