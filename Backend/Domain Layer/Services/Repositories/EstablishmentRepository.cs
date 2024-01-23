using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        ICollection<Item> GetEstablishmentItems(Guid id);
        ICollection<Sale> GetEstablishmentSales(Guid id);
        ICollection<Table> GetEstablishmentTables(Guid id);
        IEstablishmentRepository IncludeItems();
        IEstablishmentRepository IncludeTables();
        IEstablishmentRepository IncludeSalesItems();
        IEstablishmentRepository IncludeSales();
        IEstablishmentRepository EnableLazyLoading();
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
            this.query = this.query.Include(x => x.Tables);
            return this;
        }

        public IEstablishmentRepository IncludeItems()
        {
            this.query = this.query.Include(x => x.Items);
            return this;
        }

        public IEstablishmentRepository IncludeSales()
        {
            this.query = this.query.Include(x => x.Sales);
            return this;
        }

        public IEstablishmentRepository IncludeSalesItems()
        {
            this.query = this.query.Include(x => x.Sales).ThenInclude(x => x.SalesItems).ThenInclude(x => x.Item);
            return this;
        }

        public IEstablishmentRepository EnableLazyLoading()
        {
            this.context.ChangeTracker.LazyLoadingEnabled = true;
            return this;
        }
    }
}