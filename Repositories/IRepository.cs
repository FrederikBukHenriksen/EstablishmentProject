namespace WebApplication1.Repositories
{
    using System.Linq.Expressions;
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
