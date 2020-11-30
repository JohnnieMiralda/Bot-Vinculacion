using System;
using System.Linq;
using Microsoft.Extensions.Hosting;//para windows service
using Microsoft.Extensions.DependencyInjection;//para windows service
using System.Threading.Tasks;//para windows service TASK
using System.Diagnostics;//para windows service para DEBUGGER


namespace BotVinculacionUnitec
{
    internal class Program
    {

         private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<BotService>();
                });

            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }

        }

    }

   

}
