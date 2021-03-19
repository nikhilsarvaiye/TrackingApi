namespace Services
{
    using Configuration.Options;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Repositories;

    public static class ConfigurationExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, AppOptions appOptions)
        {
            services.ConfigureRepositories();

            if (true || appOptions.Cache)
            {
                services.AddScoped<ICacheService<TrackerRequest>, CacheService<TrackerRequest>>();
                services.AddScoped<ICacheService<Notification>, CacheService<Notification>>();
                services.AddScoped<ICacheService<UserNotification>, CacheService<UserNotification>>();
            }

            services.AddScoped<ITrackerRequestService, TrackerRequestService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();

            return services;
        }
    }
}
