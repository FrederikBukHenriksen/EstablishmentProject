namespace WebApplication1.Domain_Layer.Services.Repositories
{
    using System.Linq.Expressions;
    using WebApplication1.Domain_Layer.Entities;

    public interface IRepository<TCommon> where TCommon : ICommon
    {
        protected ApplicationDbContext Context { get; }

        TCommon? GetById(Guid id);
        List<TCommon> GetFromIds(List<Guid> ids);
        IEnumerable<TCommon> GetAll();
        TCommon? Find(Expression<Func<TCommon, bool>> predicate);
        IEnumerable<TCommon>? FindAll(Expression<Func<TCommon, bool>> predicate);
        void Add(TCommon entity);
        void AddRange(IEnumerable<TCommon> entities);
        void Update(TCommon entity);
        void Remove(TCommon entity);
    }
}
