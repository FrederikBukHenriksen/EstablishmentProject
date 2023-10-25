namespace WebApplication1.Repositories
{

    public interface IUserRepository : IRepository<User>
    {
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


