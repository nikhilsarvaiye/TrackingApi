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
        
        [HttpGet("request/{id}")]
        public async Task<List<Notification>> GetByTrackingRequestIdAsync(long id)
        {
            return await _notificationService.GetByTrackingRequestIdAsync(id);
        }
    }
}
