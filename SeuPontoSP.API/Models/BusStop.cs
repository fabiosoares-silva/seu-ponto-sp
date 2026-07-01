using System.Text.Json.Serialization;

namespace SeuPontoSP.API.Models;

public class BusStop
{
    [JsonPropertyName("cp")]
    public int Code { get; set; }

    [JsonPropertyName("np")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("ed")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("py")]
    public double Latitude { get; set; }

    [JsonPropertyName("px")]
    public double Longitude { get; set; }
}
