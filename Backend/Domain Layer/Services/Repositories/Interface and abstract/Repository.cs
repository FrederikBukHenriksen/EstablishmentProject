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

        public void Add(TEntity entity)
        {
            this.set.Add(entity);
            this.context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.set.AddRange(entities);
            this.context.SaveChanges();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Any(predicate);
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Where(predicate).AsEnumerable();
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return this.set.Any(predicate);
        }

        public TEntity? GetById(Guid id)
        {
            return this.set.Find(id);

        }

        public IEnumerable<TEntity>? GetAll()
        {
            return this.set.AsEnumerable();
        }

        public List<TEntity> GetFromIds(List<Guid> ids)
        {
            return this.set.Where(x => ids.Contains(x.Id)).ToList();
        }

        public void Remove(TEntity entity)
        {
            this.set.Remove(entity);
            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.set.Update(entity);
            this.context.SaveChanges();
        }


    }
}
