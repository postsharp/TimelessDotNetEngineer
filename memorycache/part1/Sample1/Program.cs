using Metalama.Patterns.Caching.Building;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Sample1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddMemoryCache();
            builder.Services.AddCaching();

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

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    sw.Start();
                    await next.Invoke();
                    sw.Stop();

                    responseBody.Seek(0, SeekOrigin.Begin);
                    var text = await new StreamReader(responseBody).ReadToEndAsync();

                    context.Response.Body = originalResponse;

                    await context.Response.WriteAsync(Regex.Replace(text, @"<render_time [a-z0-9-]* />", $@"<span>{sw.ElapsedMilliseconds} ms</span>"));
                }


            });

            app.MapRazorPages();

            app.Run();
        }
    }
}
