namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class TrackingRequestRepository : BaseSqlRepository<TrackingRequest>, IUserRepository
    {
        public TrackingRequestRepository(IDbOptions dbOptions)
            : base(dbOptions, "TrackingRequest")
        {
        }
    }
}
