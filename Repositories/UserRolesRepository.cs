using System.Linq.Expressions;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Repositories
{

    public interface IUserRolesRepository : IRepository<UserRole>
    {
    }

    public class UserRolesRepository : GenericRepository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(IDatabaseContext context) : base(context)
        { }

    }
}


