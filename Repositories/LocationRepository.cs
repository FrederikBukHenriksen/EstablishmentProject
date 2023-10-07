namespace WebApplication1.Repositories
{

    public interface ILocationRepository : IRepository<Location>
    {
    }

    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


