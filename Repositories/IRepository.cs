namespace WebApplication1.Repositories
{
    using System.Linq.Expressions;
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        protected IDatabaseContext Context { get; }

        TEntity? Get(Guid id);
        IEnumerable<TEntity>? GetAll();
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        TEntity? Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
