﻿namespace WebApplication1.Domain.Services.Repositories
{
    using System.Linq.Expressions;
    using WebApplication1.Domain.Entities;

    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        protected ApplicationDbContext Context { get; }

        TEntity? GetById(Guid id);
        public List<TEntity> GetFromIds(List<Guid> ids);
        IEnumerable<TEntity>? GetAll();
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        TEntity? Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity>? FindAll(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
