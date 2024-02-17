using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface IUserRepository : IRepository<User>
    {
        IUserRepository IncludeUserRoles();
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IUserRepository IncludeUserRoles()
        {
            this.query = this.query.Include(x => x.UserRoles);
            return this;
        }

    }
}


