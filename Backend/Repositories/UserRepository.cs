namespace WebApplication1.Repositories
{

    public interface IUserRepository : IRepository<User>
    {
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDatabaseContext context) : base(context)
        {
        }
    }
}


