namespace WebApplication1.Repositories
{

    public interface ISalesRepository : IRepository<Sale>
    {
    }

    public class SalesRepository : Repository<Sale>, ISalesRepository
    {
        public SalesRepository(IDatabaseContext context) : base(context)
        {
        }
    }
}


