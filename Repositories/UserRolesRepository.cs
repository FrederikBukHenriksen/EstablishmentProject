using System.Linq.Expressions;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Repositories
{

    public interface IUserRolesRepository : IRepository<UserRole>
    {
    }

    public class UserRolesRepository : Repository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(IDatabaseContext context) : base(context)
        { }

    }
}


