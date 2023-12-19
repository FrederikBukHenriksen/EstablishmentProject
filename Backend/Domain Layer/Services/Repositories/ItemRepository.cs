using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services.Repositories
{

    public interface IItemRepository : IRepository<Item>
    {
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


