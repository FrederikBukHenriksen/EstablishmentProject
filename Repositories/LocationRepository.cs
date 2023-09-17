using WebApplication1.Models;

namespace WebApplication1.Repositories
{  
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


