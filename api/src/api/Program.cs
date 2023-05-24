using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace api;

public static class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        await host.RunAsync();
    }
}

