using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface IEntityBuilder<TEntity> where TEntity : EntityBase
    {
        public TEntity Build();
        public bool Validation();
        internal IEntityBuilder<TEntity> UseExistingEntity(TEntity entity);

        public void WritePropertiesOfEntity(TEntity entity);

        public void ReadPropertiesOfEntity(TEntity entity);
    }

    public abstract class EntityBuilderBase<TEntity> : IEntityBuilder<TEntity>
        where TEntity : EntityBase, new()
    {
        protected TEntity? Entity { get; set; } = null;

        public TEntity Build()
        {
            if (this.Validation())
            {
                if (this.Entity == null)
                {
                    this.Entity = new TEntity();
                }
                this.WritePropertiesOfEntity(this.Entity);
                return this.Entity;
            }
            throw new System.Exception("Entity did not pass builder validation");
        }


        public abstract bool Validation();
        public IEntityBuilder<TEntity> UseExistingEntity(TEntity entity)
        {
            this.Entity = entity;
            return this;
        }

        public abstract void WritePropertiesOfEntity(TEntity entity);

        public abstract void ReadPropertiesOfEntity(TEntity entity);
    }
}
