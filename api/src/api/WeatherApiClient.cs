using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace api;

public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetWeather(string coordinates)
    {
        string key = _configuration["MapsKey"];
        HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, $"https://atlas.microsoft.com/weather/currentConditions/json?query={coordinates}");
        httpRequestMessage.Headers.Add("api-version", "1.0");
        httpRequestMessage.Headers.Add("subscription-key", key);

        HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);
        string content = await response.Content.ReadAsStringAsync();

        return content;
    }
}
