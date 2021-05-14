namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class TrackerRequestRepository : BaseSqlRepository<TrackerRequest>, ITrackerRepository
    {
        public TrackerRequestRepository(IDbOptions dbOptions)
            : base(dbOptions, "TrackerRequest")
        {
        }
    }
}
