using SeuPontoSP.API.Models;

namespace SeuPontoSP.API.Interfaces;

public interface ISPTransService
{
    Task<bool> AuthenticateAsync();
    Task<List<BusStop>> SearchBusStopAsync(string searchTerm);
}