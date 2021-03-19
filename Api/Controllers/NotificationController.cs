namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class NotificationController : BaseController<Notification>
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
            : base(notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        [HttpPost("create")]
        public async Task<Notification> CreateAsync(CreateNotificationRequest createNotificationRequest)
        {
            return await _notificationService.CreateAsync(createNotificationRequest);
        }

        [HttpGet("request")]
        public async Task<List<Notification>> GetByTrackerRequestIdAsync(long id)
        {
            return await _notificationService.GetByTrackerRequestIdAsync(id);
        }
    }
}
