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
    using Publisher;
    using global::Publisher;

    class Program
    {
        static void Main()
        {
            var serviceBusConfiguration = new ServiceBusConfiguration(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

            using (var activator = new BuiltinHandlerActivator())
            {
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq(serviceBusConfiguration.Queue))
                    .Subscriptions(s => s.StoreInSqlServer(serviceBusConfiguration.BackPlaneSqlConnectionString, serviceBusConfiguration.BackPlaneDatabaseTableName, isCentralized: true))
                    .Start();

                // Process working
                Task.Run(async () => { 
                    await Processer.Process(serviceBusConfiguration, activator);
                });

                /* test code below */
                while (true)
                {
                    Console.WriteLine(@"Enter
                                        a) Publish
                                        q) Quit");

                    var keyChar = char.ToLower(Console.ReadKey(true).KeyChar);
                    var bus = activator.Bus.Advanced.SyncBus;

                    switch (keyChar)
                    {
                        case 'a':
                            Task.Run(async () =>
                            {
                                var tracker = new Tracker(serviceBusConfiguration.TrackingApi, activator); 
                                var request = await tracker.CreateAsync(new CreateTrackingRequest
                                {
                                    Type = TrackingRequestType.DeleteUser
                                });
                            });
                            break;

                        case 'q':
                            goto consideredHarmful;

                        default:
                            Console.WriteLine($"There's no option '{keyChar}'");
                            break;
                    }
                }

            consideredHarmful:;
                Console.WriteLine("Quitting!");
            }
        }
    }
}
