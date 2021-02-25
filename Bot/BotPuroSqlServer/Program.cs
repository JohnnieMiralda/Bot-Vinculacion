using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Topshelf;

namespace BotVinculacionUnitec
{
    internal class Program
    {
         static  void Main(string[] args)
        {
            try
            {
                var exitCode = HostFactory.Run(x =>
                {
                    x.Service<BotService>(s =>
                    {
                        s.ConstructUsing(service => new BotService());

                        s.WhenStarted(service => service.Start());
                        s.WhenStopped(service => service.Stop());

                    });
                    x.RunAsNetworkService();
                    x.SetServiceName("BotService");
                    x.SetDisplayName("My BotService");
                    x.SetDescription("This service moves files around");
                });
                int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
                var config =
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();
                Environment.ExitCode = exitCodeValue;
            }
            catch (Exception e)
            {
               // Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }



        }

    }

}
