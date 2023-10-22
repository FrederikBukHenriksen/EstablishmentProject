using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Repositories
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected DbContext context;
        protected IQueryable<TEntity> query;

        public Repository(DbContext Context)
        {
            context = Context;
            query = Context.Set<TEntity>().AsQueryable();
        }

        public DbContext Context { get => context; }

        public IQueryable<TEntity> Queryable { get => query; }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            SaveChanges();
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            //return context.Find<TEntity>(predicate);
            return context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
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
            context.Remove(entity);
            SaveChanges();
        }

        protected void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
