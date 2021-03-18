namespace Services
{
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using FluentValidation;
    using Models;
    using Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Validators;

    public class UserNotificationService : BaseService<UserNotification>, IUserNotificationService
    {
        public UserNotificationService(IAppOptions appOptions, ICacheService<UserNotification> cacheService, IUserNotificationRepository userNotificationRepository)
            : base(appOptions, cacheService, userNotificationRepository)
        {
        }

        public async Task<List<UserNotification>> GetByNotificationIdAsync(long id)
        {
            var request = new Request
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsEqualTo,
                    Property = nameof(UserNotification.NotificationId),
                    Value = id
                }
            };

            return (await base.GetAsync(request).ConfigureAwait(false)).ToList();
        }

        protected override async Task<UserNotification> OnCreating(UserNotification userNotification)
        {
            new UserNotificationValidator().ValidateAndThrow(userNotification);

            return await Task.FromResult(userNotification);
        }
    }
}
