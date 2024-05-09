bool isOtherTime = false;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/FailEveryOtherTime", () => (isOtherTime = !isOtherTime) ? Results.StatusCode(500) : Results.Ok("Hello"));

app.Run();
