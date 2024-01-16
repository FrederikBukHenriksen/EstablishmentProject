using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface ISalesRepository : IRepository<Sale>
    {
        IEnumerable<Sale> GetAllSalesFromEstablishment(Guid establishmentId);

    }

    public class SalesRepository : Repository<Sale>, ISalesRepository
    {
        public SalesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Sale> GetAllSalesFromEstablishment(Guid establishmentId)
        {
            return this.set.Where(sale => sale.EstablishmentId == establishmentId);
        }

    }
}


