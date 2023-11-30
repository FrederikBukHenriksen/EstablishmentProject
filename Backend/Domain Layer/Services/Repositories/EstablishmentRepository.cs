using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        ICollection<Item> GetEstablishmentItems(Guid id);
        ICollection<Sale> GetEstablishmentSales(Guid id);
        ICollection<Table> GetEstablishmentTables(Guid id);


    }
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(IDatabaseContext context) : base(context)
        {
        }

        ICollection<Item> IEstablishmentRepository.GetEstablishmentItems(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Items).Where(x => x.Id == id).First().Items;
            return res;
        }

        ICollection<Sale> IEstablishmentRepository.GetEstablishmentSales(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Sales).Where(x => x.Id == id).First().Sales;
            return res;
        }

        ICollection<Table> IEstablishmentRepository.GetEstablishmentTables(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Tables).Where(x => x.Id == id).First().Tables;
            return res;
        }
    }
}