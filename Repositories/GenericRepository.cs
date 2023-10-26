using System.Linq;
using System.Linq.Expressions;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Repositories
{

    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        public IDatabaseContext context;
        public IQueryable<TEntity> query;

        public GenericRepository(IDatabaseContext Context)
        {
            context = Context;
            query = Context.Set<TEntity>().AsQueryable();
        }

        public IDatabaseContext Context { get => context; }

        public IQueryable<TEntity> Queryable { get => query; }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return query.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.Where(predicate).AsEnumerable();
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.Any(predicate);
        }

        public TEntity? Get(Guid id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsEnumerable();

        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
    }
}
