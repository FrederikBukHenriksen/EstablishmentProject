using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.Entities;
using Xunit;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{


    public interface IEntityBuilder<T> where T : EntityBase
    {
        protected T Entity { get; set; }
        public T Build();
        protected bool EntityValidation();
        //public IEntityBuilder<T> UseExistingEntity(T entity);
        //IEntityBuilder<T> CreateNewEntity();

    }

    public abstract class EntityBuilderBase<T> : IEntityBuilder<T>
        where T : EntityBase, new()

    {

        private T _entity = new T();

        public T Entity { get => _entity; set => _entity = value; }



        public T Build()
        {
            if(this.EntityValidation())
            {
                T tempStoreEntity = Entity;
                return Entity;
            }
            else
            {
                throw new System.Exception("Entity is not valid");
            }
        }

        //public abstract IEntityBuilder<T> CreateNewEntity();

        public virtual bool EntityValidation() {
            return true;
                }

        //public abstract IEntityBuilder<T> UseExistingEntity(T entity);
    }
}
