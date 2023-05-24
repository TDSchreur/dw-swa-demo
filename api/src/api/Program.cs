using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace api;

public static class Program
{
    public static async Task Main()
    {
        IHost host = new HostBuilder()
                    .ConfigureFunctionsWorkerDefaults(builder =>
                    {
                        builder.UseNewtonsoftJson()
                              .AddApplicationInsights()
                              .AddApplicationInsightsLogger();
                    })
                    .ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        string rootPath = context.HostingEnvironment.ContentRootPath;
                        string environmentName = context.HostingEnvironment.EnvironmentName;

                        configurationBuilder
                           .SetBasePath(rootPath)
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                           .AddEnvironmentVariables();

                        if (environmentName == "Development")
                        {
                            configurationBuilder.AddUserSecrets<GetWeather>();
                        }
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddHttpClient<WeatherApiClient>();
                    })
                    .Build();

        await host.RunAsync();
    }
}

