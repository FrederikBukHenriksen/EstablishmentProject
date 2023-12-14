using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.Entities;
using Xunit;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder<T> where T : EntityBase
    {
        public T Build();
        public bool Validation();
        internal IEntityBuilder<T> UseExistingEntity(T entity);
    }

    public abstract class EntityBuilderBase<T> : IEntityBuilder<T>
        where T : EntityBase, new()
    {
        public T? Entity { get ; set; }

        public T Build()
        {
            this.Validation();
            this.Entity ??= new T();
            this.WritePropertiesOfEntity(this.Entity);
            return this.Entity;
        }
        public abstract void WritePropertiesOfEntity(T Entity);

        public abstract bool Validation();

        public IEntityBuilder<T> UseExistingEntity(T entity)
        {
            this.Entity = entity;
            ReadPropertiesOfEntity(this.Entity);
            return this;
        }

        public abstract void ReadPropertiesOfEntity(T entity);

    }
}
