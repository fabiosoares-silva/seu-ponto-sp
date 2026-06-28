namespace SeuPontoSP.API.Interfaces;

public interface ISPTransService
{
    Task<bool> AuthenticateAsync();
}