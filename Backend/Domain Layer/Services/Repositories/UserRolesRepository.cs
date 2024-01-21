using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public interface IUserRolesRepository : IRepository<UserRole>
    {
        IEnumerable<UserRole> GetAllIncludeEstablishment();
    }

    public class UserRolesRepository : Repository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(ApplicationDbContext context) : base(context)
        { }

        //Method is needed in the UserContextMiddleware
        public IEnumerable<UserRole> GetAllIncludeEstablishment()
        {
            return this.context.Set<UserRole>().Include(x => x.User).Include(x => x.Establishment);
        }

    }
}


