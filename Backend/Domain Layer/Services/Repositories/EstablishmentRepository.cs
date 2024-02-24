using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        IEstablishmentRepository IncludeItems();
        IEstablishmentRepository IncludeTables();
        IEstablishmentRepository IncludeSalesItems();
        IEstablishmentRepository IncludeSalesTables();
        IEstablishmentRepository IncludeSales();
    }

    public class EstablishmentRepository : RepositoryBase<Establishment>, IEstablishmentRepository
    {


        public EstablishmentRepository(ApplicationDbContext context) : base(context)
        {
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

        public IEstablishmentRepository IncludeSalesTables()
        {
            this.query = this.query.Include(x => x.Sales).ThenInclude(x => x.SalesTables).ThenInclude(x => x.Table);
            return this;
        }
    }
}