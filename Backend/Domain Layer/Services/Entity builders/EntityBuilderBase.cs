using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder<T> where T : EntityBase
    {
        public T Build();
        public bool BuildValidation();
        internal IEntityBuilder<T> UseExistingEntity(T entity);
    }

    public abstract class EntityBuilderBase<TEntity> : IEntityBuilder<TEntity>
        where TEntity : EntityBase, new()
    {
        protected TEntity Entity { get; set; } = new TEntity();

        public TEntity Build()
        {
            if (this.BuildValidation())
            {
                return this.Entity;
            }
            throw new System.Exception("Entity did not pass validation");
        }
        public abstract bool BuildValidation();
        public IEntityBuilder<TEntity> UseExistingEntity(TEntity entity)
        {
            this.Entity = entity;
            return this;
        }
    }
}
