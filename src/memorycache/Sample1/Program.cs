using System.Diagnostics;
using System.Text.RegularExpressions;
using Metalama.Patterns.Caching.Building;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;

namespace Sample1;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddHttpClient();
        builder.Services.AddRazorPages(
            options => { options.Conventions.AddPageRoute("/", "/Step1"); });

        // [<snippet AddMemoryCache>]
        builder.Services.AddMemoryCache();
        // [<endsnippet AddMemoryCache>]


        // Adds the Metalama Caching service. Only used by the Metalama example.
        // [<snippet AddMetalamaCaching>]
        builder.Services.AddMetalamaCaching();
        // [<endsnippet AddMetalamaCaching>]


        // Adds the Polly service. Only used by the Polly example.
        #if SIMPLE_POLLY 
        // [<snippet SimplePolly>]
        builder.Services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
        builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(
            serviceProvider =>
            {
                var cachingPolicy = Policy.CacheAsync(
                    serviceProvider.GetRequiredService<IAsyncCacheProvider>(),
                    TimeSpan.FromMinutes(0.5));

                var registry = new PolicyRegistry { ["defaultPolicy"] = cachingPolicy };

                return registry;
            });
        // [<endsnippet SimplePolly>]
        #else
        // [<snippet Polly>]
        builder.Services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
        builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(
            serviceProvider =>
            {
                var cachingPolicy = Policy.CacheAsync(
                    serviceProvider.GetRequiredService<IAsyncCacheProvider>(),
                    TimeSpan.FromMinutes(0.5));

                var retryPolicy = Policy.Handle<Exception>().RetryAsync();

                var policy = Policy.WrapAsync(cachingPolicy, retryPolicy);

                var registry = new PolicyRegistry { ["defaultPolicy"] = policy };

                return registry;
            });
        // [<endsnippet Polly>]

        #endif

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            var sw = new Stopwatch();
            var originalResponse = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            sw.Start();
            await next.Invoke();
            sw.Stop();

            responseBody.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(responseBody).ReadToEndAsync();

            context.Response.Body = originalResponse;

            await context.Response.WriteAsync(Regex.Replace(text, @"<render_time [a-z0-9-]* />",
                $@"<span>{sw.ElapsedMilliseconds} ms</span>"));
        });

        app.MapRazorPages();

        app.Run();
    }
}