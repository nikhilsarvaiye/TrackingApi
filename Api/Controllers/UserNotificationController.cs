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
    public class UserNotificationController : BaseController<UserNotification>
    {
        private readonly IUserNotificationService _userNotificationService;

        public UserNotificationController(IUserNotificationService userNotificationService)
            : base(userNotificationService)
        {
            _userNotificationService = userNotificationService ?? throw new ArgumentNullException(nameof(userNotificationService));
        }

        [HttpGet("notification")]
        public async Task<List<UserNotification>> GetByNotificationIdAsync(long id)
        {
            return await _userNotificationService.GetByNotificationIdAsync(id);
        }
    }
}
