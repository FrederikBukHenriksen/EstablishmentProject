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
        override public List<Sale> GetFromIds(List<Guid> ids)
        {
            return this.set
                .Include(x => x.Table)
                .Include(x => x.SalesItems)
                    .ThenInclude(si => si.Item)
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public IEnumerable<Sale> GetAllSalesFromEstablishment(Guid establishmentId)
        {
            return this.set.Where(sale => sale.EstablishmentId == establishmentId);
        }

    }
}


