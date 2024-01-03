using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        ICollection<Item> GetEstablishmentItems(Guid id);
        ICollection<Sale> GetEstablishmentSales(Guid id);
        ICollection<Table> GetEstablishmentTables(Guid id);
        IEstablishmentRepository IncludeItems();
        IEstablishmentRepository IncludeTables();
        IEstablishmentRepository IncludeSales();
    }
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        ICollection<Item> IEstablishmentRepository.GetEstablishmentItems(Guid id)
        {
            var res = this.set.Include(x => x.GetItems()).Where(x => x.Id == id).First().GetItems();
            return res;
        }

        ICollection<Sale> IEstablishmentRepository.GetEstablishmentSales(Guid id)
        {
            var res = this.set.Include(x => x.Sales).Where(x => x.Id == id).First().Sales;
            return res;
        }

        ICollection<Table> IEstablishmentRepository.GetEstablishmentTables(Guid id)
        {
            var res = this.set.Include(x => x.Tables).Where(x => x.Id == id).First().Tables;
            return res;
        }

        public IEstablishmentRepository IncludeTables()
        {
            this.set.Include(x => x.Tables);
            return this;
        }

        public IEstablishmentRepository IncludeItems()
        {
            this.set.Include(x => x.Tables);
            return this;
        }

        public IEstablishmentRepository IncludeSales()
        {
            this.set.Include(x => x.Sales);
            return this;
        }
    }
}