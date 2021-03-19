namespace ServiceBus.Subscriber
{
    using System;
    using Rebus.Activation;
    using Rebus.Config;
    using System.Configuration;
    using Models;
    using global::Subscriber.Handler;
    using Serilog;
    using System.Threading;

    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main()
        {
            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };
            
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
                        activator.Register(() => new DeleteUserHandler(serviceBusConfiguration, activator));
                        activator.Register(() => new NotifyUserOnDeactivationHandler(serviceBusConfiguration, activator));

                        Configure.With(activator)
                            .Logging(l => l.Serilog())
                            .Transport(t => t.UseMsmq(serviceBusConfiguration.Subscriber))
                            .Subscriptions(s => s.StoreInSqlServer(serviceBusConfiguration.BackPlaneSqlConnectionString, serviceBusConfiguration.BackPlaneDatabaseTableName, isCentralized: true))
                            // .Routing(r => r.TypeBased().Map<TrackerRequest>(""))
                            .Start();

                        activator.Bus.Subscribe<TrackerRequest>().Wait();

                        _quitEvent.WaitOne();
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
