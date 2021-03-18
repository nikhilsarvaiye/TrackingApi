namespace ServiceBus.Subscriber
{
    using System;
    using System.Threading.Tasks;
    using Rebus.Activation;
    using Rebus.Config;
    using Rebus.Handlers;
    using Rebus.Logging;
    using System.Configuration;
    using Models;
    using global::Subscriber.Handler;

    class Program
    {
        static void Main()
        {
            var serviceBusConfiguration = new ServiceBusConfiguration(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings); 
            
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new TrackingRequestHandler(activator));
                activator.Register(() => new DeleteUserHandler());
                activator.Register(() => new NotifyUserOnDeactivationHandler());

                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq(serviceBusConfiguration.Subscriber))
                    .Subscriptions(s => s.StoreInSqlServer(serviceBusConfiguration.BackPlaneSqlConnectionString, serviceBusConfiguration.BackPlaneDatabaseTableName, isCentralized: true))
                    .Start();

                activator.Bus.Subscribe<TrackingRequest>().Wait();

                Console.WriteLine("This is Subscriber 1");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
                Console.WriteLine("Quitting...");
            }
        }
    }
}
