namespace WebApplication1.Repositories
{

    public interface IItemRepository : IRepository<Item>
    {
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(IDatabaseContext context) : base(context)
        {
        }
    }
}


