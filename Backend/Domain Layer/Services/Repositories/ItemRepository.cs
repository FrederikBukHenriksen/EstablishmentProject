using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface IItemRepository : IRepository<Item>
    {
        public IEnumerable<Item> GetAllItemsFromEstablishment(Guid establishmentId);
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Item> GetAllItemsFromEstablishment(Guid establishmentId)
        {
            return this.set.Where(sale => sale.EstablishmentId == establishmentId);
        }
    }
}


