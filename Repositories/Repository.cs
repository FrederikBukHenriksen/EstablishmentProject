using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml;

namespace WebApplication1.Repositories
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly DbContext Context;
        protected IQueryable<TEntity> Query;

        public Repository(DbContext Context)
        {
            this.Context = Context;
            this.Query = Context.Set<TEntity>().AsQueryable();
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            SaveChanges();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Find<TEntity>(predicate);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Query.Where(predicate).AsEnumerable();
        }

        public TEntity Get(Guid id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsEnumerable();

        }

        public void Remove(TEntity entity)
        {
            Context.Remove(entity);
            SaveChanges();
        }

        protected void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
