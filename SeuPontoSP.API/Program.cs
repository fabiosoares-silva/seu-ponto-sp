using SeuPontoSP.API.Interfaces;
using SeuPontoSP.API.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ISPTransService, SPTransService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.Add("User-Agent", "SeuPontoSP");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    CookieContainer = new CookieContainer(),
    UseCookies = true
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", async (ISPTransService service) =>
{
    var authenticated = await service.AuthenticateAsync();

    return authenticated
        ? Results.Ok(new { status = "healthy", sptrans = "online" })
        : Results.Json(new { status = "unhealthy", sptrans = "offline" }, statusCode: 503);
}).
WithName("HealthCheck");

app.MapGet("/bus-stops/search", async (string searchTerm, ISPTransService service) =>
{
    var busStops = await service.SearchBusStopAsync(searchTerm);
    return Results.Ok(busStops);

}).
WithName("SearchBusStops");

app.Run();