using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder<TEntity> where TEntity : EntityBase
    {
        public TEntity Build();
    }

    public abstract class EntityBuilderBase<TEntity> : IEntityBuilder<TEntity>
        where TEntity : EntityBase
    {
        public abstract TEntity Build();
    }
}
