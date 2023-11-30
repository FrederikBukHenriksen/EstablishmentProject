using System.Linq.Expressions;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services.Repositories
{

    public interface IUserRolesRepository : IRepository<UserRole>
    {
        IEnumerable<UserRole> GetAllIncludeEstablishment();
    }

    public class UserRolesRepository : Repository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(IDatabaseContext context) : base(context)
        { }

        //Method is needed in the UserContextMiddleware
        public IEnumerable<UserRole> GetAllIncludeEstablishment()
        {
            return this.context.Set<UserRole>().Include(x => x.User).Include(x => x.Establishment);
        }

    }
}


