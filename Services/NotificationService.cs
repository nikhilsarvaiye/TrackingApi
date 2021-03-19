namespace Services
{
    using Common;
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using FluentValidation;
    using Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Validators;

    public class NotificationService : BaseService<Notification>, INotificationService
    {
        private readonly IUserNotificationService _userNotificationService;

        public NotificationService(IAppOptions appOptions, ICacheService<Notification> cacheService, INotificationRepository storeRepository, IUserNotificationService userNotificationService)
            : base(appOptions, cacheService, storeRepository)
        {
            _userNotificationService = userNotificationService ?? throw new ArgumentNullException(nameof(userNotificationService));
        }

        public async Task<Notification> CreateAsync(CreateNotificationRequest createNotificationRequest)
        {
            var notification = await base.CreateAsync(new Notification
            {
                UserId = createNotificationRequest.UserId,
                RequestId = createNotificationRequest.RequestId,
                Type = createNotificationRequest.Type,
                Severity = createNotificationRequest.Severity,
                MetaDeta1 = createNotificationRequest.MetaDeta1,
                MetaDeta2 = createNotificationRequest.MetaDeta2,
                MetaDeta3 = createNotificationRequest.MetaDeta3,
                Content = createNotificationRequest.Content?.ToJson(),
            });

            if (createNotificationRequest.Users?.Count > 0)
            {
                var userNotifications = createNotificationRequest.Users.Select(x => new UserNotification
                {
                    IsRead = false,
                    NotificationId = notification.Id,
                    UserId = x,
                }).ToList();
                
                await _userNotificationService.BulkCreateAsync(userNotifications);
            }

            return notification;
        }

        public async Task<List<Notification>> GetByTrackingRequestIdAsync(long id)
        {
            var request = new Request
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsEqualTo,
                    Property = nameof(Notification.RequestId),
                    Value = id
                }
            };

            return (await base.GetAsync(request).ConfigureAwait(false)).ToList();
        }

        protected override async Task<Notification> OnCreating(Notification notification)
        {
            new NotificationValidator().ValidateAndThrow(notification);

            return await Task.FromResult(notification);
        }
    }
}
