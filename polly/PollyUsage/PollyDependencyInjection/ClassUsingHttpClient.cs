namespace PollyDependencyInjection;

internal class ClassUsingHttpClient
{
    public async Task<string> GetAsync(string url)
    {
        // Instantiating HttpClient without using IHttpClientFactory bypasses Polly.
        // See AvoidInstantiatingHttpClientFabric for a way to prevent this.

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
