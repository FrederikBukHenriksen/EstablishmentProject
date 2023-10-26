namespace WebApplication1.Repositories
{

    public interface ISalesRepository : IRepository<Sale>
    {
    }

    public class SalesRepository : GenericRepository<Sale>, ISalesRepository
    {
        public SalesRepository(IDatabaseContext context) : base(context)
        {
        }
    }
}


