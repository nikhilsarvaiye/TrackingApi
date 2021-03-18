namespace Services
{
    using Models;
    using System.Threading.Tasks;

    public interface ITrackingRequestService : IService<TrackingRequest>
    {
        Task<TrackingRequest> CreateAsync(CreateTrackingRequest createTrackingRequest);

        Task<long> UpdateStepAsync(long id, UpdateTrackingStepRequest updateTrackingStepRequest);

        Task<long> CompleteAsync(long id, CompleteTrackingRequest completeTrackingRequest);
    }
}
