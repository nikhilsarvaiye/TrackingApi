namespace ServiceBus.Publisher
{
    using System;
    using Rebus.Activation;
    using Rebus.Config;
    using Rebus.Logging;
    using System.Configuration;
    using Models;
    using SDK;
    using System.Threading.Tasks;
    using global::Publisher;
    using ServiceBus.Messages;
    using Serilog;

    class Program
    {
        static void Main()
        {
            using (var logger = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .ReadFrom.AppSettings()
                           .WriteTo.Console()
                           .CreateLogger())
            {
                Log.Logger = logger;

                Log.Logger.Information("-------Starting Application-------");

                try
                {
                    var serviceBusConfiguration = new ServiceBusConfiguration(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

                    using (var activator = new BuiltinHandlerActivator())
                    {
                        Configure.With(activator)
                            .Logging(l => l.Serilog(Log.Logger))
                            .Transport(t => t.UseMsmq(serviceBusConfiguration.Queue))
                            .Subscriptions(s => s.StoreInSqlServer(serviceBusConfiguration.BackPlaneSqlConnectionString, serviceBusConfiguration.BackPlaneDatabaseTableName, isCentralized: true))
                            .Start();

                        Task.Run(async () =>
                        {
                            await Processer.Process(serviceBusConfiguration, activator);
                        })
                        .Wait();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Something went wrong");
                }
                finally
                {
                    Log.CloseAndFlush();
                    Log.Logger.Information("-------Closing Application-------");
                }
            }
        }
    }
}
