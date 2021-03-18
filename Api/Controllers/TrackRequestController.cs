namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class TrackRequestController : BaseController<TrackingRequest>
    {
        private readonly ITrackingRequestService _trackingRequestService;

        public TrackRequestController(ITrackingRequestService trackingRequestService)
            : base(trackingRequestService)
        {
            _trackingRequestService = trackingRequestService ?? throw new ArgumentNullException(nameof(trackingRequestService));
        }

        [HttpPost("create")]
        public async Task<long> CreateAsync(CreateTrackingRequest createTrackingRequest)
        {
            return await _trackingRequestService.CreateAsync(createTrackingRequest);
        }

        [HttpPut("step/{id}")]
        public async Task<long> UpdateStepAsync(long id, UpdateTrackingStepRequest updateTrackingRequestStep)
        {
            if (updateTrackingRequestStep == null)
            {
                throw new ArgumentNullException(nameof(updateTrackingRequestStep));
            }

            return await _trackingRequestService.UpdateStepAsync(id, updateTrackingRequestStep);
        }

        [HttpPut("complete/{id}")]
        public async Task<long> CompleteAsync(long id, CompleteTrackingRequest completeTrackingRequest)
        {
            if (completeTrackingRequest == null)
            {
                throw new ArgumentNullException(nameof(completeTrackingRequest));
            }

            return await _trackingRequestService.CompleteAsync(id, completeTrackingRequest);
        }
    }
}
