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

    public class NotificationService : BaseService<Notification>, INotificationService
    {
        public NotificationService(IAppOptions appOptions, ICacheService<Notification> cacheService, INotificationRepository storeRepository)
            : base(appOptions, cacheService, storeRepository)
        {
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
