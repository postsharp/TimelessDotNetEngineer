using PollyMiddleware;

// [<snippet MiddlewareUsage>]
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ResiliencePipelineService>();
var app = builder.Build();
app.UseMiddleware<ResilienceMiddleware>();

app.MapGet("/", async () =>
{
    using var client = new HttpClient();
    var response = await client.GetAsync("http://localhost:52394/FailEveryOtherTime");
    response.EnsureSuccessStatusCode();
    var responseContent = await response.Content.ReadAsStringAsync();
    return Results.Ok($"The service returned {response.StatusCode}: {responseContent}");
});

app.Run();
// [<endsnippet MiddlewareUsage>]