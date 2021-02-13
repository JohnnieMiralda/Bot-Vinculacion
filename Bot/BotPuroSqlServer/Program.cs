using System;
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
