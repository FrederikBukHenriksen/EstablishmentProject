namespace WebApplication1.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        ICollection<Item> GetItems(Guid id);
        ICollection<Sale> GetSales(Guid id);

    }
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(IDatabaseContext context) : base(context)
        {
        }

        ICollection<Item> IEstablishmentRepository.GetItems(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Items).First().Items;
            return res;
        }

        ICollection<Sale> IEstablishmentRepository.GetSales(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Sales).First().Sales;
            return res;
        }
    }
}