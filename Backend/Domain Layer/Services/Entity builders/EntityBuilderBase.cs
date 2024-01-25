using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder<TEntity> where TEntity : EntityBase
    {
        public TEntity Build();
        public void ConstructEntity(TEntity entity);

    }

    public abstract class EntityBuilderBase<TEntity> : IEntityBuilder<TEntity>
        where TEntity : EntityBase, new()
    {
        protected TEntity? Entity { get; set; } = null;
        public TEntity Build()
        {
            this.ConstructEntity(this.Entity);
            return this.Entity;
        }


        public abstract void ConstructEntity(TEntity entity);

    }
}
