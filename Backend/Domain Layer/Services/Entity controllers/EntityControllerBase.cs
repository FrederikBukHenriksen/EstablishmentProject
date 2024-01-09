using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.Entity_controllers
{
    public abstract class ControllerServiceBase<TEntity> : IControllerSerice<TEntity>
         where TEntity : EntityBase, new()
    {
        protected TEntity? Entity { get; set; } = null;

        public IControllerSerice<TEntity> UseExistingEntity(TEntity entity)
        {
            this.Entity = entity;
            return this;
        }
    }
}
