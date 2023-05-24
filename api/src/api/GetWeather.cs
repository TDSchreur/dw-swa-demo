using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
    [OpenApiOperation(operationId: "GetWeather")]
    [OpenApiParameter("coordinates", In = ParameterLocation.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
                             contentType: "text/plain",
                             bodyType: typeof(string),
                             Summary = "The response",
                             Description = "This returns the response")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        NameValueCollection query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string coordinates = query["coordinates"] ?? "49.2833329,-123.1200278";

        string weatherInfo = await _weatherApiClient.GetWeather(coordinates);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(weatherInfo);
        return response;
    }


}
