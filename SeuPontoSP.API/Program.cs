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

app.MapGet("/test-auth", async (ISPTransService service) =>
{
    var result = await service.AuthenticateAsync();
    return Results.Ok(result);
})
.WithName("TestAuth");

app.Run();