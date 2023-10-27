using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Repositories
{

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {

        public IDatabaseContext context;

        public Repository(IDatabaseContext Context)
        {
            context = Context;
        }

        public IDatabaseContext Context { get => context; }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).AsEnumerable();
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        public TEntity? Get(Guid id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity>? GetAll()
        {
            return context.Set<TEntity>().AsEnumerable();
  
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
    }
}
