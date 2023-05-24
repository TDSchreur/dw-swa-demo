using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace api;

public class GetWeather
{
    private readonly ILogger _logger;

    public GetWeather(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GetWeather>();
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
        _logger.LogInformation("C# HTTP trigger function processed a request");

        NameValueCollection query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string coordinates = query["coordinates"] ?? "49.2833329,-123.1200278";

        HttpRequestMessage httpRequestMessage =
            new(HttpMethod.Get, $"https://atlas.microsoft.com/weather/currentConditions/json?query={coordinates}");

        httpRequestMessage.Headers.Add("api-version", "1.0");
        httpRequestMessage.Headers.Add("subscription-key", "UcQazJQI0lKo4s2Xh1YEu-tmco7PWnXYyV-36DB7ZP4");

        HttpClient httpClient = new();
        HttpResponseMessage res = await httpClient.SendAsync(httpRequestMessage);
        // WeatherResponse weatherResponse = await res.Content.ReadFromJsonAsync<WeatherResponse>();

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(await res.Content.ReadAsStringAsync());
        return response;
    }
    //
    // public partial class WeatherResponse
    // {
    //     [JsonProperty("results")]
    //     public Result[] Results { get; init; }
    // }
    //
    // public partial class Result
    // {
    //     [JsonProperty("dateTime")]
    //     public DateTimeOffset DateTime { get; init; }
    //
    //     [JsonProperty("phrase")]
    //     public string Phrase { get; init; }
    //
    //     [JsonProperty("iconCode")]
    //     public long IconCode { get; init; }
    //
    //     [JsonProperty("hasPrecipitation")]
    //     public bool HasPrecipitation { get; init; }
    //
    //     [JsonProperty("isDayTime")]
    //     public bool IsDayTime { get; init; }
    //
    //     [JsonProperty("temperature")]
    //     public ApparentTemperature Temperature { get; init; }
    //
    //     [JsonProperty("realFeelTemperature")]
    //     public ApparentTemperature RealFeelTemperature { get; init; }
    //
    //     [JsonProperty("realFeelTemperatureShade")]
    //     public ApparentTemperature RealFeelTemperatureShade { get; init; }
    //
    //     [JsonProperty("relativeHumidity")]
    //     public long RelativeHumidity { get; init; }
    //
    //     [JsonProperty("dewPoint")]
    //     public ApparentTemperature DewPoint { get; init; }
    //
    //     [JsonProperty("wind")]
    //     public Wind Wind { get; init; }
    //
    //     [JsonProperty("windGust")]
    //     public WindGust WindGust { get; init; }
    //
    //     [JsonProperty("uvIndex")]
    //     public long UvIndex { get; init; }
    //
    //     [JsonProperty("uvIndexPhrase")]
    //     public string UvIndexPhrase { get; init; }
    //
    //     [JsonProperty("visibility")]
    //     public ApparentTemperature Visibility { get; init; }
    //
    //     [JsonProperty("obstructionsToVisibility")]
    //     public string ObstructionsToVisibility { get; init; }
    //
    //     [JsonProperty("cloudCover")]
    //     public long CloudCover { get; init; }
    //
    //     [JsonProperty("ceiling")]
    //     public ApparentTemperature Ceiling { get; init; }
    //
    //     [JsonProperty("pressure")]
    //     public ApparentTemperature Pressure { get; init; }
    //
    //     [JsonProperty("pressureTendency")]
    //     public PressureTendency PressureTendency { get; init; }
    //
    //     [JsonProperty("past24HourTemperatureDeparture")]
    //     public ApparentTemperature Past24HourTemperatureDeparture { get; init; }
    //
    //     [JsonProperty("apparentTemperature")]
    //     public ApparentTemperature ApparentTemperature { get; init; }
    //
    //     [JsonProperty("windChillTemperature")]
    //     public ApparentTemperature WindChillTemperature { get; init; }
    //
    //     [JsonProperty("wetBulbTemperature")]
    //     public ApparentTemperature WetBulbTemperature { get; init; }
    //
    //     [JsonProperty("precipitationSummary")]
    //     public Dictionary<string, ApparentTemperature> PrecipitationSummary { get; init; }
    //
    //     [JsonProperty("temperatureSummary")]
    //     public TemperatureSummary TemperatureSummary { get; init; }
    // }
    //
    // public partial class ApparentTemperature
    // {
    //     [JsonProperty("value")]
    //     public double Value { get; init; }
    //
    //     [JsonProperty("unit")]
    //     public string Unit { get; init; }
    //
    //     [JsonProperty("unitType")]
    //     public long UnitType { get; init; }
    // }
    //
    // public partial class PressureTendency
    // {
    //     [JsonProperty("localizedDescription")]
    //     public string LocalizedDescription { get; init; }
    //
    //     [JsonProperty("code")]
    //     public string Code { get; init; }
    // }
    //
    // public partial class TemperatureSummary
    // {
    //     [JsonProperty("past6Hours")]
    //     public PastHours Past6Hours { get; init; }
    //
    //     [JsonProperty("past12Hours")]
    //     public PastHours Past12Hours { get; init; }
    //
    //     [JsonProperty("past24Hours")]
    //     public PastHours Past24Hours { get; init; }
    // }
    //
    // public partial class PastHours
    // {
    //     [JsonProperty("minimum")]
    //     public ApparentTemperature Minimum { get; init; }
    //
    //     [JsonProperty("maximum")]
    //     public ApparentTemperature Maximum { get; init; }
    // }
    //
    // public partial class Wind
    // {
    //     [JsonProperty("direction")]
    //     public Direction Direction { get; init; }
    //
    //     [JsonProperty("speed")]
    //     public ApparentTemperature Speed { get; init; }
    // }
    //
    // public partial class Direction
    // {
    //     [JsonProperty("degrees")]
    //     public long Degrees { get; init; }
    //
    //     [JsonProperty("localizedDescription")]
    //     public string LocalizedDescription { get; init; }
    // }
    //
    // public partial class WindGust
    // {
    //     [JsonProperty("speed")]
    //     public ApparentTemperature Speed { get; init; }
    // }
}
