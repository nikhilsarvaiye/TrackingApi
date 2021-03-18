namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class UserNotificationRepository : BaseSqlRepository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(IDbOptions dbOptions)
            : base(dbOptions, "UserNotification")
        {
        }
    }
}
