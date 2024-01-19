using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    public interface IEntityConfiguration<T> where T : class
    {
        void Configure(EntityTypeBuilder<T> builder);
    }

    public abstract class EntityConfigurationBase<T> : IEntityConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Add common configurations for all entities if needed
        }
    }
}