using System.Linq.Expressions;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public ApplicationDbContext context;
        public DbSet<TEntity> set;
        public IQueryable<TEntity> query;

        public Repository(ApplicationDbContext Context)
        {
            this.context = Context;
            this.set = this.context.Set<TEntity>();
            this.query = this.set.AsQueryable();
        }

        public ApplicationDbContext Context => this.context;
        public IQueryable<TEntity> Query => this.query;

        public virtual void Add(TEntity entity)
        {
            this.set.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            this.set.AddRange(entities);
        }
        public virtual TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.Where(predicate).FirstOrDefault();
        }

        public virtual IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.Where(predicate).AsEnumerable();
        }

        public virtual TEntity? GetById(Guid id)
        {
            return this.query.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<TEntity>? GetAll()
        {
            return this.query.AsEnumerable();
        }

        public virtual List<TEntity> GetFromIds(List<Guid> ids)
        {
            return this.query.Where(x => ids.Contains(x.Id)).ToList();
        }

        public virtual void Remove(TEntity entity)
        {
            this.set.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.set.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;

        }

        public IRepository<TEntity> EnableLazyLoading()
        {
            this.context.ChangeTracker.LazyLoadingEnabled = true;
            return this;
        }
    }
}
