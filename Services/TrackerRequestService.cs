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

    public class TrackerRequestService : BaseService<TrackerRequest>, ITrackerRequestService
    {
        public TrackerRequestService(IAppOptions appOptions, ICacheService<TrackerRequest> cacheService, ITrackerRepository userRepository)
            : base(appOptions, cacheService, userRepository)
        {
        }

        public async Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest)
        {
            return await base.CreateAsync(new TrackerRequest
            { 
                UserId = createTrackerRequest.UserId,
                Type = createTrackerRequest.Type,
                MetaDeta1 = createTrackerRequest.MetaDeta1,
                MetaDeta2 = createTrackerRequest.MetaDeta2,
                MetaDeta3 = createTrackerRequest.MetaDeta3,
                Content = createTrackerRequest.Content?.ToJson(),
                TotalSteps = createTrackerRequest.TotalSteps,
                CurrentStep = createTrackerRequest.CurrentStep,
                CurrentStepDescription = createTrackerRequest.CurrentStepDescription
            });
        }

        public async Task<long> UpdateStepAsync(long id, UpdateTrackerStepRequest updateTrackerStepRequest)
        {
            if (updateTrackerStepRequest == null)
            {
                throw new ArgumentNullException(nameof(updateTrackerStepRequest));
            }

            var trackingRequest = await base.GetOrThrowAsync(id).ConfigureAwait(false);

            trackingRequest.Status = TrackerRequestStatus.InProgress;
            trackingRequest.CurrentStep = updateTrackerStepRequest.Step;
            if (trackingRequest.TotalSteps <= trackingRequest.CurrentStep)
            {
                trackingRequest.TotalSteps = trackingRequest.CurrentStep;
            }
            trackingRequest.CurrentStepDescription = updateTrackerStepRequest.Description;

            return await base.UpdateAsync(id, trackingRequest).ConfigureAwait(false);
        }

        public async Task<long> CompleteAsync(long id, CompleteTrackerRequest completeTrackerRequest)
        {
            if (completeTrackerRequest == null)
            {
                throw new ArgumentNullException(nameof(completeTrackerRequest));
            }

            var trackingRequest = await base.GetOrThrowAsync(id).ConfigureAwait(false);

            trackingRequest.Status = TrackerRequestStatus.Completed;
            trackingRequest.Result = completeTrackerRequest.ResultType;
            trackingRequest.ResultDetails = completeTrackerRequest.ResultDetails?.ToJson();

            return await base.UpdateAsync(id, trackingRequest).ConfigureAwait(false);
        }

        protected override async Task<TrackerRequest> OnCreating(TrackerRequest trackingRequest)
        {
            new TrackerRequestValidator().ValidateAndThrow(trackingRequest);

            return await Task.FromResult(trackingRequest);
        }
    }
}
