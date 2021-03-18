namespace Services
{
    using Configuration.Options;
    using FluentValidation;
    using Models;
    using Services.Validators;
    using System.Threading.Tasks;
    using Repositories;
    using Common;
    using System;

    public class TrackingRequestService : BaseService<TrackingRequest>, ITrackingRequestService
    {
        public TrackingRequestService(IAppOptions appOptions, ICacheService<TrackingRequest> cacheService, IUserRepository userRepository)
            : base(appOptions, cacheService, userRepository)
        {
        }

        public async Task<TrackingRequest> CreateAsync(CreateTrackingRequest createTrackingRequest)
        {
            return await base.CreateAsync(new TrackingRequest
            { 
                UserId = createTrackingRequest.UserId,
                Type = createTrackingRequest.Type,
                MetaDeta1 = createTrackingRequest.MetaDeta1,
                MetaDeta2 = createTrackingRequest.MetaDeta2,
                MetaDeta3 = createTrackingRequest.MetaDeta3,
                Content = createTrackingRequest.Content?.ToJson(),
                TotalSteps = createTrackingRequest.TotalSteps,
                CurrentStep = createTrackingRequest.CurrentStep,
                CurrentStepDescription = createTrackingRequest.CurrentStepDescription
            });
        }

        public async Task<long> UpdateStepAsync(long id, UpdateTrackingStepRequest updateTrackingStepRequest)
        {
            if (updateTrackingStepRequest == null)
            {
                throw new ArgumentNullException(nameof(updateTrackingStepRequest));
            }

            var trackingRequest = await base.GetOrThrowAsync(id).ConfigureAwait(false);

            trackingRequest.Status = TrackingRequestStatus.InProgress;
            trackingRequest.CurrentStep = updateTrackingStepRequest.Step;
            if (trackingRequest.TotalSteps <= trackingRequest.CurrentStep)
            {
                trackingRequest.TotalSteps = trackingRequest.CurrentStep;
            }
            trackingRequest.CurrentStepDescription = updateTrackingStepRequest.Description;

            return await base.UpdateAsync(id, trackingRequest).ConfigureAwait(false);
        }

        public async Task<long> CompleteAsync(long id, CompleteTrackingRequest completeTrackingRequest)
        {
            if (completeTrackingRequest == null)
            {
                throw new ArgumentNullException(nameof(completeTrackingRequest));
            }

            var trackingRequest = await base.GetOrThrowAsync(id).ConfigureAwait(false);

            trackingRequest.Status = TrackingRequestStatus.Completed;
            trackingRequest.Result = completeTrackingRequest.ResultType;
            trackingRequest.ResultDetails = completeTrackingRequest.ResultDetails?.ToJson();

            return await base.UpdateAsync(id, trackingRequest).ConfigureAwait(false);
        }

        protected override async Task<TrackingRequest> OnCreating(TrackingRequest trackingRequest)
        {
            new TrackingRequestValidator().ValidateAndThrow(trackingRequest);

            return await Task.FromResult(trackingRequest);
        }
    }
}
