namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class TrackerRequestController : BaseController<TrackerRequest>
    {
        private readonly ITrackerRequestService _trackingRequestService;

        public TrackerRequestController(ITrackerRequestService trackingRequestService)
            : base(trackingRequestService)
        {
            _trackingRequestService = trackingRequestService ?? throw new ArgumentNullException(nameof(trackingRequestService));
        }

        [HttpPost("create")]
        public async Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest)
        {
            return await _trackingRequestService.CreateAsync(createTrackerRequest);
        }

        [HttpPut("step/{id}")]
        public async Task<long> UpdateStepAsync(long id, UpdateTrackerStepRequest updateTrackerRequestStep)
        {
            if (updateTrackerRequestStep == null)
            {
                throw new ArgumentNullException(nameof(updateTrackerRequestStep));
            }

            return await _trackingRequestService.UpdateStepAsync(id, updateTrackerRequestStep);
        }

        [HttpPut("complete/{id}")]
        public async Task<long> CompleteAsync(long id, CompleteTrackerRequest completeTrackerRequest)
        {
            if (completeTrackerRequest == null)
            {
                throw new ArgumentNullException(nameof(completeTrackerRequest));
            }

            return await _trackingRequestService.CompleteAsync(id, completeTrackerRequest);
        }
    }
}
