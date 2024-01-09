using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.Entity_controllers
{
    public interface IControllerSerice<TEntity> where TEntity : EntityBase
    {
        public IControllerSerice<TEntity> UseExistingEntity(TEntity entity);

    }
}
