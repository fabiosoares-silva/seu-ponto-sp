var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var apikey = builder.Configuration["SPTrans:ApiKey"];

app.MapGet("/test-auth", async () =>
{
    var handler = new HttpClientHandler 
    {
        CookieContainer = new System.Net.CookieContainer(),
        UseCookies = true    
    };
    string url = $"https://api.olhovivo.sptrans.com.br/v2.1/Login/Autenticar?token={apikey}";

    var client = new HttpClient(handler);
    var response = await client.PostAsync(url, new StringContent(""));
    var content = await response.Content.ReadAsStringAsync();

    return Results.Ok(new { status = response.StatusCode, body = content });
   

})
.WithName("MeuPontoSP");

app.Run();
