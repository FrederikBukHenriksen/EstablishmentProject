﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<TEntity> set;

        public Repository(IDatabaseContext Context)
        {
            context = Context;
            set = context.Set<TEntity>();
        }

        public IDatabaseContext Context { get => context; }

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

        public TEntity? Get(Guid id)
        {
            return set.Find(id);

        }

        public IEnumerable<TEntity>? GetAll()
        {
            return set.AsEnumerable();
            
  
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
