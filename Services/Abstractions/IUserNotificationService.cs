namespace Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserNotificationService : IService<UserNotification>
    {

        Task<List<UserNotification>> GetByNotificationIdAsync(long id);
    }
}
