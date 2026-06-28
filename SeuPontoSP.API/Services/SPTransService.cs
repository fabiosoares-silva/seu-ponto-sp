using SeuPontoSP.API.Interfaces;
using System.Net;

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
}