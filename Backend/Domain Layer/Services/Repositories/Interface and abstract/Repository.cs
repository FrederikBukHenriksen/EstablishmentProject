using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain_Layer.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {

        public ApplicationDbContext context;
        public DbSet<TEntity> set;

        public Repository(ApplicationDbContext Context)
        {
            context = Context;
            set = context.Set<TEntity>();
        }

        public ApplicationDbContext Context { get => context; }

        public void Add(TEntity entity)
        {
            set.Add(entity);
            context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            set.AddRange(entities);
            context.SaveChanges();
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return set.Any(predicate);
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return set.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return set.Where(predicate).AsEnumerable();
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return set.Any(predicate);
        }

        public TEntity? GetById(Guid id)
        {
            return set.Find(id);

        }

        public IEnumerable<TEntity>? GetAll()
        {
            return set.AsEnumerable();
        }

        public List<TEntity> GetFromIds(List<Guid> ids)
        {
            return set.Where(x => ids.Contains(x.Id)).ToList();
        }

        public void Remove(TEntity entity)
        {
            set.Remove(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            set.Update(entity);
            context.SaveChanges();
        }


    }
}
