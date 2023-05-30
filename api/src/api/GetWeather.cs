using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace api;

public class GetWeather
{
    private readonly ILogger _logger;
    private readonly WeatherApiClient _weatherApiClient;

    public GetWeather(WeatherApiClient weatherApiClient, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GetWeather>();
        _weatherApiClient = weatherApiClient;
    }

    [Function("GetWeather")]
    [OpenApiOperation("GetWeather")]
    [OpenApiParameter("coordinates", In = ParameterLocation.Query)]
    [OpenApiResponseWithBody(HttpStatusCode.OK,
                             "text/plain",
                             typeof(string),
                             Summary = "The response",
                             Description = "This returns the response")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(req.Url.Query);
        string coordinates = query["coordinates"];

        if (string.IsNullOrWhiteSpace(coordinates))
        {
            _logger.LogWarning("Coordinates missing from request");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        _logger.LogInformation("Getting info for coordinates {Coordinates}", coordinates);

        string weatherInfo = await _weatherApiClient.GetWeather(coordinates);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(weatherInfo);
        return response;
    }
}
