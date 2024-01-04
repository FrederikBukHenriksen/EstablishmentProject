using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder2<TEntity> where TEntity : EntityBase
    {
        public TEntity Build();
        public bool BuildValidation();
        public IEntityBuilder2<TEntity> UseExistingEntity(TEntity entity);

    }

    public abstract class EntityBuilderBase2<TEntity> : IEntityBuilder2<TEntity>
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
        public IEntityBuilder2<TEntity> UseExistingEntity(TEntity entity)
        {
            this.Entity = entity;
            return this;
        }
    }
}
