namespace WebApplication1.Repositories
{

    public interface ISalesRepository : IRepository<Sale>
    {
    }

    public class SalesRepository : GenericRepository<Sale>, ISalesRepository
    {
        public SalesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


