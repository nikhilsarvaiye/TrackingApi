namespace Repositories
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigurationExtension
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserRepository, TrackingRequestRepository>();

            return services;
        }
    }
}
