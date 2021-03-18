namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class NotificationRepository : BaseSqlRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbOptions dbOptions)
            : base(dbOptions, "Notification")
        {
        }
    }
}
