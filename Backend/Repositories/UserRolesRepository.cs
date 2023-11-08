﻿using System.Linq.Expressions;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Repositories
{

    public interface IUserRolesRepository : IRepository<UserRole>
    {
        IEnumerable<UserRole> GetAllInclude();
    }

    public class UserRolesRepository : Repository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(IDatabaseContext context) : base(context)
        { }

        //Method is needed in the UserContextMiddleware
        public IEnumerable<UserRole> GetAllInclude()
        {
            return this.context.Set<UserRole>().Include(x => x.User).Include(x => x.Establishment);
        }

    }
}


