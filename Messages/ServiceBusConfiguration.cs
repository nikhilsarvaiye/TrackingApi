namespace ServiceBus
{
    using System.Collections.Specialized;
    using System.Configuration;
    
    public class ServiceBusConfiguration
    {
        public string Queue { get; set; }

        public string BackPlaneSqlConnectionString { get; set; }

        public string Subscriber { get; set; }

        public string BackPlaneDatabaseTableName { get; set; } = "Subscriptions";

        public string TrackingApi { get; set; }

        public ServiceBusConfiguration(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
        {
            Queue = appSettings.Get(nameof(Queue));
            Subscriber = appSettings.Get(nameof(Subscriber));
            TrackingApi = appSettings.Get(nameof(TrackingApi));
            BackPlaneSqlConnectionString = connectionStrings[nameof(BackPlaneSqlConnectionString)]?.ToString();
        }
    }
}
