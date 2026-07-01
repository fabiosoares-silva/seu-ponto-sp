using SeuPontoSP.API.Interfaces;
using SeuPontoSP.API.Models;
using System.Net;
using System.Text.Json;

namespace SeuPontoSP.API.Services;

public class SPTransService : ISPTransService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;


    public SPTransService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["SPTrans:ApiKey"] ?? throw new Exception("SPTrans:ApiKey não configurado");
    }

    public async Task<bool> AuthenticateAsync()
    {
        var url = $"https://api.olhovivo.sptrans.com.br/v2.1/Login/Autenticar?token={_apiKey}";
        var response = await _httpClient.PostAsync(url, new StringContent(""));
        var content = await response.Content.ReadAsStringAsync();

        return content.Trim().ToLower() == "true";
    }

    public async Task<List<BusStop>> SearchBusStopAsync(string searchTerm)
    {
        var url = $"https://api.olhovivo.sptrans.com.br/v2.1/Parada/Buscar?termosBusca={searchTerm}";
        var content = await GetWithAuthRetryAsync(url);
        var response = await _httpClient.GetAsync(url);        
        
        var busStops = JsonSerializer.Deserialize<List<BusStop>>(content);

        return busStops ?? new List<BusStop>();
    }

    private async Task<string> GetWithAuthRetryAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if ( response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await AuthenticateAsync();
            response = await _httpClient.GetAsync(url);
            content = await response.Content.ReadAsStringAsync();
        }

        return content;
    }
}