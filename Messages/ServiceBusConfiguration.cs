namespace ServiceBus
{
    using System.Collections.Specialized;
    using System.Configuration;
    
    public class ServiceBusConfiguration
    {
        public string Queue { get; set; }

        public string BackPlaneSqlConnectionString { get; set; }

        public string Subscriber { get; set; } = "Subscriber";

        public string BackPlaneDatabaseTableName { get; set; } = "Subscriptions";

        public string TrackingApi { get; set; }

        public string Api { get; set; }

        public ServiceBusConfiguration(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
        {
            Queue = appSettings.Get(nameof(Queue));
            Subscriber = appSettings.Get(nameof(Subscriber));
            TrackingApi = appSettings.Get(nameof(TrackingApi));
            Api = appSettings.Get(nameof(Api));
            BackPlaneSqlConnectionString = connectionStrings[nameof(BackPlaneSqlConnectionString)]?.ToString();
        }
    }
}
