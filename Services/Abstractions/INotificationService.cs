namespace Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationService : IService<Notification>
    {
        Task<List<Notification>> GetByTrackingRequestIdAsync(long id);
    }
}
