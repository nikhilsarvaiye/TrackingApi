namespace Services
{
    using Models;
    using System.Threading.Tasks;

    public interface ITrackerRequestService : IService<TrackerRequest>
    {
        Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest);

        Task<long> UpdateStepAsync(long id, UpdateTrackerStepRequest updateTrackerStepRequest);

        Task<long> CompleteAsync(long id, CompleteTrackerRequest completeTrackerRequest);
    }
}
