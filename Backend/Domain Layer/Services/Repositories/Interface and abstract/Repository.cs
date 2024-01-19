using System.Linq.Expressions;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {

        public ApplicationDbContext context;
        public DbSet<TEntity> set;

        public Repository(ApplicationDbContext Context)
        {
            this.context = Context;
            this.set = this.context.Set<TEntity>();
        }

        public ApplicationDbContext Context { get => this.context; }

        public virtual void Add(TEntity entity)
        {
            this.set.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            this.set.AddRange(entities);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Any(predicate);
        }

        public virtual TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Where(predicate).FirstOrDefault();
        }

        public virtual IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Where(predicate).AsEnumerable();
        }

        public virtual bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Any(predicate);
        }

        public virtual TEntity? GetById(Guid id)
        {
            return this.set.Find(id);

        }

        public virtual IEnumerable<TEntity>? GetAll()
        {
            return this.set.AsEnumerable();
        }

        public virtual List<TEntity> GetFromIds(List<Guid> ids)
        {
            return this.set.Where(x => ids.Any(y => y == x.Id)).ToList();
        }


        public virtual void Remove(TEntity entity)
        {
            this.set.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.set.Update(entity);
        }


    }
}
